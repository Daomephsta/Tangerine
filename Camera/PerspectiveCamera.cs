using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tangerine.GLObjects;

namespace Tangerine
{
    public class PerspectiveCamera : Camera
    {
        public float FOV
        {
            get => fov;
            set => fov = MathHelper.Clamp(value, minFov, maxFov);
        }
        private float fov, minFov = 1.0F, maxFov = 90.0F;

        public PerspectiveCamera(float defaultFOV)
        {
            FOV = defaultFOV;
        }

        public override void UpdateView(ShaderProgram shaderProgram, int width, int height)
        {
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(FOV), (float)width / height, 0.1F, 100.0F);
            shaderProgram.SetUniform("projection", projection);

            Matrix4 view = Matrix4.LookAt(Pos, Pos + Front, Up);
            shaderProgram.SetUniform("view", view);
        }

        public void SetFOVBounds(float minFov, float maxFov)
        {
            this.minFov = minFov;
            this.maxFov = maxFov;
        }
    }
}
