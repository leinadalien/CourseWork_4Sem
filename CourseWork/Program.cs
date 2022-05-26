using System;
using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;
using SFML.Window;

namespace CourseWork
{
    internal class Program
    {
        private static RenderWindow window;
        public static RenderWindow Window { get { return window; } }
        public static Game Game { private set; get; }
        public static void Main(string[] args)
        {
            ContextSettings settings = new();
            settings.AntialiasingLevel = 8;
            window = new(new(1280, 720), "Title", Styles.Close, settings);
            window.Closed += (sender, e) => window.Close();
            window.Resized += (sender, e) => window.SetView(new(new FloatRect(0, 0, e.Width, e.Height)));
            Game = new();
            Content.Load();
            window.KeyPressed += Game.KeyPressed;
            while ( window.IsOpen) {
                
                window.DispatchEvents();
                Game.Update();
                window.Clear(Color.Black);
                Game.Draw();
                window.Display();
            }
        }
    }
}