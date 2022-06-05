using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.GUI
{
    public class Menu : Transformable, Drawable
    {
        public Vector2f Size { get; set; }
        public EventHandler<MouseMoveEventArgs> MoveHundler { get; }
        public EventHandler<MouseButtonEventArgs> ClickHandler { get; }
        protected List<Button> buttons;
        public Menu()
        {
            Size = new(500, 500);
            buttons = new List<Button>();
            MoveHundler = MouseMove;
            ClickHandler = MouseClick;
        }
        public virtual void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform *= Transform;
            foreach (var button in buttons)
            {
                target.Draw(button, states);
            }
        }
        private void MouseMove(object? sendeer, MouseMoveEventArgs args)
        {
            foreach (var button in buttons)
            {
                if (button.Bounds.Contains(args.X, args.Y))
                {
                    button.InFocus.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    button.OutFocus.Invoke(this, EventArgs.Empty);
                }
            }
        }
        private void MouseClick(object? sendeer, MouseButtonEventArgs args)
        {
            if (args.Button == Mouse.Button.Left)
            {
                foreach (var button in buttons)
                {
                    if (button.Bounds.Contains(args.X, args.Y))
                    {
                        button.Click.Invoke(this, EventArgs.Empty);
                    }
                }
            }
        }
    }
}
