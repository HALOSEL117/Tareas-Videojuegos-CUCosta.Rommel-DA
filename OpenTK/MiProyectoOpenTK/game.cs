using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Diagnostics;

// 3. ¡Importante! Usamos el namespace de nuestra librería
using classlib;

namespace MiProyectoOpenTK
{
    public class Game : GameWindow
    {
        private readonly float[] _vertices =
        {
             // Posición (XYZ)      // Coordenadas de Textura (UV)
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

        // 5. Usamos las clases de nuestra 'classlib'
        private Shader _shader;
        private Texture _texture;

        // Shaders incrustados
        private const string VertexShaderSource = @"
            #version 330 core
            layout(location = 0) in vec3 aPosition;
            layout(location = 1) in vec2 aTexCoord;
            out vec2 vTexCoord;
            void main(void)
            {
                vTexCoord = aTexCoord;
                gl_Position = vec4(aPosition, 1.0);
            }";

        private const string FragmentShaderSource = @"
            #version 330 core
            out vec4 FragColor;
            in vec2 vTexCoord;
            uniform sampler2D uTexture0;
            void main()
            {
                FragColor = texture(uTexture0, vTexCoord);
            }";


        public Game(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(0.1f, 0.1f, 0.2f, 1.0f);

            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);

            _elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

            int stride = 5 * sizeof(float);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, stride, 0);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, stride, 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            // 6. Creamos instancias de las clases de 'classlib'
            _shader = new Shader(VertexShaderSource, FragmentShaderSource);
            _shader.Use();

            _texture = new Texture("textura.jpg");
            _shader.SetInt("uTexture0", 0);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            _shader.Use();
            _texture.Use(TextureUnit.Texture0);
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
            GL.DeleteVertexArray(_vertexArrayObject);
            GL.DeleteBuffer(_elementBufferObject);

            _shader.Dispose();
            _texture.Dispose();
            base.OnUnload();
        }
    }
}