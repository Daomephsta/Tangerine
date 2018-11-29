using log4net;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;

namespace Tangerine.GLObjects
{
    public class ShaderProgram
    {
        private static readonly ILog LOGGER = LogManager.GetLogger(typeof(Shader));
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
            LOGGER.Debug($"Attempting to link shader program '{name}'");
            //Check the link succeeded
            int success;
            GL.GetProgram(this.glID, GetProgramParameterName.LinkStatus, out success);
            if (success == 0)
            {
                string errors;
                GL.GetProgramInfoLog(this.glID, out errors);
                LOGGER.Error($"Linking of shader program '{name}' failed\n{errors}");
            }
            else
                LOGGER.Info($"Shader program '{name}' linked successfully!");
        }

        public void Use()
        {
            GLStateManager.UseShaderProgram(this);
        }

        public void Revert()
        {
            CheckInUse("Attempted to revert to previous shader using shader program that is not in use");
            GLStateManager.RevertShaderProgram();
        }

        public int GetUniformLocation(string uniformName)
        {
            CheckInUse("Attempted to get uniform of shader program that is not in use");
            return GL.GetUniformLocation(this.glID, uniformName);
        }

        public void SetUniform(string uniformName, int value)
        {
            CheckInUse("Attempted to set uniform of shader program that is not in use");
            GL.Uniform1(GL.GetUniformLocation(this.glID, uniformName), value);
        }

        public void SetUniform(string uniformName, float value)
        {
            CheckInUse("Attempted to set uniform of shader program that is not in use");
            GL.Uniform1(GL.GetUniformLocation(this.glID, uniformName), value);
        }

        public void SetUniform(string uniformName, float x, float y, float z)
        {
            SetUniform(uniformName, new Vector3(x, y, z));
        }

        public void SetUniform(string uniformName, Vector3 value)
        {
            CheckInUse("Attempted to set uniform of shader program that is not in use");
            GL.Uniform3(GL.GetUniformLocation(this.glID, uniformName), value);
        }

        public void SetUniform(string uniformName, Matrix4 value)
        {
            CheckInUse("Attempted to set uniform of shader program that is not in use");
            GL.UniformMatrix4(GL.GetUniformLocation(this.glID, uniformName), false, ref value);
        }

        private void CheckInUse(string message)
        {
            if (GLStateManager.BoundShaderProgram != this) 
                throw new InvalidOperationException(message);
        }
    }
}
