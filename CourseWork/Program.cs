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
        private static Menu deathMenu;
        private static Menu winMenu;
        private static Updater updater;
        private static void NewGame(Menu sender)
        {
            
            DrawLoading();
            Random random = new();
            Game = new(random.Next());
            window.MouseButtonReleased -= sender.ClickHandler;
            window.MouseMoved -= sender.MoveHundler;

            Game.PauseHandler += (s, e) =>
            {
                window.KeyPressed -= Game.KeyPressed;
                window.KeyReleased -= Game.KeyReleased;
                window.MouseButtonReleased -= Game.MouseClick;
                updater = () =>
                {
                    Game.Update();
                    window.Clear(Color.Black);
                    Game.Draw();
                    window.Draw(pauseMenu);
                };
                window.MouseMoved += pauseMenu.MoveHundler;
                window.MouseButtonReleased += pauseMenu.ClickHandler;
            };
            Game.PlayerDeadHandler += (s, e) =>
            {
                window.KeyPressed -= Game.KeyPressed;
                window.KeyReleased -= Game.KeyReleased;
                window.MouseButtonReleased -= Game.MouseClick;
                updater = () =>
                {
                    window.Clear(Color.Black);
                    Game.Draw();
                    window.Draw(deathMenu);
                };
                window.MouseMoved += deathMenu.MoveHundler;
                window.MouseButtonReleased += deathMenu.ClickHandler;
            };
            Game.PlayerWinHandler += (s, e) =>
            {
                window.KeyPressed -= Game.KeyPressed;
                window.KeyReleased -= Game.KeyReleased;
                window.MouseButtonReleased -= Game.MouseClick;
                updater = () =>
                {
                    window.Clear(Color.Black);
                    Game.Draw();
                    window.Draw(winMenu);
                };
                window.MouseMoved += winMenu.MoveHundler;
                window.MouseButtonReleased += winMenu.ClickHandler;
            };
            updater = () =>
            {
                Game.Update();
                window.Clear(Color.Black);
                Game.Draw();
            };
            window.KeyPressed += Game.KeyPressed;
            window.KeyReleased += Game.KeyReleased;
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
        }
        private static void MainMenu(Menu sender)
        {
            Text gameName = new("Red Riding Hood:", Content.Font, 20);
            gameName.Origin = new(gameName.GetLocalBounds().Width / 2, 0);
            gameName.Position = new((float)window.Size.X / 2, 220);
            window.MouseButtonReleased -= sender.ClickHandler;
            window.MouseMoved -= sender.MoveHundler;
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
            window = new(new(1280, 720), "Red Riding Hood: Way home", Styles.Close, settings);

            Button m1 = new("Play", new(200, 50)) { FillColor = Color.Black, TextColor = Color.White, OutlineThickness = 5, OutlineColor = Color.White, Click = (s,e) => NewGame(mainMenu) };
            //Button LoadButton = new("Load game", new(200, 50)) { FillColor = Color.Black, TextColor = Color.White, OutlineThickness = 5, OutlineColor = Color.White };
            Button m3 = new("Exit", new(200, 50)) { FillColor = Color.Black, TextColor = Color.White, OutlineThickness = 5, OutlineColor = Color.White, Click = (s, e) => window.Close() };
            mainMenu = new((Vector2f)window.Size, "Way Home");
            mainMenu.AddButton(m1);
            //mainMenu.AddButton(LoadButton);
            mainMenu.AddButton(m3);

            pauseMenu = new((Vector2f)window.Size, "Pause");
            Button p1 = new("Continue", new(200, 50)) { FillColor = Color.Black, TextColor = Color.White, OutlineThickness = 5, OutlineColor = Color.White, Click = (s, e) => ContinueGame() };
            Button p2 = new("Exit", new(200, 50)) { FillColor = Color.Black, TextColor = Color.White, OutlineThickness = 5, OutlineColor = Color.White, Click = (s, e) => MainMenu(pauseMenu) };
            pauseMenu.AddButton(p1);
            pauseMenu.AddButton(p2);

            deathMenu = new((Vector2f)window.Size, "You dead");
            Button d1 = new("Restart", new(200, 50)) { FillColor = Color.Black, TextColor = Color.White, OutlineThickness = 5, OutlineColor = Color.White, Click = (s, e) => NewGame(deathMenu) };
            Button d2 = new("Exit", new(200, 50)) { FillColor = Color.Black, TextColor = Color.White, OutlineThickness = 5, OutlineColor = Color.White, Click = (s, e) => MainMenu(deathMenu) };
            deathMenu.AddButton(d1);
            deathMenu.AddButton(d2);

            winMenu = new((Vector2f)window.Size, "YOU SURVIVED!!!");
            Button w1 = new("New game", new(200, 50)) { FillColor = Color.Black, TextColor = Color.White, OutlineThickness = 5, OutlineColor = Color.White, Click = (s, e) => NewGame(winMenu) };
            Button w2 = new("Exit", new(200, 50)) { FillColor = Color.Black, TextColor = Color.White, OutlineThickness = 5, OutlineColor = Color.White, Click = (s, e) => MainMenu(winMenu) };
            winMenu.AddButton(w1);
            winMenu.AddButton(w2);

            window.Closed += (s, e) => window.Close();
            MainMenu(pauseMenu);

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