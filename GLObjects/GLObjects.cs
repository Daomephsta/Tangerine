using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using opengl = OpenTK.Graphics.OpenGL;
using imaging = System.Drawing.Imaging;

namespace Tangerine
{
    public class Shader
    {
        internal readonly int glID;

        public Shader(ShaderType type, string shaderPath, params string[] shaderSource)
        {
            this.glID = GL.CreateShader(type);
            GL.ShaderSource(this.glID, shaderSource.Length, shaderSource, (int[])null);
            GL.CompileShader(this.glID);
            Console.WriteLine("Compiling shader: " + shaderPath + ".glsl");
            //Check the shader compiled successfully
            int success;
            GL.GetShader(this.glID, ShaderParameter.CompileStatus, out success);
            if (success == 0)
            {
                Console.WriteLine("Could not compile shader: " + shaderPath + ".glsl");
                string errors;
                GL.GetShaderInfoLog(this.glID, out errors);
                Console.WriteLine(errors);
            }
            else
            {
                Console.WriteLine("Shader: " + shaderPath + ".glsl compiled successfully!");
            }
        }

        public void Delete()
        {
            GL.DeleteShader(this.glID);
        }

        public static void DeleteShaders(Shader[] shaders)
        {
            foreach (Shader shader in shaders)
            {
                GL.DeleteShader(shader.glID);
            }
        }
    }

    public class ShaderProgram
    {
        internal readonly int glID;
        private readonly string name;

        public ShaderProgram(string name)
        {
            this.glID = GL.CreateProgram();
            this.name = name;
        }

        public void Attach(Shader shader, bool delete = false)
        {
            GL.AttachShader(this.glID, shader.glID);
            if (delete) shader.Delete();
        }

        public void Link()
        {
            GL.LinkProgram(this.glID);
            Console.WriteLine("Attempting to link shader program: " + name);
            //Check the link succeeded
            int success;
            GL.GetProgram(this.glID, GetProgramParameterName.LinkStatus, out success);
            if (success == 0)
            {
                Console.WriteLine("Linking of shader program: " + name + " failed");
                string errors;
                GL.GetProgramInfoLog(this.glID, out errors);
                Console.WriteLine(errors);
            }
            else
            {
                Console.WriteLine("Shader program: " + name + " linked successfully!");
            }
        }

        public void Use()
        {
            GLStateManager.UseShaderProgram(this);
        }

        public void Revert()
        {
            if (GLStateManager.BoundShaderProgram == this) GLStateManager.RevertShaderProgram();
            else throw new InvalidOperationException("Attempted to revert to previous shader using shader that is not in use");
        }

        public int GetUniformLocation(string uniformName)
        {
            return GL.GetUniformLocation(this.glID, uniformName);
        }

        public void SetUniform(string uniformName, int value)
        {
            GL.Uniform1(GL.GetUniformLocation(this.glID, uniformName), value);
        }

        public void SetUniform(string uniformName, float value)
        {
            GL.Uniform1(GL.GetUniformLocation(this.glID, uniformName), value);
        }

        public void SetUniform(string uniformName, Matrix4 value)
        {
            GL.UniformMatrix4(GL.GetUniformLocation(this.glID, uniformName), false, ref value);
        }
    }

    public class VertexBuffer
    {
        internal readonly int glID;

        public VertexBuffer()
        {
            this.glID = GL.GenBuffer();
        }

        public void Bind()
        {
            GLStateManager.BindVertexBuffer(this);
        }

        public void Unbind()
        {
            if (GLStateManager.BoundVertexBuffer == this) GLStateManager.UnbindVertexBuffer();
            else throw new InvalidOperationException("Attempted to unbind buffer that is already unbound");
        }

        public void SetData(float[] vertices, BufferUsageHint usageHint)
        {
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * vertices.Length, vertices, usageHint);
        }

        public void Delete()
        {
            GL.DeleteBuffer(this.glID);
        }

        public static void DeleteVBOS(VertexBuffer[] buffers)
        {
            foreach (VertexBuffer buffer in buffers)
            {
                GL.DeleteBuffer(buffer.glID);
            }
        }
    }

    public class ElementBuffer
    {
        internal readonly int glID;

        public ElementBuffer()
        {
            this.glID = GL.GenBuffer();
        }

        public void Bind()
        {
            GLStateManager.BindElementBuffer(this);
        }

        public void Unbind()
        {
            if (GLStateManager.BoundElementBuffer == this) GLStateManager.UnbindElementBuffer();
            else throw new InvalidOperationException("Attempted to unbind buffer that is already unbound");
        }

        public void SetData(int[] indices, BufferUsageHint usageHint)
        {
            GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(int) * indices.Length, indices, usageHint);
        }

        public void Delete()
        {
            GL.DeleteBuffer(this.glID);
        }

        public static void DeleteEBOS(ElementBuffer[] buffers)
        {
            foreach (ElementBuffer buffer in buffers)
            {
                GL.DeleteBuffer(buffer.glID);
            }
        }
    }

    public struct Texture
    {
        internal readonly int glID;

        public Texture(String fileName) : this(fileName, imaging::PixelFormat.Format24bppRgb, opengl::PixelFormat.Bgr) { }

        public Texture(String fileName, imaging::PixelFormat imageFormat, opengl::PixelFormat format)
        {
            glID = GL.GenTexture();
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
