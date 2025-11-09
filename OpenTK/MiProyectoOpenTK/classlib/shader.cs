using OpenTK.Graphics.OpenGL4;
using System.Diagnostics;

namespace classlib
{
    public class Shader : IDisposable
    {
        public readonly int Handle;
        public Shader(string vertexSource, string fragmentSource)
        {
            // --- 1. Compilar Vertex Shader ---
            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertexSource);
            GL.CompileShader(vertexShader);
            GL.GetShader(vertexShader, ShaderParameter.CompileStatus, out int success);
            if (success == 0)
            {
                string infoLog = GL.GetShaderInfoLog(vertexShader);

                throw new Exception($"Error de compilación del Vertex Shader:\n{infoLog}");
            }


            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, fragmentSource);
            GL.CompileShader(fragmentShader);


            GL.GetShader(fragmentShader, ShaderParameter.CompileStatus, out success);
            if (success == 0)
            {
                string infoLog = GL.GetShaderInfoLog(fragmentShader);

                throw new Exception($"Error de compilación del Fragment Shader:\n{infoLog}");
            }
            Handle = GL.CreateProgram();
            GL.AttachShader(Handle, vertexShader);
            GL.AttachShader(Handle, fragmentShader);
            GL.LinkProgram(Handle);


            GL.GetProgram(Handle, GetProgramParameterName.LinkStatus, out success);
            if (success == 0)
            {
                string infoLog = GL.GetProgramInfoLog(Handle);
                throw new Exception($"Error de enlace (Link) del Programa de Shader:\n{infoLog}");
            }
            GL.DetachShader(Handle, vertexShader);
            GL.DetachShader(Handle, fragmentShader);
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);
        }
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
            GL.ProgramUniform1(Handle, location, value);
        }

        public void Dispose()
        {
            GL.DeleteProgram(Handle);
            GC.SuppressFinalize(this);
        }
    }
}