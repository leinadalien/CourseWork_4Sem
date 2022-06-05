using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.GUI
{
    public class Button : Transformable, Drawable
    {
        private RectangleShape shape;
        private Color fillColor;
        private Text buttonText;
        public IntRect Bounds { get { return new((Vector2i)(Position - Origin), (Vector2i)shape.Size); } }
        public EventHandler Click { get; set; }
        public EventHandler InFocus { get; }
        public EventHandler OutFocus { get; }
        public Color FillColor { get { return fillColor; } set { fillColor = value; shape.FillColor = fillColor; } }
        public Color TextColor { get { return buttonText.FillColor; } set { buttonText.FillColor = value; } }
        public Color OutlineColor { get { return shape.OutlineColor; } set { shape.OutlineColor = value; } }
        public float OutlineThickness { get { return shape.OutlineThickness; } set { shape.OutlineThickness = value; } }

        public Button(string text, Vector2f size)
        {
            shape = new(size);
            buttonText = new(text, Content.Font);
            buttonText.Origin = new(buttonText.GetLocalBounds().Width / 2, buttonText.GetLocalBounds().Height / 2);
            buttonText.Position = new(size.X / 2, size.Y / 2);
            InFocus = (s, e) => shape.FillColor = new(64, 64, 64);
            OutFocus = (s, e) => shape.FillColor = fillColor;
            Click = (s, e) => { };
        }
        public void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform *= Transform;
            target.Draw(shape, states);
            target.Draw(buttonText, states);
        }
    }
}
