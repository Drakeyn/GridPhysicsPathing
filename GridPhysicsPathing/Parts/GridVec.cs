namespace PhysicsAStar.Primitives
{
    public class GridVec
    {
        public int x;
        public int y;

        public GridVec(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        
        public static bool operator ==(GridVec a, GridVec b)
            => a.Equals(b);

        public static bool operator !=(GridVec a, GridVec b)
            => !a.Equals(b);

        public static GridVec operator +(GridVec a, GridVec b)
        {
            return new GridVec(a.x + b.x, a.y + b.y);
        }
        
        public static GridVec operator -(GridVec a, GridVec b)
        {
            return new GridVec(a.x - b.x, a.y - b.y);
        }

        public override string ToString() => $"({this.x}, {this.y})";

        public override bool Equals(object obj) => obj is GridVec GridVec && this.Equals(GridVec);

        public bool Equals(GridVec other) => this.x == other.x && this.y == other.y;
        
    }
}