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
            GL.BindBuffer(BufferTarget.ArrayBuffer, this.glID);
        }

        public void Unbind()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
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
