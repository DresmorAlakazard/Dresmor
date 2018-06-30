using SFML.System;
using System.Collections.Generic;

namespace Dresmor.System
{
    public struct UDim2
    {
        public UDim X;
        public UDim Y;

        public Vector2f Scale => new Vector2f(X.Scale, Y.Scale);
        public Vector2f Offset => new Vector2f(X.Offset, Y.Offset);

        public UDim2(float xScale, float xOffset, float yScale, float yOffset)
        {
            X.Scale = xScale;
            X.Offset = xOffset;
            Y.Scale = yScale;
            Y.Offset = yOffset;
        }

        public UDim2(UDim2 copy)
        {
            X.Scale = copy.X.Scale;
            X.Offset = copy.X.Offset;
            Y.Scale = copy.Y.Scale;
            Y.Offset = copy.Y.Offset;
        }

        public UDim2(UDim X, UDim Y)
        {
            this.X.Scale = X.Scale;
            this.X.Offset = X.Offset;
            this.Y.Scale = Y.Scale;
            this.Y.Offset = Y.Offset;
        }

        public UDim2(float xOffset, float yOffset)
        {
            this.X.Scale = 0.0f;
            this.X.Offset = xOffset;
            this.Y.Scale = 0.0f;
            this.Y.Offset = yOffset;
        }

        public static bool operator ==(UDim2 left, UDim2 right) => left.X == right.X && left.Y == right.Y;
        public static bool operator !=(UDim2 left, UDim2 right) => left.X != right.X || left.Y != right.Y;

        public static UDim2 operator +(UDim2 left, UDim2 right) => new UDim2(left.X + right.X, left.Y + right.Y);
        public static UDim2 operator -(UDim2 left, UDim2 right) => new UDim2(left.X - right.X, left.Y - right.Y);
        public static UDim2 operator *(UDim2 left, UDim2 right) => new UDim2(left.X * right.X, left.Y * right.Y);
        public static UDim2 operator /(UDim2 left, UDim2 right) => new UDim2(left.X / right.X, left.Y / right.Y);

        public static UDim2 operator +(UDim2 left, UDim right) => new UDim2(left.X + right, left.Y + right);
        public static UDim2 operator -(UDim2 left, UDim right) => new UDim2(left.X - right, left.Y - right);
        public static UDim2 operator *(UDim2 left, UDim right) => new UDim2(left.X * right, left.Y * right);
        public static UDim2 operator /(UDim2 left, UDim right) => new UDim2(left.X / right, left.Y / right);

        public static UDim2 operator +(UDim2 left, float right) => new UDim2(left.X + right, left.Y + right);
        public static UDim2 operator -(UDim2 left, float right) => new UDim2(left.X - right, left.Y - right);
        public static UDim2 operator *(UDim2 left, float right) => new UDim2(left.X * right, left.Y * right);
        public static UDim2 operator /(UDim2 left, float right) => new UDim2(left.X / right, left.Y / right);

        public override bool Equals(object obj)
        {
            if (!(obj is UDim2))
            {
                return false;
            }

            var dim = (UDim2)obj;
            return EqualityComparer<UDim>.Default.Equals(X, dim.X) &&
                   EqualityComparer<UDim>.Default.Equals(Y, dim.Y) &&
                   Scale.Equals(dim.Scale) &&
                   Offset.Equals(dim.Offset);
        }

        public override int GetHashCode()
        {
            var hashCode = 1899034662;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<UDim>.Default.GetHashCode(X);
            hashCode = hashCode * -1521134295 + EqualityComparer<UDim>.Default.GetHashCode(Y);
            hashCode = hashCode * -1521134295 + EqualityComparer<Vector2f>.Default.GetHashCode(Scale);
            hashCode = hashCode * -1521134295 + EqualityComparer<Vector2f>.Default.GetHashCode(Offset);
            return hashCode;
        }
    }
}
