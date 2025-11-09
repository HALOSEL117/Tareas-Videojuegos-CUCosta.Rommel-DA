using OpenTK.Graphics.OpenGL4;
using System.Diagnostics;

// 1. Definimos un namespace para nuestra librería
namespace classlib
{
    // 2. Hacemos la clase 'public' para que MiProyectoOpenTK pueda verla
    public class Shader : IDisposable
    {
        public readonly int Handle;

        // --- ESTA ES LA ACTUALIZACIÓN ---
        // Constructor que acepta código de shader directamente
        public Shader(string vertexSource, string fragmentSource)
        {
            // --- 1. Compilar Vertex Shader ---
            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertexSource);
            GL.CompileShader(vertexShader);

            // Comprobar errores de compilación
            GL.GetShader(vertexShader, ShaderParameter.CompileStatus, out int success);
            if (success == 0)
            {
                string infoLog = GL.GetShaderInfoLog(vertexShader);
                // Lanzamos una excepción para detener el programa y mostrar el error
                throw new Exception($"Error de compilación del Vertex Shader:\n{infoLog}");
            }

            // --- 2. Compilar Fragment Shader ---
            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, fragmentSource);
            GL.CompileShader(fragmentShader);

            // Comprobar errores de compilación
            GL.GetShader(fragmentShader, ShaderParameter.CompileStatus, out success);
            if (success == 0)
            {
                string infoLog = GL.GetShaderInfoLog(fragmentShader);
                // Lanzamos una excepción para detener el programa y mostrar el error
                throw new Exception($"Error de compilación del Fragment Shader:\n{infoLog}");
            }

            // --- 3. Crear y Enlazar Programa de Shader ---
            Handle = GL.CreateProgram();
            GL.AttachShader(Handle, vertexShader);
            GL.AttachShader(Handle, fragmentShader);
            GL.LinkProgram(Handle);

            // Comprobar errores de enlace
            GL.GetProgram(Handle, GetProgramParameterName.LinkStatus, out success);
            if (success == 0)
            {
                string infoLog = GL.GetProgramInfoLog(Handle);
                // Lanzamos una excepción
                throw new Exception($"Error de enlace (Link) del Programa de Shader:\n{infoLog}");
            }

            // --- 4. Limpieza ---
            // Los shaders individuales ya no son necesarios después de enlazarlos
            GL.DetachShader(Handle, vertexShader);
            GL.DetachShader(Handle, fragmentShader);
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);
        }
        // --- FIN DE LA ACTUALIZACIÓN ---


        public void Use()
        {
            GL.UseProgram(Handle);
        }

        public int GetAttribLocation(string attribName)
        {
            return GL.GetAttribLocation(Handle, attribName);
        }

        public int GetUniformLocation(string uniformName)
        {
            return GL.GetUniformLocation(Handle, uniformName);
        }

        public void SetInt(string name, int value)
        {
            int location = GetUniformLocation(name);
            // Usar 'GL.ProgramUniform1' es más seguro si el shader no está activo
            GL.ProgramUniform1(Handle, location, value);
        }

        public void Dispose()
        {
            GL.DeleteProgram(Handle);
            GC.SuppressFinalize(this);
        }
    }
}