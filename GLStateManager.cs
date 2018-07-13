using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Graphics.OpenGL;

namespace Tangerine
{
    /// <summary>
    /// Manages the GL state. Performs tasks like rebinding previously bound objects upon the ucrrent object being unbound. 
    /// </summary>
    public static class GLStateManager
    {
        public static VertexBuffer BoundVertexBuffer
        {
            get => boundVBOs.Peek();
        }
        private static readonly Stack<VertexBuffer> boundVBOs = new Stack<VertexBuffer>();

        public static ElementBuffer BoundElementBuffer
        {
            get => boundEBOs.Peek();
        }
        private static readonly Stack<ElementBuffer> boundEBOs = new Stack<ElementBuffer>();

        public static VertexArray BoundVertexArray
        {
            get => boundVertexArrays.Peek();
        }
        private static readonly Stack<VertexArray> boundVertexArrays = new Stack<VertexArray>();

        public static ShaderProgram BoundShaderProgram
        {
            get => boundShaderPrograms.Peek();
        }
        private static readonly Stack<ShaderProgram> boundShaderPrograms = new Stack<ShaderProgram>();

        internal static void BindVertexBuffer(VertexBuffer buffer)
        {
            if (boundVBOs.Contains(buffer)) throw new InvalidOperationException("Attempted to bind already bound buffer. ID: " + buffer.glID);
            GL.BindBuffer(BufferTarget.ArrayBuffer, buffer.glID);
            boundVBOs.Push(buffer);
        }

        internal static void UnbindVertexBuffer()
        {
            boundVBOs.Pop();
            if (boundVBOs.Count != 0) GL.BindBuffer(BufferTarget.ArrayBuffer, boundVBOs.Peek().glID);
            else GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        internal static void BindElementBuffer(ElementBuffer buffer)
        {
            if (boundEBOs.Contains(buffer)) throw new InvalidOperationException("Attempted to bind already bound buffer. ID: " + buffer.glID);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, buffer.glID);
            boundEBOs.Push(buffer);
        }

        internal static void UnbindElementBuffer()
        {
            boundEBOs.Pop();
            if (boundEBOs.Count != 0) GL.BindBuffer(BufferTarget.ElementArrayBuffer, boundEBOs.Peek().glID);
            else GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }

        internal static void BindVertexArray(VertexArray buffer)
        {
            if (boundVertexArrays.Contains(buffer)) throw new InvalidOperationException("Attempted to bind already bound buffer. ID: " + buffer.glID);
            GL.BindVertexArray(buffer.glID);
            boundVertexArrays.Push(buffer);
        }

        internal static void UnbindVertexArray()
        {
            boundVertexArrays.Pop();
            if (boundVertexArrays.Count != 0) GL.BindVertexArray(boundVertexArrays.Peek().glID);
            else GL.BindVertexArray(0);
        }

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
