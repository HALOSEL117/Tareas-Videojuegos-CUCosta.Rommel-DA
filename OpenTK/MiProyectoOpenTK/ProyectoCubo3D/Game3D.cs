using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using System;
using System.IO;
using classlib;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace ProyectoCubo3D
{
    public class Game3D : GameWindow
    {
        private readonly float[] _vertices =
        {
            // Cara Trasera (-) Z
            -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,
             0.5f, -0.5f, -0.5f,  1.0f, 0.0f,
             0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
             0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
            -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,

            // Cara Delantera (+) Z
            -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
             0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
             0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
             0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
            -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
            -0.5f,  0.5f,  0.5f,  0.0f, 1.0f,

            // Cara Izquierda (-) X
            -0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
            -0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
            -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
            -0.5f,  0.5f,  0.5f,  1.0f, 0.0f,

            // Cara Derecha (+) X
             0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
             0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
             0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
             0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
             0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
             0.5f, -0.5f,  0.5f,  0.0f, 0.0f,

            // Cara Inferior (-) Y
            -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
             0.5f, -0.5f, -0.5f,  1.0f, 1.0f,
             0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
             0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
            -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,

            // Cara Superior (+) Y
            -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
             0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
             0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
             0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
            -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
            -0.5f,  0.5f,  0.5f,  0.0f, 0.0f
        };
        private int _vertexBufferObject;
        private int _vertexArrayObject;

        private Shader _shader = null!;
        private Texture _texture = null!;

        private Matrix4 _model;
        private Matrix4 _view;
        private Matrix4 _projection;

        public Game3D(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.1f, 0.1f, 0.2f, 1.0f);
            GL.Enable(EnableCap.DepthTest);

            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);

            int stride = 5 * sizeof(float);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, stride, 0);
            GL.EnableVertexAttribArray(0);

            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, stride, 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            string exeDirectory = AppDomain.CurrentDomain.BaseDirectory;

            string vertPath = Path.Combine(exeDirectory, "shader3D.vert");
            string fragPath = Path.Combine(exeDirectory, "shader3D.frag");
            string texPath = Path.Combine(exeDirectory, "textura_cubo.jpg");

            string vertexShaderSource = File.ReadAllText(vertPath);
            string fragmentShaderSource = File.ReadAllText(fragPath);

            _shader = new Shader(vertexShaderSource, fragmentShaderSource);
            _shader.Use();

            _texture = new Texture(texPath);
            _shader.SetInt("uTexture0", 0);

            //Configurar Matrices 3D (Cámara)
            _model = Matrix4.Identity;
            _view = Matrix4.CreateTranslation(0.0f, 0.0f, -3.0f);
            _projection = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(45.0f),
                (float)Size.X / Size.Y,
                0.1f,
                100.0f
            );
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            float time = (float)GLFW.GetTime();
            _model = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(time * 20.0f)) *
                     Matrix4.CreateRotationY(MathHelper.DegreesToRadians(time * 30.0f));
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            _shader.Use();
            _texture.Use(TextureUnit.Texture0);
            GL.BindVertexArray(_vertexArrayObject);

            int modelLoc = _shader.GetUniformLocation("model");
            int viewLoc = _shader.GetUniformLocation("view");
            int projLoc = _shader.GetUniformLocation("projection");

            GL.UniformMatrix4(modelLoc, false, ref _model);
            GL.UniformMatrix4(viewLoc, false, ref _view);
            GL.UniformMatrix4(projLoc, false, ref _projection);

            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);

            SwapBuffers();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
            _projection = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(45.0f),
                (float)e.Width / e.Height,
                0.1f,
                100.0f
            );
        }

        protected override void OnUnload()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);

            GL.DeleteBuffer(_vertexBufferObject);
            GL.DeleteVertexArray(_vertexArrayObject);

            _shader.Dispose();
            _texture.Dispose();

            base.OnUnload();
        }
    }
}