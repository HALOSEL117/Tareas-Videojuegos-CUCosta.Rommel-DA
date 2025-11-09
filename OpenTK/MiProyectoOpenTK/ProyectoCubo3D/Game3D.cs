using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using System;
using System.IO;
using classlib; // ¡Usando nuestra librería!
using OpenTK.Windowing.GraphicsLibraryFramework; // Para el tiempo (GLFW)

namespace ProyectoCubo3D
{
    // Heredamos de GameWindow
    public class Game3D : GameWindow
    {
        // Vértices del Cubo
        // Un cubo tiene 6 caras, cada cara 2 triángulos, cada triángulo 3 vértices.
        // 6 * 2 * 3 = 36 vértices.
        // Cada vértice tiene Posición (3 floats) + Coordenada de Textura (2 floats)
        // (X, Y, Z, U, V)
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

        // Handles de OpenGL
        private int _vertexBufferObject;
        private int _vertexArrayObject;

        // Referencias a nuestras clases de la librería
        // El `null!` es para la advertencia CS8618 (confiamos que se inicializará en OnLoad)
        private Shader _shader = null!;
        private Texture _texture = null!;

        // Variables para las matrices 3D
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

            // Color de fondo (un azul oscuro)
            GL.ClearColor(0.1f, 0.1f, 0.2f, 1.0f);

            // Habilitar el Test de Profundidad (Z-buffer)
            // Esto es ESENCIAL para el 3D, para que las caras
            // de adelante tapen a las de atrás.
            GL.Enable(EnableCap.DepthTest);

            // --- 1. Configurar Buffers (VBO y VAO) ---
            // (No usamos EBO esta vez, dibujamos los 36 vértices directamente)

            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);

            // --- 2. Configurar Atributos de Vértices ---
            int stride = 5 * sizeof(float); // 3 de Pos + 2 de TexCoord

            // Atributo 0: Posición
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, stride, 0);
            GL.EnableVertexAttribArray(0);

            // Atributo 1: Coordenada de Textura
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, stride, 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            // --- 3. Cargar y Compilar Shaders ---
            // Leemos los archivos GLSL
            string vertexShaderSource = File.ReadAllText("shader3D.vert");
            string fragmentShaderSource = File.ReadAllText("shader3D.frag");

            // Creamos el shader usando nuestra clase de 'classlib'
            _shader = new Shader(vertexShaderSource, fragmentShaderSource);
            _shader.Use();

            // --- 4. Cargar Textura ---
            _texture = new Texture("textura_cubo.jpg");
            _shader.SetInt("uTexture0", 0); // Decirle al shader que use la Unidad 0

            // --- 5. Configurar Matrices 3D (Cámara) ---

            // Matriz de Modelo: Dónde está el objeto en el mundo (empezamos en el origen)
            _model = Matrix4.Identity;

            // Matriz de Vista: Dónde está la cámara
            // La movemos "hacia atrás" 3 unidades en Z para poder ver el cubo
            _view = Matrix4.CreateTranslation(0.0f, 0.0f, -3.0f);

            // Matriz de Proyección: Cómo se ve la perspectiva
            // (Campo de visión de 45°, relación de aspecto, 0.1f cerca, 100.0f lejos)
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

            // --- 6. Hacer Girar el Cubo ---
            // Obtenemos el tiempo total para que la rotación sea constante
            float time = (float)GLFW.GetTime();

            // Creamos una rotación que cambia con el tiempo
            // Girará en el eje X y en el eje Y
            _model = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(time * 20.0f)) *
                     Matrix4.CreateRotationY(MathHelper.DegreesToRadians(time * 30.0f));
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            // Limpiamos el buffer de color Y el buffer de profundidad
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // --- 7. Dibujar el Cubo ---
            _shader.Use();
            _texture.Use(TextureUnit.Texture0);
            GL.BindVertexArray(_vertexArrayObject);

            // Enviar nuestras matrices (Modelo, Vista, Proyección) al Vertex Shader
            // (El shader debe tener 'uniforms' llamados 'model', 'view' y 'projection')
            int modelLoc = _shader.GetUniformLocation("model");
            int viewLoc = _shader.GetUniformLocation("view");
            int projLoc = _shader.GetUniformLocation("projection");

            GL.UniformMatrix4(modelLoc, true, ref _model);
            GL.UniformMatrix4(viewLoc, true, ref _view);
            GL.UniformMatrix4(projLoc, true, ref _projection);

            // Dibujamos los 36 vértices
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);

            SwapBuffers();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);

            // Actualizar la Matriz de Proyección cuando la ventana cambia de tamaño
            _projection = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(45.0f),
                (float)e.Width / e.Height,
                0.1f,
                100.0f
            );
        }

        protected override void OnUnload()
        {
            // Liberar recursos
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