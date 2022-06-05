using System;
using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;
using SFML.Window;
using CourseWork.GUI;
using SFML.System;

namespace CourseWork
{
    internal class Program
    {
        private delegate void Updater();
        private static RenderWindow window;
        public static RenderWindow Window { get { return window; } }
        public static Game Game { private set; get; }
        public static void Main(string[] args)
        {
            Updater updater;
            ContextSettings settings = new();
            settings.AntialiasingLevel = 8;
            window = new(new(1280, 720), "Title", Styles.Close, settings);
            MainMenu mainMenu = new((Vector2f)window.Size);
            PauseMenu pauseMenu = new((Vector2f)window.Size);
            window.MouseMoved += mainMenu.MoveHundler;
            window.MouseButtonReleased += mainMenu.ClickHandler;
            updater = () =>
            {
                window.Clear(Color.Black);
                window.Draw(mainMenu);
            };
            pauseMenu.ContinueButton.Click = (s, e) =>
            {
                updater = () =>
                {
                    Game.Update();
                    window.Clear(Color.Black);
                    Game.Draw();
                };
                Game.Pause = false;
                window.MouseMoved -= pauseMenu.MoveHundler;
                window.MouseButtonReleased -= pauseMenu.ClickHandler;
                window.MouseButtonReleased += Game.MouseClick;
                window.MouseMoved += Game.MouseMove;
            };
            pauseMenu.ExitToMainMenuButton.Click = (s, e) => {
                
                updater = () =>
                {
                    
                    window.Clear(Color.Black);
                    window.Draw(mainMenu);
                };
                window.MouseButtonReleased -= pauseMenu.ClickHandler;
                window.MouseMoved -= pauseMenu.MoveHundler;
                window.MouseMoved += mainMenu.MoveHundler;
                window.MouseButtonReleased += mainMenu.ClickHandler;
            };
            mainMenu.PlayButton.Click += (s, e) =>
            {
                Random random = new Random();
                Game = new(random.Next());
                window.MouseButtonReleased -= mainMenu.ClickHandler;
                window.MouseMoved -= mainMenu.MoveHundler;
                window.KeyPressed += Game.KeyPressed;
                window.KeyReleased += Game.KeyReleased;
                Game.PauseHundler += (s, e) =>
                {
                    updater = () =>
                    {
                        window.Clear(Color.Black);
                        Game.Draw();
                        window.Draw(pauseMenu);
                        window.MouseMoved += pauseMenu.MoveHundler;
                        window.MouseButtonReleased += pauseMenu.ClickHandler;
                        window.MouseButtonReleased -= Game.MouseClick;
                        window.MouseMoved -= Game.MouseMove;
                    };
                };
                updater = () =>
                {
                    Game.Update();
                    window.Clear(Color.Black);
                    Game.Draw();
                };
            };
            mainMenu.ExitButton.Click += (s, e) => window.Close();
            window.Closed += (s, e) => window.Close();
            while (window.IsOpen)
            {
                window.DispatchEvents();
                updater();
                window.Display();
            }
        }
    }
}