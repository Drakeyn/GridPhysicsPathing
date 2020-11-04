using System.Collections.Generic;
using PhysicsAStar.Parts;

namespace PhysicsAStar.Pathing
{
    public class Path
    {
        
        public List<Node> Nodes { get; }

        public bool Complete { get; }

        public Path(List<Node> nodes, bool complete)
        {
            Nodes = nodes;
            Complete = complete;
        }
        
    }
}