using OpenTK.Graphics.OpenGL4;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Diagnostics;
using System;
namespace classlib
{
    public class Texture : IDisposable
    {
        public readonly int Handle;

        public Texture(string path)
        {
            Handle = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, Handle);

            try
            {
                using (var image = Image.Load<Rgba32>(path))
                {
                    image.Mutate(x => x.Flip(FlipMode.Vertical));
                    var pixels = new byte[image.Width * image.Height * 4];
                    image.CopyPixelDataTo(pixels);

                    GL.TexImage2D(
                        TextureTarget.Texture2D,
                        0,
                        PixelInternalFormat.Rgba,
                        image.Width,
                        image.Height,
                        0,
                        PixelFormat.Rgba,
                        PixelType.UnsignedByte,
                        pixels
                    );
                }
            }
            catch (FileNotFoundException e)
            {
                Debug.WriteLine($"Error: No se encontró el archivo de textura en '{path}'.");
                Debug.WriteLine("Asegúrate de que 'textura.jpg' esté en la carpeta de salida (ej. bin/Debug/...).");
                throw;
            }

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }

        public void Use(TextureUnit unit)
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, Handle);
        }

        public void Dispose()
        {
            GL.DeleteTexture(Handle);
            GC.SuppressFinalize(this);
        }
    }
}