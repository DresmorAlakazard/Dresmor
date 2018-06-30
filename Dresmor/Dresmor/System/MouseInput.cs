using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dresmor.System
{
    public class MouseInput: Input
    {
        public Vector2f Position;
        public Mouse.Button Button;
        public bool Pressed;

        public MouseInput(Vector2f position)
        {
            Position = position;
        }

        public MouseInput(Vector2f position, Mouse.Button button, bool pressed)
        {
            Position = position;
            Button = button;
            Pressed = pressed;
        }

        public MouseInput(MouseInput copy)
        {
            Position = copy.Position;
            Button = copy.Button;
            Pressed = copy.Pressed;
        }

        public override Types Type => Types.Mouse;
    }
}
