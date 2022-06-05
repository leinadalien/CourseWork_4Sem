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
        private float buttonOffset = 0;
        private RectangleShape background;
        private Vector2f size;
        private Text topText = new("Menu", Content.Font, 50);
        public EventHandler<MouseMoveEventArgs> MoveHundler { get; }
        public EventHandler<MouseButtonEventArgs> ClickHandler { get; }
        private List<Button> buttons = new();
        public Menu(Vector2f size, string text)
        {
            topText = new(text, Content.Font, 50);
            this.size = size;
            topText.Origin = new(topText.GetLocalBounds().Width / 2, 0);
            topText.Position = new(size.X / 2, 250);
            background = new(size);
            background.FillColor = new(0, 0, 0, 127);
            MoveHundler = MouseMove;
            ClickHandler = MouseClick;
        }
        public void AddButton(Button button)
        {
            buttons.Add(button);
            button.Origin = new(button.Bounds.Width / 2, button.Bounds.Height / 2);
            button.Position = new(size.X / 2, size.Y / 2 + buttonOffset);
            buttonOffset += button.Bounds.Height + 20;
        }
        public virtual void Draw(RenderTarget target, RenderStates states)
        {
            background.Draw(target, states);
            topText.Draw(target, states);
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
