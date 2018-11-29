using OpenTK.Graphics.OpenGL;
using System;

namespace Tangerine.GLObjects
{
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
}
