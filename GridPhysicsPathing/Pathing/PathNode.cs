using System;
using PhysicsAStar.Parts;

namespace PhysicsAStar.Pathing
{
    public class PathNode : IComparable<PathNode>
    {
        
        public PathNode(Node node, PathNode cameFrom, Node lastSafe, int distSoFar, int distFromEnd, int depth)
        {
            Node = node;
            CameFrom = cameFrom;
            LastSafe = lastSafe;
            DistSoFar = distSoFar;
            DistFromEnd = distFromEnd;
            Depth = depth;
            ExpectedTotalTime = DistSoFar + DistFromEnd;
        }
        
        public Node Node { get; }

        public PathNode CameFrom { get; private set; } //The previous node on this path

        public Node LastSafe { get; set; } //The last "safe" node this path has visited

        public int Depth { get; } //How many "safe" positions the path has been through to reach this point

        public int DistSoFar { get; } //The distance from the starting node

        public int DistFromEnd { get; } //The distance from the ending node

        public int ExpectedTotalTime { get; } //The expected total time of the path
        
        public int CompareTo(PathNode other) => ExpectedTotalTime.CompareTo(other.ExpectedTotalTime);
 
        public override string ToString() => $"📍{{{Node.position.x}, {Node.position.y}}}, ⏱~{ExpectedTotalTime}, Depth {Depth}";

    }
}