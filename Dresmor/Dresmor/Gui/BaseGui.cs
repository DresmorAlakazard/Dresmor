using Dresmor.System;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dresmor.Gui
{
    public class BaseGui : Node, Drawable
    {
        // Private Fields
        private Simple body = new Simple();
        private UDim2 size;
        private UDim2 position;
        private UDim2 origin;
        private float rotation;
        private Vector2f absoluteSize;
        private Vector2f absolutePosition;
        private Vector2f absoluteOrigin;
        private float absoluteRotation;
        private Transformable transformable = new Transformable();
        private bool visible = true;
        private bool collide = false;
        private List<TweenValue> tweenSize = new List<TweenValue>();
        private List<TweenValue> tweenOrigin = new List<TweenValue>();
        private List<TweenValue> tweenPosition = new List<TweenValue>();

        // Public Fields
        public Simple Body => body;
        public Vector2f AbsoluteSize => absoluteSize;
        public Vector2f AbsolutePosition => absolutePosition;
        public Vector2f AbsoluteOrigin => absoluteOrigin;
        public float AbsoluteRotation => absoluteRotation;
        public Transformable Transformable => transformable;
        public UDim2 Size
        {
            get => size; set
            {
                if (size == value) return;
                size = value;
                SizeChanged.Call(this, value);
                UpdateAbsoluteSize();
            }
        }
        public UDim2 Position
        {
            get => position; set
            {
                if (position == value) return;
                position = value;
                PositionChanged.Call(this, value);
                UpdateAbsolutePosition();
            }
        }
        public UDim2 Origin
        {
            get => origin;
            set {
                if (origin == value) return;
                origin = value;
                OriginChanged.Call(this, value);
                UpdateAbsoluteOrigin();
            }
        }
        public float Rotation
        {
            get => rotation;
            set {
                if (rotation == value) return;
                rotation = value;
                RotationChanged.Call(this, value);
                UpdateAbsoluteRotation();
            }
        }
        public bool Visible
        {
            get => visible;
            set
            {
                if (visible == value) return;
                visible = value;
                VisibleChanged.Call(this, value);
            }
        }
        public bool Collide
        {
            get => collide;
            set
            {
                if (collide == value) return;
                collide = value;
                if (collide) (Root as HudGui)?.HoverGroup.Add(this);
                else (Root as HudGui)?.HoverGroup.Remove(this);
                if (Root is HudGui) (Root as HudGui).RequireNextMouseHover = true;
                CollideChanged.Call(this, value);
            }
        }
        public override Node Parent {
            get => base.Parent;
            set
            {
                var oldParent = Parent;
                base.Parent = value;
                if (oldParent != Parent)
                {
                    UpdateAbsoluteSize();
                    UpdateAbsoluteRotation();
                }
                EnsureHoverGroupState(Root as HudGui, Root as HudGui);
            }
        }

        public Transform Transform => transformable.Transform;
        public Color FillColor { get => body.FillColor; set { if (body.FillColor.Equals(value)) return;  body.FillColor = value; FillColorChanged.Call(this, value); } }
        public Color OutlineColor { get => body.OutlineColor; set { if (body.OutlineColor.Equals(value)) return;  body.OutlineColor = value; OutlineColorChanged.Call(this, value); } }
        public float OutlineThickness { get => body.OutlineThickness; set { if (body.OutlineThickness.Equals(value)) return;  body.OutlineThickness = value; OutlineThicknessChanged.Call(this, value); } }
        public Texture Texture { get => body.Texture; set { if (body.Texture?.Equals(value) ?? false) return; body.Texture = value; TextureChanged.Call(this, value); } }
        public IntRect TextureRect { get => body.TextureRect; set { if (body.TextureRect.Equals(value)) return;  body.TextureRect = value; TextureRectChanged.Call(this, value); } }
        public ShapeTypes ShapeType { get => body.ShapeType; set { if (body.ShapeType.Equals(value)) return;  body.ShapeType = value; ShapeTypeChanged.Call(this, value); } }
        public float CornerRadius { get => body.CornerRadius; set { if (body.CornerRadius.Equals(value)) return; body.CornerRadius = value; CornerRadiusChanged.Call(this, value); } }
        public uint CornerPoints { get => body.CornerPoints; set { if (body.CornerPoints.Equals(value)) return; body.CornerPoints = value; CornerPointsChanged.Call(this, value); } }
        public List<Vector2f> Points => body.GetPoints();

        public List<TweenValue> TweenSize => tweenSize;
        public List<TweenValue> TweenOrigin => tweenOrigin;
        public List<TweenValue> TweenPosition => tweenPosition;

        // Public Events
        public DresmorHandler<float> CornerRadiusChanged = new DresmorHandler<float>();
        public DresmorHandler<uint> CornerPointsChanged = new DresmorHandler<uint>();
        public DresmorHandler<ShapeTypes> ShapeTypeChanged = new DresmorHandler<ShapeTypes>();

        public DresmorHandler<IntRect> TextureRectChanged = new DresmorHandler<IntRect>();
        public DresmorHandler<Texture> TextureChanged = new DresmorHandler<Texture>();

        public DresmorHandler<Color> FillColorChanged = new DresmorHandler<Color>();
        public DresmorHandler<Color> OutlineColorChanged = new DresmorHandler<Color>();
        public DresmorHandler<float> OutlineThicknessChanged = new DresmorHandler<float>();

        public DresmorHandler<bool> VisibleChanged = new DresmorHandler<bool>();
        public DresmorHandler<bool> CollideChanged = new DresmorHandler<bool>();

        public DresmorHandler<UDim2> SizeChanged = new DresmorHandler<UDim2>();
        public DresmorHandler<UDim2> OriginChanged = new DresmorHandler<UDim2>();
        public DresmorHandler<UDim2> PositionChanged = new DresmorHandler<UDim2>();
        public DresmorHandler<float> RotationChanged = new DresmorHandler<float>();

        public DresmorHandler<Vector2f> AbsoluteSizeChanged = new DresmorHandler<Vector2f>();
        public DresmorHandler<Vector2f> AbsoluteOriginChanged = new DresmorHandler<Vector2f>();
        public DresmorHandler<Vector2f> AbsolutePositionChanged = new DresmorHandler<Vector2f>();
        public DresmorHandler<float> AbsoluteRotationChanged = new DresmorHandler<float>();

        public DresmorHandler<MouseInput> MouseClick = new DresmorHandler<MouseInput>();
        public DresmorHandler<MouseInput> MouseDoubleClick = new DresmorHandler<MouseInput>();
        public DresmorHandler<MouseInput> MousePressed = new DresmorHandler<MouseInput>();
        public DresmorHandler<MouseInput> MouseReleased = new DresmorHandler<MouseInput>();
        public DresmorHandler<MouseInput> MouseEnter = new DresmorHandler<MouseInput>();
        public DresmorHandler<MouseInput> MouseLeave = new DresmorHandler<MouseInput>();
        public DresmorHandler<MouseInput> MouseMoved = new DresmorHandler<MouseInput>();

        // Private Methods
        private void EnsureHoverGroupState(HudGui newRoot, HudGui oldRoot)
        {
            if (oldRoot == newRoot)
            {
                if (newRoot != null)
                {
                    var index = newRoot.HoverGroup.IndexOf(this);
                    if (index > 0) newRoot.HoverGroup.RemoveAt(index);
                }
                else
                {
                    return;
                }
            }
            if (!collide) return;
            oldRoot?.HoverGroup.Remove(this);
            if (newRoot != null)
            {
                // LATER this method gonna change to faster with other algorithm...
                var index = 0;
                foreach (BaseGui other in newRoot.HoverGroup)
                {
                    if (ComparePath(other) < 0)
                    {
                        break;
                    }
                    ++index;
                }
                newRoot.HoverGroup.Insert(index, this);
            }
        }

        private void UpdateAbsoluteSize(bool reportSubs = true)
        {
            absoluteSize = size.Offset;
            if (Parent is BaseGui) absoluteSize += Vec2.Multiply((Parent as BaseGui).absoluteSize, size.Scale);
            body.Size = absoluteSize;
            AbsoluteSizeChanged.Call(this, absoluteSize);
            UpdateAbsoluteOrigin(false);
            if (reportSubs)
            {
                if (Root is HudGui) (Root as HudGui).RequireNextMouseHover = true;
                Layer((BaseGui gui) => { gui.UpdateAbsoluteSize(false); return 0; });
            }
        }
        private void UpdateAbsoluteOrigin(bool reportSubs = true)
        {
            absoluteOrigin = origin.Offset + Vec2.Multiply(absoluteSize, origin.Scale);
            AbsoluteOriginChanged.Call(this, absoluteOrigin);
            UpdateAbsolutePosition(false);
            if (reportSubs)
            {
                if (Root is HudGui) (Root as HudGui).RequireNextMouseHover = true;
                Layer((BaseGui gui) => { gui.UpdateAbsolutePosition(false); return 0; });
            }
        }
        private void UpdateAbsolutePosition(bool reportSubs = true)
        {
            absolutePosition = position.Offset;
            if (Parent is BaseGui)
            {
                absolutePosition += Vec2.Multiply((Parent as BaseGui).absoluteSize, position.Scale);
                absolutePosition = (Parent as BaseGui).Transform.TransformPoint(absolutePosition);
            }
            transformable.Origin = absoluteOrigin;
            transformable.Rotation = absoluteRotation;
            transformable.Position = absolutePosition;
            AbsolutePositionChanged.Call(this, absolutePosition);
            if (reportSubs)
            {
                if (Root is HudGui) (Root as HudGui).RequireNextMouseHover = true;
                Layer((BaseGui gui) => { gui.UpdateAbsolutePosition(false); return 0; });
            }
        }
        private void UpdateAbsoluteRotation(bool reportSubs = true)
        {
            absoluteRotation = rotation;
            if (Parent is BaseGui) absoluteRotation += (Parent as BaseGui).absoluteRotation;
            AbsoluteRotationChanged.Call(this, absoluteRotation);
            UpdateAbsolutePosition(false);
            if (reportSubs)
            {
                if (Root is HudGui) (Root as HudGui).RequireNextMouseHover = true;
                Layer((BaseGui gui) => { gui.UpdateAbsoluteRotation(false); return 0; });
            }
        }

        private void UpdateTweenPosition()
        {
            for (int i = 0; i < tweenPosition.Count; ++i)
            {
                var t = tweenPosition[i];
                if (t.Ready)
                {
                    Position = t.Position;
                    if (t.Done)
                    {
                        t?.Callback.Invoke(this, t);
                        tweenPosition.RemoveAt(i--);
                    }
                }
                else
                {
                    t.Setup(position);
                    break;
                }
            }
        }

        private void UpdateTweenSize()
        {
            for (int i = 0; i < tweenSize.Count; ++i)
            {
                var t = tweenSize[i];
                if (t.Ready)
                {
                    Size = t.Position;
                    if (t.Done)
                    {
                        t?.Callback.Invoke(this, t);
                        tweenSize.RemoveAt(i--);
                    }
                }
                else
                {
                    t.Setup(size);
                    break;
                }
            }
        }

        private void UpdateTweenOrigin()
        {
            for (int i = 0; i < tweenOrigin.Count; ++i)
            {
                var t = tweenOrigin[i];
                if (t.Ready)
                {
                    Origin = t.Position;
                    if (t.Done)
                    {
                        t?.Callback.Invoke(this, t);
                        tweenOrigin.RemoveAt(i--);
                    }
                }
                else
                {
                    t.Setup(origin);
                    break;
                }
            }
        }

        // Public Methods
        public virtual bool IsPointInside(Vector2f point)
        {
            List<Vector2f> points = Points;
            for (int z = 0; z < points.Count; ++z)
            {
                points[z] = Transform.TransformPoint(points[z]);
            }
            int i, j, nvert = points.Count();
            bool c = false;
            for (i = 0, j = nvert - 1; i < nvert; j = i++)
            {
                if (((points[i].Y >= point.Y) != (points[j].Y >= point.Y)) && (point.X <= (points[j].X - points[i].X) * (point.Y - points[i].Y) / (points[j].Y - points[i].Y) + points[i].X))
                {
                    c = !c;
                }
            }
            return c;
        }

        public void AddTweenPosition(UDim2 to, float seconds = 1.0f, bool overridable = false, Action<BaseGui, TweenValue> callback = null)
        {
            if (overridable) for (int i = 0; i < tweenPosition.Count; ++i) if (tweenPosition[i].Overridable) tweenPosition.RemoveAt(i--);
            if (tweenPosition.Count > 0)
                tweenPosition.Add(new TweenValue(to, seconds, overridable, callback));
            else
                tweenPosition.Add(new TweenValue(to, seconds, overridable, callback).Setup(position));
        }

        public void AddTweenSize(UDim2 to, float seconds = 1.0f, bool overridable = false, Action<BaseGui, TweenValue> callback = null)
        {
            if (overridable) for (int i = 0; i < tweenSize.Count; ++i) if (tweenSize[i].Overridable) tweenSize.RemoveAt(i--);
            if (tweenSize.Count > 0)
                tweenSize.Add(new TweenValue(to, seconds, overridable, callback));
            else
                tweenSize.Add(new TweenValue(to, seconds, overridable, callback).Setup(size));
        }

        public void AddTweenOrigin(UDim2 to, float seconds = 1.0f, bool overridable = false, Action<BaseGui, TweenValue> callback = null)
        {
            if (overridable) for (int i = 0; i < tweenOrigin.Count; ++i) if (tweenOrigin[i].Overridable) tweenOrigin.RemoveAt(i--);
            if (tweenOrigin.Count > 0)
                tweenOrigin.Add(new TweenValue(to, seconds, overridable, callback));
            else
                tweenOrigin.Add(new TweenValue(to, seconds, overridable, callback).Setup(origin));
        }

        public virtual void Draw(RenderTarget target, RenderStates states)
        {
            UpdateTweenOrigin();
            UpdateTweenPosition();
            UpdateTweenSize();
            states.Transform *= Transform;
            target.Draw(body, states);
        }

        // Constructor
        public BaseGui()
        {
            RootChanged += (s, e) => EnsureHoverGroupState(e[0] as HudGui, e[1] as HudGui);
        }
    }
}
