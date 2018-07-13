using OpenTK.Graphics.OpenGL;
using System;
using System.IO;
using System.Text;

namespace Tangerine
{
    public static class ResourceManager
    {
        public static Shader CreateShaderFromFile(ShaderType type, String shaderPath)
        {
            StringBuilder source = new StringBuilder();

            using (StreamReader reader = new StreamReader(Path.Combine(Environment.CurrentDirectory, shaderPath + ".glsl")))
            {
                while (reader.Peek() != -1)
                {
                    source.AppendLine(reader.ReadLine());
                }
            }
            return new Shader(type, shaderPath, source.ToString());
        }
    }
}
