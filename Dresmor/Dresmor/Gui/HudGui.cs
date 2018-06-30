using Dresmor.System;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dresmor.Gui
{
    public class HudGui : BaseGui
    {
        // Private Fields
        private List<BaseGui> hoverGroup = new List<BaseGui>();
        private BaseGui lastMouseHover = null;
        private MouseInput lastMouseInput = new MouseInput(new Vector2f(-1, -1));
        private bool requireNextMouseHover = false;
        private Clock lastMouseClickAction = new Clock();

        // Public Fields
        public List<BaseGui> HoverGroup => hoverGroup;
        public BaseGui LastMouseHover => lastMouseHover;
        public MouseInput LastMouseInput => lastMouseInput;
        public bool RequireNextMouseHover { get => requireNextMouseHover; set => requireNextMouseHover = value; }

        // Private Methods
        private BaseGui GetMouseHover(Vector2f point)
        {
            BaseGui result = null;
            BaseGui skip = null;
            foreach (BaseGui gui in hoverGroup)
            {
                if (skip != null && skip.IsAncestorOf(gui)) continue;
                else skip = null;
                if (gui.Visible)
                {
                    if (gui.IsPointInside(point)) result = gui;
                }
                else skip = gui;
            }
            return result;
        }

        // Public Methods
        public void ApplyMouseMovement(MouseInput nextMouseInput)
        {
            if (!IsPointInside(nextMouseInput.Position)) return;
            BaseGui nextMouseHover = GetMouseHover(nextMouseInput.Position);
            if (lastMouseHover != nextMouseHover)
            {
                lastMouseHover?.MouseLeave.Call(lastMouseHover, nextMouseInput);
                nextMouseHover?.MouseEnter.Call(nextMouseHover, nextMouseInput);
            }
            else if (nextMouseInput.Position != lastMouseInput.Position)
            {
                lastMouseHover?.MouseMoved.Call(lastMouseHover, nextMouseInput);
            }
            lastMouseInput.Position = nextMouseInput.Position;
            lastMouseHover = nextMouseHover;
        }

        public void ApplyMouseAction(MouseInput nextMouseInput)
        {
            if (!IsPointInside(nextMouseInput.Position)) return;
            if (!nextMouseInput.Pressed)
            {
                lastMouseHover?.MouseClick.Call(LastMouseHover, nextMouseInput);
                if (lastMouseClickAction.ElapsedTime.AsMilliseconds() <= 250)
                    lastMouseHover?.MouseDoubleClick.Call(LastMouseHover, nextMouseInput);
                else lastMouseClickAction.Restart();
            }
            if (nextMouseInput.Pressed) lastMouseHover?.MousePressed.Call(lastMouseHover, nextMouseInput);
            else lastMouseHover?.MouseReleased.Call(lastMouseHover, nextMouseInput);
            lastMouseInput = nextMouseInput;
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            if (requireNextMouseHover)
            {
                requireNextMouseHover = false;
                ApplyMouseMovement(lastMouseInput);
            }
            Layer((BaseGui gui) => {
                if (gui.Visible == false) return 1;
                target.Draw(gui, states);
                return 0;
            });
        }

        // Constructor
        public HudGui()
        {
            Roots = true;
        }
    }
}
