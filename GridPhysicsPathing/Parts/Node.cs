using PhysicsAStar.Primitives;

namespace PhysicsAStar.Parts
{
    public class Node
    {
        public Node[] Surrounding => _surrounding;

        public GridVec position;

        public Node(GridVec position)
        {
            this.position = position;
        }
        
        //Initialised with nulls because we need to know the relative position of each node
        //All the positional properties are below. This makes physics checks much easier, since
        //we can get nodes relative to this one very easily (ex - checking if there is a space underneath us to apply gravity)
        //This has the downside of requiring us to check whether each node in the surrounding nodes is null or not when pathfinding

        private Node[] _surrounding = { null, null, null, null, null, null, null, null };

        public Node Up 
        {
            get => _surrounding[0];
            set => _surrounding[0] = value;
        }
        
        public Node Down
        {
            get => _surrounding[1];
            set => _surrounding[1] = value;
        }
        
        public Node Left
        {
            get => _surrounding[2];
            set => _surrounding[2] = value;
        }
        
        public Node Right
        {
            get => _surrounding[3];
            set => _surrounding[3] = value;
        }
        
        public Node UpRight
        {
            get => _surrounding[4];
            set => _surrounding[4] = value;
        }
        
        public Node UpLeft
        {
            get => _surrounding[5];
            set => _surrounding[5] = value;
        }
        
        public Node DownRight
        {
            get => _surrounding[6];
            set => _surrounding[6] = value;
        }
        
        public Node DownLeft
        {
            get => _surrounding[7];
            set => _surrounding[7] = value;
        }

        //Using nulls for the surrounding nodes does have the advantage that we can very easily disconnect and reconnect nodes.
        //Additionally, it means nodes *can* easily be connected to nodes that they are actually not next to in terms of position,
        //which makes including things like teleporters in pathfinding much easier.
        public void DisconnectAll()
        {
            _surrounding = new Node[] {null, null, null, null, null, null, null, null};
        }
    }
}