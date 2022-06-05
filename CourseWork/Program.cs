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
        private static MainMenu mainMenu;
        private static PauseMenu pauseMenu;
        private static Updater updater;
        private static void NewGame()
        {
            DrawLoading();
            Random random = new();
            Game = new(random.Next());
            window.MouseButtonReleased -= mainMenu.ClickHandler;
            window.MouseMoved -= mainMenu.MoveHundler;
            Game.PauseHandler += (s, e) =>
            {
                window.KeyPressed -= Game.KeyPressed;
                window.KeyReleased -= Game.KeyReleased;
                window.MouseMoved -= Game.MouseMove;
                window.MouseButtonReleased -= Game.MouseClick;
                updater = () =>
                {
                    window.Clear(Color.Black);
                    Game.Draw();
                    window.Draw(pauseMenu);
                };
                window.MouseMoved += pauseMenu.MoveHundler;
                window.MouseButtonReleased += pauseMenu.ClickHandler;
            };
            updater = () =>
            {
                Game.Update();
                window.Clear(Color.Black);
                Game.Draw();
            };
            window.KeyPressed += Game.KeyPressed;
            window.KeyReleased += Game.KeyReleased;
            window.MouseMoved += Game.MouseMove;
            window.MouseButtonReleased += Game.MouseClick;
        }
        private static void ContinueGame()
        {
            window.MouseButtonReleased -= pauseMenu.ClickHandler;
            window.MouseMoved -= pauseMenu.MoveHundler;
            updater = () =>
            {
                Game.Update();
                window.Clear(Color.Black);
                Game.Draw();
            };
            Game.Pause = false;
            window.KeyPressed += Game.KeyPressed;
            window.KeyReleased += Game.KeyReleased;
            window.MouseButtonReleased += Game.MouseClick;
            window.MouseMoved += Game.MouseMove;
        }
        private static void MainMenu()
        {
            window.MouseButtonReleased -= pauseMenu.ClickHandler;
            window.MouseMoved -= pauseMenu.MoveHundler;
            updater = () =>
            {

                window.Clear(Color.Black);
                window.Draw(mainMenu);
            };
            window.MouseMoved += mainMenu.MoveHundler;
            window.MouseButtonReleased += mainMenu.ClickHandler;
        }
        public static void Main(string[] args)
        {
            ContextSettings settings = new();
            settings.AntialiasingLevel = 8;
            window = new(new(1280, 720), "Title", Styles.Close, settings);
            mainMenu = new((Vector2f)window.Size);
            pauseMenu = new((Vector2f)window.Size);
            MainMenu();
            pauseMenu.ContinueButton.Click = (s, e) => ContinueGame();
            pauseMenu.ExitToMainMenuButton.Click = (s, e) => MainMenu();
            mainMenu.PlayButton.Click += (s, e) => NewGame();
            mainMenu.ExitButton.Click += (s, e) => window.Close();
            window.Closed += (s, e) => window.Close();
            while (window.IsOpen)
            {
                window.DispatchEvents();
                updater();
                window.Display();
            }
        }
        private static void DrawLoading()
        {
            Text text = new("Loading...", Content.Font);
            text.Origin = new Vector2f(text.GetLocalBounds().Width, text.GetLocalBounds().Height) / 2;
            text.Position = (Vector2f)window.Size / 2;
            window.Clear(Color.Black);
            window.Draw(text);
            window.Display();
        }
    }
}