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
    public class LabelGui: BaseGui
    {
        // Private Fields
        private string textString;
        private Vector2f textAlignemnt = new Vector2f(0.5f, 0.5f);
        private List<Text> textLineShapes = new List<Text>();
        private Font textFont;
        private uint textSize = 20;
        private Color textColor = Color.Black;
        private FloatRect textBounds;
        private bool requireTextUpdate = true;
        private bool requireTextReposition = false;

        // Public Fields
        public string TextString { get => textString; set { textString = value; requireTextUpdate = true;  } }
        public Vector2f TextAlignemnt { get => textAlignemnt; set { textAlignemnt = value; requireTextReposition = true; } }
        public List<Text> TextLineShapes { get { UpdateText(); return textLineShapes; } }
        public Font TextFont { get => textFont; set { textFont = value; requireTextUpdate = true; } }
        public uint TextSize { get => textSize; set { textSize = value; requireTextUpdate = true; } }
        public Color TextColor { get => textColor; set { textColor = value; foreach (Text text in TextLineShapes) text.Color = textColor; } }
        public bool RequireTextUpdate { get => requireTextUpdate; set => requireTextUpdate = value; }
        public FloatRect TextBounds => textBounds;

        // Private Methods
        private void UpdateText()
        {
            if (!requireTextUpdate && !requireTextReposition) return;

            textBounds = default(FloatRect);

            if (!requireTextReposition || requireTextUpdate)
            {
                requireTextUpdate = false;
                textLineShapes.Clear();
                if (textFont == null || textSize == 0 || textString.Length == 0) return;

                for (int i = 0, j = 0; i <= textString.Length; ++i)
                {
                    if (i == textString.Length || textString[i] == '\n')
                    {
                        textLineShapes.Add(new Text(textString.Substring(j, i - j), textFont, textSize));
                        j = i + 1;
                    }
                }
            } else {
                requireTextReposition = false;
            }

            if (textLineShapes.Count == 0) return;

            float lineSpacing = textLineShapes.First().GetLocalBounds().Height;
            float y = (Body.StrictSize.Y - textLineShapes.Count * lineSpacing) * textAlignemnt.Y
                    - (lineSpacing / 2.0f) * textAlignemnt.Y
                ;
            textBounds.Top = y;
            textBounds.Height = textLineShapes.Count * lineSpacing;
            foreach (Text text in textLineShapes)
            {
                float x = (Body.StrictSize.X - text.GetLocalBounds().Width) * textAlignemnt.X;
                textBounds.Left = Math.Min(textBounds.Left, x);
                textBounds.Width = Math.Max(textBounds.Width, text.GetLocalBounds().Width);
                text.Position = new Vector2f(x, y);
                text.Color = textColor;
                y += lineSpacing;
            }
        }

        // Public Methods
        public override void Draw(RenderTarget target, RenderStates states)
        {
            base.Draw(target, states);
            states.Transform *= Transform;
            foreach (Text text in TextLineShapes)
            {
                target.Draw(text, states);
            }
        }

        // Constructor
        public LabelGui()
        {
            ShapeType = ShapeTypes.Rectangle;
            Size = new UDim2(0, 120, 0, 32);
            ParentChanged += (s, e) => requireTextReposition = true;
            AbsoluteSizeChanged += (s, e) => requireTextReposition = true;
        }
    }
}
