using OpenTK.Graphics.OpenGL;
using System;

namespace Tangerine.GLObjects
{
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
}
