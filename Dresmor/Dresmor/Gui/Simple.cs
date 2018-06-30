using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;

namespace Dresmor.Gui
{
    public enum ShapeTypes
    {
        Rectangle,
        Rounded,
        Ellipse,
        Circle
    }

    public class Simple: Shape
    {
        // Private Fields
        private ShapeTypes shapeType = ShapeTypes.Rounded;
        private Vector2f size = new Vector2f(100.0f, 32.0f);
        private float cornerRadius = 10.0f;
        private uint cornerPoints = 5;

        // Public Fields
        public ShapeTypes ShapeType { get => shapeType; set { shapeType = value; Update();  } }
        public Vector2f Size { get => size; set { size = value; Update(); } }
        public float CornerRadius { get => cornerRadius; set { cornerRadius = value; Update(); } }
        public uint CornerPoints { get => cornerPoints; set { cornerPoints = value; Update(); } }
        public Vector2f StrictSize
        {
            get
            {
                switch (shapeType)
                {
                    case ShapeTypes.Circle: return new Vector2f(Math.Min(size.X, size.Y), Math.Min(size.X, size.Y));
                    default: return size;
                }
            }
        }
        public float StrictCornerRadius
        {
            get
            {
                switch (shapeType)
                {
                    case ShapeTypes.Ellipse:
                    case ShapeTypes.Circle: return Math.Min(size.X, size.Y) / 2.0f;
                    case ShapeTypes.Rectangle: return 0.0f;
                    default: return cornerRadius;
                }
            }
        }
        public float StrictCornerRadiusX
        {
            get
            {
                switch (shapeType)
                {
                    case ShapeTypes.Ellipse: return size.X / 2.0f;
                    case ShapeTypes.Circle: return Math.Min(size.X, size.Y) / 2.0f;
                    case ShapeTypes.Rectangle: return 0.0f;
                    default: return cornerRadius;
                }
            }
        }
        public float StrictCornerRadiusY
        {
            get
            {
                switch (shapeType)
                {
                    case ShapeTypes.Ellipse: return size.Y / 2.0f;
                    case ShapeTypes.Circle: return Math.Min(size.X, size.Y) / 2.0f;
                    case ShapeTypes.Rectangle: return 0.0f;
                    default: return cornerRadius;
                }
            }
        }
        public uint StrictCornerPoints
        {
            get
            {
                switch (shapeType)
                {
                    case ShapeTypes.Ellipse:
                    case ShapeTypes.Circle: return Math.Max(8, cornerPoints);
                    case ShapeTypes.Rectangle: return 1;
                    default: return Math.Max(1, cornerPoints);
                }
            }
        }

        // Private Methods

        // Public Methods
        public List<Vector2f> GetPoints()
        {
            List<Vector2f> result = new List<Vector2f>((int)GetPointCount());
            for (uint i = 0, c = GetPointCount(); i < c; ++i) result.Add(GetPoint(i));
            return result;
        }

        public override uint GetPointCount()
        {
            return StrictCornerPoints * 4;
        }

        public override Vector2f GetPoint(uint index)
        {
            if (index >= StrictCornerPoints * 4) return default(Vector2f);
            Vector2f center = default(Vector2f);
            uint centerIndex = index / StrictCornerPoints;
            switch(centerIndex)
            {
                case 0: center.X = StrictSize.X - StrictCornerRadiusX; center.Y = StrictCornerRadiusY; break;
                case 1: center.X = StrictCornerRadiusX; center.Y = StrictCornerRadiusY; break;
                case 2: center.X = StrictCornerRadiusX; center.Y = StrictSize.Y - StrictCornerRadiusY; break;
                case 3: center.X = StrictSize.X - StrictCornerRadiusX; center.Y = StrictSize.Y - StrictCornerRadiusY; break;
            }
            double r = ((90.0f / Math.Max(1, StrictCornerPoints - 1)) * (index - centerIndex) * Math.PI / 180.0f);
            return new Vector2f(
                StrictCornerRadiusX * Math.Cos(r).ToFloat() + center.X,
                -StrictCornerRadiusY * Math.Sin(r).ToFloat() + center.Y
            );
        }

        // Public Constructors
        public Simple()
        {
            Update();
        }
    }
}


public static class MathExtensions
{
    public static float ToFloat(this double value)
    {
        return (float)value;
    }
}