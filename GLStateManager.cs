using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Graphics.OpenGL;
using Tangerine.GLObjects;

namespace Tangerine
{
    /// <summary>
    /// Manages the GL state. Performs tasks like rebinding previously bound objects upon the ucrrent object being unbound. 
    /// </summary>
    public static class GLStateManager
    {
        public static ShaderProgram BoundShaderProgram
        {
            get => boundShaderPrograms.Count > 0 ? boundShaderPrograms.Peek() : null;
        }
        private static readonly Stack<ShaderProgram> boundShaderPrograms = new Stack<ShaderProgram>();

        internal static void UseShaderProgram(ShaderProgram program)
        {
            if (boundShaderPrograms.Contains(program)) throw new InvalidOperationException("Attempted to bind already bound shader program. ID: " + program.glID);
            GL.UseProgram(program.glID);
            boundShaderPrograms.Push(program);
        }

        internal static void RevertShaderProgram()
        {
            boundShaderPrograms.Pop();
            if (boundShaderPrograms.Count != 0) GL.UseProgram(boundShaderPrograms.Peek().glID);
            else throw new InvalidOperationException("No previous shader program found to revert to.");
        }
    }
}
