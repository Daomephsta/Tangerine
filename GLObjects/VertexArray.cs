using Tangerine.Extensions.OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tangerine.GLObjects
{
    public class VertexArray
    {
        internal readonly int glID;

        public VertexArray()
        {
            this.glID = GL.GenVertexArray();
        }

        public void Bind()
        {
            GL.BindVertexArray(this.glID);
        }

        public void Unbind()
        {
            GL.BindVertexArray(0);
        }

        public void Delete()
        {
            GL.DeleteVertexArray(this.glID);
        }

        public static void DeleteVAOS(VertexArray[] vaos)
        {
            foreach (VertexArray vao in vaos)
            {
                GL.DeleteVertexArray(vao.glID);
            }
        }
    }

    /// <summary>
    /// Defines the format GL should expect vertices to be in
    /// </summary>
    public class VertexFormat
    {
        public static readonly VertexFormat POSITION = new VertexFormat.Builder()
            .AddAttribute(VertexAttribPointerType.Float, 3).Build();
        public static readonly VertexFormat POSITION_NORMAL = new VertexFormat.Builder()
            .AddAttribute(VertexAttribPointerType.Float, 3)
            .AddAttribute(VertexAttribPointerType.Float, 3).Build();
        public static readonly VertexFormat POSITION_UV = new VertexFormat.Builder()
            .AddAttribute(VertexAttribPointerType.Float, 3)
            .AddAttribute(VertexAttribPointerType.Float, 2).Build();

        private readonly VertexAttrib[] vertexAttributes;
        private readonly int stride;

        private VertexFormat(IEnumerable<VertexAttrib> attributes)
        {
            vertexAttributes = new VertexAttrib[attributes.Count()];

            for (int attributeIndex = 0; attributeIndex < attributes.Count(); attributeIndex++)
            {
                VertexAttrib attribute = attributes.ElementAt(attributeIndex);
                vertexAttributes[attributeIndex] = attribute;
                stride += attribute.GetSizeBytes();
            }
        }

        public void ApplyFormat()
        {
            VertexAttrib? prevAttribute = null;
            for(int attributeIndex = 0; attributeIndex < vertexAttributes.Length; attributeIndex++)
            {
                VertexAttrib attribute = vertexAttributes.ElementAt(attributeIndex);
                //If prevAttribute is null, this is the first attribute and the offset is 0. Otherwise the offset is the size of the previous attribute in bytes.
                int offset = prevAttribute.HasValue ? prevAttribute.Value.GetSizeBytes() : 0;
                GL.VertexAttribPointer(attributeIndex, attribute.count, attribute.pointerType, attribute.normalised, stride, offset);
                GL.EnableVertexAttribArray(attributeIndex);
                prevAttribute = attribute;
            }
        }

        /// <summary>
        /// Creates instances of <see cref="VertexFormat"/>
        /// </summary>
        public class Builder
        {
            private readonly IList<VertexAttrib> vertexAttributes = new List<VertexAttrib>();

            public Builder AddNormalisedAttribute(VertexAttribPointerType type, int count)
            {
                vertexAttributes.Add(new VertexAttrib(type, count, true));
                return this;
            }

            public Builder AddAttribute(VertexAttribPointerType type, int count)
            {
                vertexAttributes.Add(new VertexAttrib(type, count, false));
                return this;
            }

            public VertexFormat Build()
            {
                return new VertexFormat(vertexAttributes);
            }
        }

        /// <summary>
        /// Struct representation of a GL vertex attribute
        /// </summary>
        private struct VertexAttrib
        {
            //The type of the vertex elements consumed by this vertex attribute
            public readonly VertexAttribPointerType pointerType;
            //The number of vertex elements consumed by this vertex attribute
            public readonly int count;
            public readonly bool normalised;

            public VertexAttrib(VertexAttribPointerType type, int count, bool normalised)
            {
                this.pointerType = type;
                this.count = count;
                this.normalised = normalised;
            }

            /// <summary>
            /// Returns the size in bytes of this vertex attribute
            /// </summary>
            public int GetSizeBytes()
            {
                return count * pointerType.GetSize();
            }
        }
    }

    
}
