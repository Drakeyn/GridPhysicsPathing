using PhysicsAStar.Parts;
using PhysicsAStar.Pathing;

namespace PhysicsAStar.Physics
{
    public interface ICheckNodes
    {
        bool NodeValid(Node node, Node from, Node lastSafe);
        bool NodeSafe(Node node);
    }
}