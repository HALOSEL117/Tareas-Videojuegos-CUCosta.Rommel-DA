using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

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

            using (var game = new Game(GameWindowSettings.Default, nativeWindowSettings))
            {
                game.Run();
            }
        }
    }
}