using SFML.System;

namespace Dresmor.System
{
    public class Vec2
    {
        public static Vector2f Divide(Vector2f left, Vector2f right) => new Vector2f(left.X / right.X, left.Y / right.Y);
        public static Vector2f Multiply(Vector2f left, Vector2f right) => new Vector2f(left.X * right.X, left.Y * right.Y);
    }
}
