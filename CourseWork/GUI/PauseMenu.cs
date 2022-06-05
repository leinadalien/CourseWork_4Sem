using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.GUI
{
    public class PauseMenu : Menu
    {
        private RectangleShape shape;
        private Text text = new Text("Pause", Content.Font, 50);
        public Button ContinueButton { get; }
        public Button ExitToMainMenuButton { get; }
        public PauseMenu(Vector2f size)
        {
            shape = new RectangleShape(size);
            shape.FillColor = new(0, 0, 0, 127);
            text.Origin = new(text.GetLocalBounds().Width / 2, 0);
            text.Position = new(size.X / 2, 50);
            ContinueButton = new Button("Continue", new(200, 50)) { Origin = new(100, 25), Position = size / 2, FillColor = Color.Black, TextColor = Color.White, OutlineThickness = 5, OutlineColor = Color.White };
            ExitToMainMenuButton = new Button("Exit", new(200, 50)) { Origin = new(100, 25), Position = new(size.X / 2, size.Y / 2 + 70), FillColor = Color.Black, TextColor = Color.White, OutlineThickness = 5, OutlineColor = Color.White };
            buttons.Add(ContinueButton);
            buttons.Add(ExitToMainMenuButton);
        }
        public override void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform *= Transform;
            target.Draw(shape, states);
            target.Draw(text);
            foreach (var button in buttons)
            {
                target.Draw(button);
            }
        }
    }
}
