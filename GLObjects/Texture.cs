using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using opengl = OpenTK.Graphics.OpenGL;
using imaging = System.Drawing.Imaging;
using log4net;

namespace Tangerine.GLObjects
{
    public struct Texture
    {
        private static readonly ILog LOGGER = LogManager.GetLogger(typeof(Texture));
        internal readonly int glID;

        public Texture(String fileName) : this(fileName, imaging::PixelFormat.Format24bppRgb, opengl::PixelFormat.Bgr) { }

        public Texture(String fileName, imaging::PixelFormat imageFormat, opengl::PixelFormat format)
        {
            glID = GL.GenTexture();
            LOGGER.Debug($"Loading texture from {fileName} into GL texture {glID} Image Format: {imageFormat} Pixel Format: {format}");
            using (Bitmap bitmap = new Bitmap(fileName))
            {
                bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
                BitmapData imageData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, imageFormat);

                GL.BindTexture(TextureTarget.Texture2D, glID);

                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, bitmap.Width, bitmap.Height, 0, format, PixelType.UnsignedByte, imageData.Scan0);
                GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

                //Cleanup
                GL.BindTexture(TextureTarget.Texture2D, 0);
                bitmap.UnlockBits(imageData);
            }
            LOGGER.Info($"Loaded texture from {fileName}");
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
