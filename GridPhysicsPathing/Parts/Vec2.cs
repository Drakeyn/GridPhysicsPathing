namespace PhysicsAStar.Primitives
{
    public class Vec2
    {
        public float x;
        public float y;

        public Vec2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
        
        public static bool operator ==(Vec2 a, Vec2 b)
            => a.Equals(b);

        public static bool operator !=(Vec2 a, Vec2 b)
            => !a.Equals(b);

        public static Vec2 operator +(Vec2 a, Vec2 b)
        {
            return new Vec2(a.x + b.x, a.y + b.y);
        }
        
        public static Vec2 operator -(Vec2 a, Vec2 b)
        {
            return new Vec2(a.x - b.x, a.y - b.y);
        }

        public override string ToString() => $"({this.x}, {this.y})";

        public override bool Equals(object obj) => obj is Vec2 Vec2 && this.Equals(Vec2);

        public bool Equals(Vec2 other) => this.x == other.x && this.y == other.y;
        
    }
}