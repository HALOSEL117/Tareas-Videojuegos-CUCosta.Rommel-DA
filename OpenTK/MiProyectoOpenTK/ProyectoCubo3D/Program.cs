using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace ProyectoCubo3D
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            var nativeWindowSettings = new NativeWindowSettings()
            {
                Size = new Vector2i(800, 600),
                Title = "Mi Cubo 3D Giratorio con OpenTK",
                Flags = ContextFlags.ForwardCompatible,
            };

            using (var game = new Game3D(GameWindowSettings.Default, nativeWindowSettings))
            {
                game.Run();
            }
        }
    }
}