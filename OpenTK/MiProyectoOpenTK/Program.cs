using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

// 1. Definimos un namespace para nuestra aplicación
namespace MiProyectoOpenTK
{
    public static class Program
    {
        public static void Main()
        {
            var nativeWindowSettings = new NativeWindowSettings()
            {
                Size = new Vector2i(800, 600),
                Title = "Ejemplo con classlib e Herencia",
                Flags = ContextFlags.ForwardCompatible,
            };

            // 2. Aquí creamos una instancia de nuestra clase Game
            //    (que está en Game.cs y hereda de GameWindow)
            using (var game = new Game(GameWindowSettings.Default, nativeWindowSettings))
            {
                game.Run();
            }
        }
    }
}