using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dresmor.Gui
{
    public class ButtonGui : LabelGui
    {
        // Private Fields
        private int buttonStage = 0; // 0 nothing, 1 mouse hover, 2 mouse down

        // Public Fields
        public bool AutoButtonColor = true;

        // Public Methods
        public override void Draw(RenderTarget target, RenderStates states)
        {
            if (!AutoButtonColor)
            {
                base.Draw(target, states);
                return;
            }
            Color old = Body.FillColor;
            Color col = Body.FillColor;
            switch (buttonStage)
            {
                case 1:
                    col = new Color(
                    (byte)Math.Min(255, col.R + 40),
                    (byte)Math.Min(255, col.G + 40),
                    (byte)Math.Min(255, col.B + 40)
                ); break;
                case 2:
                    col = new Color(
                    (byte)Math.Max(0, col.R - 40),
                    (byte)Math.Max(0, col.G - 40),
                    (byte)Math.Max(0, col.B - 40)
                ); break;
            }
            if (col != old) Body.FillColor = col;
            base.Draw(target, states);
            if (col != old) Body.FillColor = old;
        }

        // Constructor
        public ButtonGui() {
            ShapeType = ShapeTypes.Rounded;
            CornerRadius = 5.0f;
            FillColor = new Color(150, 150, 150);
            Collide = true;

            MouseEnter += (s, e) => buttonStage = Mouse.IsButtonPressed(Mouse.Button.Left) ? 2 : 1;
            MousePressed += (s, e) => buttonStage = Mouse.IsButtonPressed(Mouse.Button.Left) ? 2 : 1;
            MouseReleased += (s, e) => buttonStage = Mouse.IsButtonPressed(Mouse.Button.Left) ? 2 : 1;
            MouseLeave += (s, e) => buttonStage = 0;
        }
    }
}
