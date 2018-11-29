using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tangerine.GLObjects;

namespace Tangerine
{
    public class OrthographicCamera : Camera
    {
        public override void SetupView(ShaderProgram shaderProgram, int width, int height)
        {
            shaderProgram.SetUniform("projection", Matrix4.CreateOrthographic(width, height, 0.1F, 100.0F));
            shaderProgram.SetUniform("view", Matrix4.CreateTranslation(0.0F, 0.0F, 0.0F));
            shaderProgram.SetUniform("model", Matrix4.CreateTranslation(0.0F, 0.0F, 0.0F));
        }
    }
}
