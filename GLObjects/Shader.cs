using OpenTK.Graphics.OpenGL;
using System;
using log4net;

namespace Tangerine.GLObjects
{
    public class Shader
    {
        private static readonly ILog LOGGER = LogManager.GetLogger(typeof(Shader));
        internal readonly int glID;

        public Shader(ShaderType type, string shaderPath, params string[] shaderSource)
        {
            this.glID = GL.CreateShader(type);
            GL.ShaderSource(this.glID, shaderSource.Length, shaderSource, (int[])null);
            GL.CompileShader(this.glID);
            LOGGER.Debug($"Compiling {type} from {shaderPath}.glsl");
            //Check the shader compiled successfully
            int success;
            GL.GetShader(this.glID, ShaderParameter.CompileStatus, out success);
            if (success == 0)
            {
                LOGGER.Error($"Could not compile shader from {shaderPath}.glsl");
                string errors;
                GL.GetShaderInfoLog(this.glID, out errors);
                Console.WriteLine(errors);
            }
            else
                LOGGER.Info($"Shader successfully compiled from {shaderPath}.glsl !");
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
}
