using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dresmor.Gui;
using Dresmor.System;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Dresmor
{
    class Program
    {
        static void Main(string[] args)
        {
            RenderWindow window = new RenderWindow(new VideoMode(800, 600), "Dresmor");
            window.SetFramerateLimit(60u);

            HudGui hud = new HudGui
            {
                Size = new UDim2(0, window.Size.X, 0, window.Size.Y)
            };

            BaseGui gui_1 = new BaseGui
            {
                Parent = hud,
                ShapeType = ShapeTypes.Ellipse,
                Texture = new Texture("lego.jpg"),
                Size = new UDim2(0.3f, 0, 0.3f, 0),
                Origin = new UDim2(0.5f, 0, 0.5f, 0),
                Position = new UDim2(1.0f, 0.0f, 0.5f, 0.0f)
            };
            BaseGui gui_2 = new BaseGui
            {
                Parent = gui_1,
                ShapeType = ShapeTypes.Ellipse,
                Texture = new Texture("lego.jpg"),
                Size = new UDim2(0.5f, 0, 0.5f, 0),
                Origin = new UDim2(0.5f, 0, 0.5f, 0),
                Position = new UDim2(0.5f, 0.0f, 0.5f, 0.0f)
            };
            BaseGui gui_3 = new BaseGui
            {
                Parent = gui_2,
                ShapeType = ShapeTypes.Ellipse,
                Texture = new Texture("lego.jpg"),
                Size = new UDim2(0.5f, 0, 0.5f, 0),
                Origin = new UDim2(0.5f, 0, 0.5f, 0),
                Position = new UDim2(0.5f, 0.0f, 0.5f, 0.0f),
                Collide = true
            };

            ButtonGui label = new ButtonGui
            {
                Size = new UDim2(0.5f, 0.0f, 0.5f, 0.0f),
                TextString = "Hi",
                TextFont = new Font("C:/Windows/Fonts/Arial.ttf")
            };

            gui_3.MouseDoubleClick += (s, e) => Console.WriteLine("Double Click: {0}", e.Position);

            ClockGui clockGui = new ClockGui
            {
                Parent = hud,
                ShapeType = ShapeTypes.Ellipse,
                Position = new UDim2(0, 100, 0, 100),
                Size = new UDim2(0, 200, 0, 200),
                TimeScale = 10.0f,
                Precision = 0.0001f,
                TimeHours = 12.0f,
                Texture = new Texture("clock.png"),
                TextureRect = new IntRect(0, 0, 600, 600)
            };

            label.Parent = clockGui;
            gui_1.Parent = clockGui;

            clockGui.AddTweenSize(new UDim2(1, 0, 1, 0), 10.0f, false, (g, v) => g.AddTweenSize(v.From, v.Seconds, false, v.Callback));

            SliderGui slider = new SliderGui();
            slider.Size = new UDim2(0, 20, 0, 300);
            slider.SliderValueChanged += (s, e) => { Console.WriteLine("{0}", e); };
            slider.Parent = hud;

            window.Closed += (sender, e) => window.Close();
            window.Resized += (s, e) => { window.SetView(new View(new FloatRect(0, 0, e.Width, e.Height))); hud.Size = new UDim2(0, e.Width, 0, e.Height); };
            window.MouseMoved += (s, e) => hud.ApplyMouseMovement(new MouseInput(new Vector2f(e.X, e.Y)));
            window.MouseButtonPressed += (s, e) => hud.ApplyMouseAction(new MouseInput(new Vector2f(e.X, e.Y), e.Button, true));
            window.MouseButtonReleased += (s, e) => hud.ApplyMouseAction(new MouseInput(new Vector2f(e.X, e.Y), e.Button, false));

            Clock clock = new Clock();
            
            while (window.IsOpen)
            {
                window.DispatchEvents();
                window.Clear(Color.Cyan);
                gui_1.Rotation += (Math.Sin(clock.ElapsedTime.AsSeconds()) * Math.PI * 2.0f).ToFloat();
                gui_2.Rotation -= (Math.Cos(clock.ElapsedTime.AsSeconds()) * Math.PI * 2.0f).ToFloat();
                gui_3.Rotation -= (Math.Sin(clock.ElapsedTime.AsSeconds()) * Math.PI * 2.0f).ToFloat();
                window.Draw(hud);
                window.Display();
            }
        }
    }
}
