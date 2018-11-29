using System;
using OpenTK.Graphics.OpenGL;

namespace Tangerine.Extensions.OpenTK
{
    public static class VertexAttribPointerTypeExtension
    {
        /// <summary>
        /// Gets the size of a <see cref="VertexAttribPointerType"/> in bytes
        /// </summary>
        /// <param name="pointerType">The type to return the size of. Valid values are all values of <see cref="VertexAttribPointerType"/> that have an equivalent C# integral type</param>
        /// <returns></returns>
        public static int GetSize(this VertexAttribPointerType pointerType)
        {
            switch (pointerType)
            {
                case VertexAttribPointerType.Byte: return sizeof(sbyte);
                case VertexAttribPointerType.UnsignedByte: return sizeof(byte);
                case VertexAttribPointerType.Short: return sizeof(short);
                case VertexAttribPointerType.UnsignedShort: return sizeof(ushort);
                case VertexAttribPointerType.Int: return sizeof(int);
                case VertexAttribPointerType.UnsignedInt: return sizeof(uint);
                case VertexAttribPointerType.Float: return sizeof(float);
                case VertexAttribPointerType.Double: return sizeof(double);
                default:
                    //Is the value an enum constant of VertexAttribPointerType?
                    if (Enum.IsDefined(typeof(VertexAttribPointerType), pointerType))
                        throw new ArgumentOutOfRangeException("pointerType", pointerType + " is invalid as it does not correspond to a C# integral type");
                    else throw new ArgumentOutOfRangeException("pointerType", pointerType + " is invalid as it is not the value of an enum constant of VertexAttribPointerType");
            }
        }
    }
}
