using System;
using System.Collections.Generic;
using System.Net;
using PhysicsAStar.Parts;
using PhysicsAStar.Physics;
using PhysicsAStar.Primitives;

namespace PhysicsAStar.Pathing
{
    public class Pathfinder
    {
        private readonly MinHeap<PathNode> Interesting;
        private readonly List<Dictionary<Node, PathNode>> Visited;

        private PathNode NodeClosestToGoal;

        private ICheckNodes NodeChecker;

        public Pathfinder()
        {
            this.Interesting = new MinHeap<PathNode>();
            this.Visited = new List<Dictionary<Node, PathNode>>();
            this.NodeChecker = null;
        }
        
        public Path FindPath(GridVec start, GridVec end, Grid grid)
        {
            var startNode = grid.GetNode(start);
            var endNode = grid.GetNode(end);

            return FindPath(startNode, endNode);
        }

        //Main pathfinding loop, explained in detail.
        public Path FindPath(Node start, Node end)
        {
            //Resets the pathfinder and adds the starting node to the open set
            ResetState();
            AddFirstNode(start, end);

            while (this.Interesting.Count > 0) //Loops while we have not reached the end and still have nodes to look at
            {
                PathNode current = this.Interesting.Extract(); //Gets the best node that is currently available
                
                if (GoalReached(end, current)) //Checks if the node is the end, and if it is, ends the pathfinding
                {
                    return ConstructPath(current, end);
                }

                UpdateNodeClosestToGoal(current); //Checks whether the node is closer to the goal than the current closest found, and updates the current closest if so.

                foreach (Node adjacent in current.Node.Surrounding) //Loops though all nodes surrounding this node
                {
                    if(adjacent == null) continue; //Skips this iteration if the adjacent node is null. This is required due to the adjacent nodes being indexed.

                    int distSoFar = current.DistSoFar + Dist(current.Node, adjacent); //Calculates the distances we have travelled so far

                    int depth = current.Depth; //Gets the current node depth (how many safe nodes the path has visited getting to it) for later.

                    bool safe = false; //Creates a variable to store whether the node is safe in, defaulted to false.

                    if (NodeChecker != null) //Makes sure the node checker is not null. If it is, we ignore the checks and the pathing acts as if it's normal A*
                    {
                        if (!NodeChecker.NodeValid(adjacent, current.Node, current.LastSafe)) continue; //Skips this iteration if we can't access the adjacent node
                        
                        if (NodeChecker.NodeSafe(adjacent)) //Checks if the node is safe, and updates the depth and safe variables if it is.
                        {
                            depth++;
                            safe = true;
                        }
                    }
                    
                    PathNode nodeOut; //Creates a variable to store the node from the already visited nodes, if it exists
                    
                    if (Visited.Count - 1 >= current.Depth && Visited[current.Depth].TryGetValue(adjacent, out nodeOut)) //Tries to get the node from the known nodes
                    {
                        UpdateNode(nodeOut, current, end, safe, distSoFar, depth); //Updates the node if it already exists
                    }
                    else
                    {
                        InsertNode(adjacent, current, end, safe, distSoFar, depth); //Creates a new PathNode if it doesn't exist
                    }

                }//Foreach loop end

            }//While loop end
            
            return ConstructPath(NodeClosestToGoal, end); //Constructs the path once we exit the loop, in the case that we didn't manage to reach the goal
            
        }

        public void SetNodeChecker(ICheckNodes checker)
        {
            NodeChecker = checker;
        }

        private Path ConstructPath(PathNode current, Node end) //Runs through the nodes in reverse order to get the path
        {
            List<Node> nodes = new List<Node>();

            nodes.Add(current.Node);
            
            PathNode from = current.CameFrom;
            
            while(from.CameFrom != null)
            {
                nodes.Add(from.Node);
                from = from.CameFrom;
            }
            
            nodes.Reverse();

            return new Path(nodes, current.Node == end);
            
        }

        private void UpdateNode(PathNode node, PathNode from, Node end, bool safe, int distSoFar, int depth) //Updates a already existing PathNode.. except really it just replaces it if it needs to
        {
            if (node.DistSoFar > distSoFar)
            {
                Interesting.Remove(node);
                InsertNode(node.Node, from, end, safe, distSoFar, depth);
            }
        }
        
        private void InsertNode(Node node, PathNode from, Node end, bool safe, int distSoFar, int depth) //Creates a new PathNode for a grid node at a certain depth
        {
            
            var pathNode = new PathNode(node, from, safe ? node : from.LastSafe, distSoFar, Dist(node, end), depth);

            Interesting.Insert(pathNode);
            
            if (Visited.Count - 1 >= depth)
            {
                Visited[depth][node] = pathNode;
            }
            else
            {
                Visited.Add(new Dictionary<Node, PathNode>()); //I feel like the Visited array could be much, much more efficient by not creating a new dictionary for each depth layer...
                Visited[depth][node] = pathNode;
            }
        }

        private bool GoalReached(Node goal, PathNode current) => current.Node == goal;

        private void AddFirstNode(Node start, Node goal) //Adds the first PathNode to the arrays
        {
            var head = new PathNode(start, null, start, 0,Dist(start, goal), 0);
            Interesting.Insert(head);
            Visited.Add(new Dictionary<Node, PathNode>());
            Visited[0].Add(start, head);
            NodeClosestToGoal = head;
        }

        private void ResetState() //Clears the arrays and resets the NodeClosestToGoal
        {
            Interesting.Clear();
            Visited.Clear();
            NodeClosestToGoal = null;
        }
        
        private void UpdateNodeClosestToGoal(PathNode current)
        {
            if (current.DistFromEnd < this.NodeClosestToGoal.DistFromEnd)
            {
                this.NodeClosestToGoal = current;
            }
        }
        
        private int Dist(Node a, Node b) //Simple Manhatten distance calculation
        {
            return Math.Abs(a.position.x - b.position.x) + Math.Abs(a.position.y - b.position.y);
        }
    }
}