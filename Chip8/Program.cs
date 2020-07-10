using System;
using System.IO;
using System.Threading;
using OpenToolkit.Mathematics;
using OpenToolkit.Windowing.Desktop;
using OpenToolkit.Windowing.Common;

namespace Chip8
{
    class Program
    {
        static void Main(string[] args)
        {
            var gameSettings = new GameWindowSettings
            {
                RenderFrequency = 60,
                UpdateFrequency = 600
            };

            var nativeSettings = new NativeWindowSettings
            {
                Size = new Vector2i(1024, 512),
                Profile = ContextProfile.Compatability,
                Title = "Chip8"
            };

            var window = new Window(gameSettings, nativeSettings);
            window.Run();            
        }
    }
}
