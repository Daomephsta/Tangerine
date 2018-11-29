using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using opengl = OpenTK.Graphics.OpenGL;
using imaging = System.Drawing.Imaging;
using log4net;

namespace Tangerine.GLObjects
{
    public class Texture
    {
        private static readonly ILog LOGGER = LogManager.GetLogger(typeof(Texture));
        internal readonly int glID;

        public Texture(opengl::PixelFormat format, int width, int height, IntPtr pixels)
        {
            glID = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, glID);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, width, height, 0, format, PixelType.UnsignedByte, pixels);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            //Cleanup
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        public void Bind()
        {
            GL.BindTexture(TextureTarget.Texture2D, glID);
        }

        public void Unbind()
        {
            GL.BindTexture(TextureTarget.Texture2D, glID);
        }
    }
}
