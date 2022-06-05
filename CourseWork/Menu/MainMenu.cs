using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.GUI
{
    public class MainMenu : Menu
    {
        private Text topText = new Text("RED RIDING HOOD:", Content.Font, 20);
        private Text mainText = new Text("Way Home", Content.Font, 50);
        public Button PlayButton { get;}
        public Button LoadButton { get; }
        public Button ExitButton { get; }
        public MainMenu(Vector2f size)
        {
            topText.Origin = new(topText.GetLocalBounds().Width / 2, 0);
            mainText.Origin = new(mainText.GetLocalBounds().Width / 2, 0);
            topText.Position = new(size.X / 2, 50);
            mainText.Position = new(size.X / 2, 100);
            PlayButton = new Button("New game", new(200, 50)) { Origin = new(100, 25), Position = size / 2, FillColor = Color.Black, TextColor = Color.White, OutlineThickness = 5, OutlineColor = Color.White };
            LoadButton = new Button("Load game", new(200, 50)) { Origin = new(100, 25), Position = new(size.X / 2, size.Y / 2 + 70), FillColor = Color.Black, TextColor = Color.White, OutlineThickness = 5, OutlineColor = Color.White };
            ExitButton = new Button("Exit", new(200, 50)) { Origin = new(100, 25), Position = new(size.X / 2, size.Y / 2 + 140), FillColor = Color.Black, TextColor = Color.White, OutlineThickness = 5, OutlineColor = Color.White };
            buttons.Add(PlayButton);
            buttons.Add(LoadButton);
            buttons.Add(ExitButton);
        }
        public override void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform *= Transform;
            target.Draw(topText);
            target.Draw(mainText);
            foreach (var button in buttons)
            {
                target.Draw(button);
            }
        }
    }
}
