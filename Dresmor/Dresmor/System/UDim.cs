namespace Dresmor.System
{
    public struct UDim
    {
        public float Scale;
        public float Offset;

        public UDim(float scale, float offset)
        {
            Scale = scale;
            Offset = offset;
        }

        public UDim(UDim copy)
        {
            Scale = copy.Scale;
            Offset = copy.Offset;
        }


        public static bool operator ==(UDim left, UDim right) => left.Offset == right.Offset && left.Scale == right.Scale;
        public static bool operator !=(UDim left, UDim right) => left.Offset != right.Offset || left.Scale != right.Scale;

        public static UDim operator +(UDim left, UDim right) => new UDim(left.Scale + right.Scale, left.Offset + right.Offset);
        public static UDim operator -(UDim left, UDim right) => new UDim(left.Scale - right.Scale, left.Offset - right.Offset);
        public static UDim operator /(UDim left, UDim right) => new UDim(left.Scale / right.Scale, left.Offset / right.Offset);
        public static UDim operator *(UDim left, UDim right) => new UDim(left.Scale * right.Scale, left.Offset * right.Offset);

        public static UDim operator +(UDim left, float right) => new UDim(left.Scale + right, left.Offset + right);
        public static UDim operator -(UDim left, float right) => new UDim(left.Scale - right, left.Offset - right);
        public static UDim operator /(UDim left, float right) => new UDim(left.Scale / right, left.Offset / right);
        public static UDim operator *(UDim left, float right) => new UDim(left.Scale * right, left.Offset * right);

        public override bool Equals(object obj)
        {
            if (!(obj is UDim))
            {
                return false;
            }

            var dim = (UDim)obj;
            return Scale == dim.Scale &&
                   Offset == dim.Offset;
        }

        public override int GetHashCode()
        {
            var hashCode = -2002834013;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + Scale.GetHashCode();
            hashCode = hashCode * -1521134295 + Offset.GetHashCode();
            return hashCode;
        }
    }
}
