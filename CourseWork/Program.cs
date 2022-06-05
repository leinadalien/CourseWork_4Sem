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
        private static Menu mainMenu;
        private static Menu pauseMenu;
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
            Text gameName = new("Red Riding Hood:", Content.Font, 20);
            gameName.Origin = new(gameName.GetLocalBounds().Width / 2, 0);
            gameName.Position = new((float)window.Size.X / 2, 220);
            window.MouseButtonReleased -= pauseMenu.ClickHandler;
            window.MouseMoved -= pauseMenu.MoveHundler;
            updater = () =>
            {
                window.Clear(Color.Black);
                window.Draw(mainMenu);
                window.Draw(gameName);
            };
            window.MouseMoved += mainMenu.MoveHundler;
            window.MouseButtonReleased += mainMenu.ClickHandler;
        }
        public static void Main(string[] args)
        {
            ContextSettings settings = new();
            settings.AntialiasingLevel = 8;
            window = new(new(1280, 720), "Title", Styles.Close, settings);

            Button PlayButton = new("New game", new(200, 50)) { FillColor = Color.Black, TextColor = Color.White, OutlineThickness = 5, OutlineColor = Color.White, Click = (s,e) => NewGame() };
            //Button LoadButton = new("Load game", new(200, 50)) { FillColor = Color.Black, TextColor = Color.White, OutlineThickness = 5, OutlineColor = Color.White };
            Button ExitButton = new("Exit", new(200, 50)) { FillColor = Color.Black, TextColor = Color.White, OutlineThickness = 5, OutlineColor = Color.White, Click = (s, e) => window.Close() };
            mainMenu = new((Vector2f)window.Size, "Way Home");
            mainMenu.AddButton(PlayButton);
            //mainMenu.AddButton(LoadButton);
            mainMenu.AddButton(ExitButton);

            pauseMenu = new((Vector2f)window.Size, "Pause");
            Button ContinueButton = new("Continue", new(200, 50)) { FillColor = Color.Black, TextColor = Color.White, OutlineThickness = 5, OutlineColor = Color.White, Click = (s, e) => ContinueGame() };
            Button ToMainButton = new("Exit", new(200, 50)) { FillColor = Color.Black, TextColor = Color.White, OutlineThickness = 5, OutlineColor = Color.White, Click = (s, e) => MainMenu() };
            pauseMenu.AddButton(ContinueButton);
            pauseMenu.AddButton(ToMainButton);

            window.Closed += (s, e) => window.Close();
            MainMenu();

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