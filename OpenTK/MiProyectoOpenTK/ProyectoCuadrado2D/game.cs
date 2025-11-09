using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.IO;
using classlib; 

namespace MiProyectoOpenTK
{
    public class Game : GameWindow
    {
        // (Posición X, Y, Z) + (Coordenada Textura U, V)
        private readonly float[] _vertices =
        {
             // Posición           // Coordenadas de Textura
             0.5f,  0.5f, 0.0f,   1.0f, 1.0f, // Top Right
             0.5f, -0.5f, 0.0f,   1.0f, 0.0f, // Bottom Right
            -0.5f, -0.5f, 0.0f,   0.0f, 0.0f, // Bottom Left
            -0.5f,  0.5f, 0.0f,   0.0f, 1.0f  // Top Left
        };

        private readonly uint[] _indices =
        {
            0, 1, 3, // Primer triángulo
            1, 2, 3  // Segundo triángulo
        };

        private int _vertexBufferObject;
        private int _vertexArrayObject;
        private int _elementBufferObject;

        private Shader _shader = null!;
        private Texture _texture = null!;

        public Game(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
            this.VSync = VSyncMode.On;
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);

            _elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

            string assemblyDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            string vertPath = Path.Combine(assemblyDir, "shader.vert");
            string fragPath = Path.Combine(assemblyDir, "shader.frag");
            string vertCode = File.ReadAllText(vertPath);
            string fragCode = File.ReadAllText(fragPath);

            _shader = new Shader(vertCode, fragCode); 
            _shader.Use();

            int stride = 5 * sizeof(float);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, stride, 0);
            GL.EnableVertexAttribArray(0);

            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, stride, 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            _texture = new Texture(Path.Combine(assemblyDir, "textura.jpg"));
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit);

            _shader.Use();
            _texture.Use(TextureUnit.Texture0);
            _shader.SetInt("uTexture0", 0);

            GL.BindVertexArray(_vertexArrayObject);
            GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);

            SwapBuffers();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
        }

        protected override void OnUnload()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);

            GL.DeleteBuffer(_vertexBufferObject);
            GL.DeleteBuffer(_elementBufferObject);
            GL.DeleteVertexArray(_vertexArrayObject);
            
            _shader.Dispose();
            base.OnUnload();
        }
    }
}