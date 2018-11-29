using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tangerine.GLObjects;

namespace Tangerine
{
    public class Camera
    {
        public Vector3 Pos
        {
            get;
            set;
        }

        public Vector3 Front
        {
            get;
            private set;
        } = new Vector3(0.0f, 0.0f, -1.0f);

        public Vector3 Right
        {
            get;
            private set;
        }

        public Vector3 Up
        {
            get;
            private set;
        } = new Vector3(0.0F, 1.0F, 0.0F);


        public double Pitch
        {
            get => pitch;
            private set => pitch = MathHelper.Clamp(value, minPitch, maxPitch);
        }
        private double pitch, minPitch = -90.0D, maxPitch = 90.0D;

        public double Yaw
        {
            get => yaw;
            private set => yaw = MathHelper.Clamp(value, minYaw, maxYaw);
        }
        private double yaw = -90.0F, minYaw = -90.0D, maxYaw = 90.0D;

        public double Roll
        {
            get => roll;
            private set => roll = MathHelper.Clamp(value, minRoll, maxRoll);
        }
        private double roll = -90.0F, minRoll = -90.0D, maxRoll = 90.0D;

        public virtual void UpdateView(ShaderProgram shaderProgram, int width, int height) {}

        public virtual void SetupView(ShaderProgram shaderProgram, int width, int height) {}

        public void Move(Vector3 xyz)
        {
            Pos += xyz;
        }

        public void Move(float x = 0.0F, float y = 0.0F, float z = 0.0F)
        {
            Pos = new Vector3(Pos.X + x, Pos.Y + y, Pos.Z + z);
        }

        public void Rotate(double deltaYaw = 0.0D, double deltaPitch = 0.0D, double deltaRoll = 0.0D)
        {
            yaw += deltaYaw;
            pitch += deltaPitch;
            roll += deltaRoll;

            Vector3d tempFront;
            tempFront.X = Math.Cos(MathHelper.DegreesToRadians(pitch)) * Math.Cos(MathHelper.DegreesToRadians(yaw));
            tempFront.Y = Math.Sin(MathHelper.DegreesToRadians(pitch));
            tempFront.Z = Math.Cos(MathHelper.DegreesToRadians(pitch)) * Math.Sin(MathHelper.DegreesToRadians(yaw));
            Front = (Vector3)Vector3d.Normalize(tempFront);
            RecalculateLocalUnitVectors();
        }

        public void SetPitchBounds(double minPitch, double maxPitch)
        {
            this.minPitch = minPitch;
            this.maxPitch = maxPitch;
        }

        public void SetYawBounds(double minYaw, double maxYaw)
        {
            this.minYaw = minYaw;
            this.maxYaw = maxYaw;
        }

        public void SetRollBounds(double minRoll, double maxRoll)
        {
            this.minRoll = minRoll;
            this.maxRoll = maxRoll;
        }

        private void RecalculateLocalUnitVectors()
        {
            Right = Vector3.Normalize(Vector3.Cross(Vector3.UnitY, Front));
            Up = Vector3.Cross(Front, Right);
        }
    }
}
