using Dresmor.System;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dresmor.Gui
{
    public class SliderGui: ButtonGui
    {
        // Private Fields
        private ButtonGui slider = new ButtonGui();
        private float sliderValue = 0.0f;
        private UDim sliderSize = new UDim(0.1f, 10.0f);

        // Public Fields
        public ButtonGui Slider => slider;
        public bool IsHorizontal => AbsoluteSize.X > AbsoluteSize.Y;
        public UDim SliderSize { get => sliderSize; set { sliderSize = value; EnsureSliderGeometry(); } }
        public float SliderValue { get => sliderValue; set { if (sliderValue == value) return; sliderValue = value; EnsureSliderGeometry(); SliderValueChanged.Call(this, value); } }

        public DresmorHandler<float> SliderValueChanged = new DresmorHandler<float>();

        // Private Methods
        private void EnsureSliderGeometry()
        {
            slider.Size = IsHorizontal ? new UDim2(sliderSize.Scale, sliderSize.Offset, 1.0f, 0.0f) : new UDim2(1.0f, 0.0f, sliderSize.Scale, sliderSize.Offset);
            slider.Position = IsHorizontal ? new UDim2(sliderValue, -sliderValue * slider.AbsoluteSize.X, 0.0f, 0.0f) : new UDim2(0.0f, 0.0f, sliderValue, -sliderValue * slider.AbsoluteSize.Y);
        }

        // Constructor
        public SliderGui()
        {
            AutoButtonColor = false;

            FillColor = new Color(210, 210, 210);
            Size = new UDim2(0, 120, 0, 20);
            slider.Parent = this;
            EnsureSliderGeometry();
            AbsoluteSizeChanged += (s, e) => EnsureSliderGeometry();
            
            float dragDistance = 0.0f;

            EventHandler<MouseInput> onMousePressed = (s, e) =>
            {
                if (IsHorizontal)
                {
                    Vector2f A = slider.Transform.TransformPoint(0, 0);
                    Vector2f B = slider.Transform.TransformPoint(0, slider.AbsoluteSize.Y);
                    Vector2f P = e.Position;
                    Vector2f dif = B - A;
                    dragDistance = ((P.X - A.X) * (B.Y - A.Y) - (P.Y - A.Y) * (B.X - A.X)) / Math.Sqrt(dif.X * dif.X + dif.Y * dif.Y).ToFloat();
                }
                else
                {
                    Vector2f A = slider.Transform.TransformPoint(0, 0);
                    Vector2f B = slider.Transform.TransformPoint(slider.AbsoluteSize.X, 0);
                    Vector2f P = e.Position;
                    Vector2f dif = B - A;
                    dragDistance = -((P.X - A.X) * (B.Y - A.Y) - (P.Y - A.Y) * (B.X - A.X)) / Math.Sqrt(dif.X * dif.X + dif.Y * dif.Y).ToFloat();
                }
            };

            slider.MousePressed.Add(onMousePressed);
            MousePressed.Add(onMousePressed);

            EventHandler<MouseInput> onMouseMoved = (s, e) => {
                if (!Mouse.IsButtonPressed(Mouse.Button.Left)) return;
                if (IsHorizontal)
                {
                    Vector2f A = Transform.TransformPoint(0, 0);
                    Vector2f B = Transform.TransformPoint(0, AbsoluteSize.Y);
                    Vector2f P = e.Position;
                    Vector2f dif = B - A;
                    SliderValue = Math.Max(0, Math.Min(AbsoluteSize.X - slider.AbsoluteSize.X, ((P.X - A.X) * (B.Y - A.Y) - (P.Y - A.Y) * (B.X - A.X)) / Math.Sqrt(dif.X * dif.X + dif.Y * dif.Y).ToFloat() - dragDistance)) / (AbsoluteSize.X - slider.AbsoluteSize.X);
                }
                else
                {
                    Vector2f A = Transform.TransformPoint(0, 0);
                    Vector2f B = Transform.TransformPoint(AbsoluteSize.X, 0);
                    Vector2f P = e.Position;
                    Vector2f dif = B - A;
                    SliderValue = Math.Max(0, Math.Min(AbsoluteSize.Y - slider.AbsoluteSize.Y, -((P.X - A.X) * (B.Y - A.Y) - (P.Y - A.Y) * (B.X - A.X)) / Math.Sqrt(dif.X * dif.X + dif.Y * dif.Y).ToFloat() - dragDistance)) / (AbsoluteSize.Y - slider.AbsoluteSize.Y);
                }
            };

            slider.MouseMoved.Add(onMouseMoved);
            MouseMoved.Add(onMouseMoved);
        }
    }
}
