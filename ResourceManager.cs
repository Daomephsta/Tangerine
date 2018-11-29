using OpenTK.Graphics.OpenGL;
using System;
using System.IO;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using Tangerine.GLObjects;
using opengl = OpenTK.Graphics.OpenGL;
using imaging = System.Drawing.Imaging;

namespace Tangerine
{
    public static class ResourceManager
    {
        public static Shader CreateShaderFromFile(ShaderType type, String shaderPath)
        {
            StringBuilder source = new StringBuilder();

            using (StreamReader reader = new StreamReader(Path.Combine(Environment.CurrentDirectory, shaderPath + ".glsl")))
            {
                while (reader.Peek() != -1)
                {
                    source.AppendLine(reader.ReadLine());
                }
            }
            return new Shader(type, shaderPath, source.ToString());
        }

        public static Texture CreateTextureFromFile(String filePath)
        {
            return CreateTextureFromFile(filePath, imaging::PixelFormat.Format24bppRgb, opengl::PixelFormat.Bgr);
        }

        public static Texture CreateTextureFromFile(String filePath, imaging::PixelFormat imageFormat, opengl::PixelFormat pixelFormat)
        {
            using (Bitmap bitmap = new Bitmap(filePath))
            {
                bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
                BitmapData imageData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, imageFormat);
                bitmap.UnlockBits(imageData);

                return new Texture(pixelFormat, bitmap.Width, bitmap.Height, imageData.Scan0);
            }
        }
    }
}
