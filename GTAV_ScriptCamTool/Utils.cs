using System;
using GTA;
using GTA.Math;
using System.Drawing;
namespace GTAV_ScriptCamTool
{
    public static class Utils
    {
        public static double ToRadians(this float val)
        {
            return (Math.PI / 180) * val;
        }

        public static Quaternion GetLookRotation(Vector3 lookPosition, Vector3 up)
        {
            OrthoNormalize(ref lookPosition, ref up);
            Vector3 right = Vector3.Cross(up, lookPosition);
            var w = Math.Sqrt(1.0f + right.X + up.Y + lookPosition.Z) * 0.5f;
            var val = 1.0f / (4.0f * w);
            var x = (up.Z - lookPosition.Y) * val;
            var y = (lookPosition.X - right.Z) * val;
            var z = (right.Y - up.X) * val;
            return new Quaternion((float)x, (float)y, (float)z, (float)w);
        }

        public static Vector3 RotationToDirection(Vector3 rotation)
        {
            double retZ = rotation.Z * 0.01745329f;
            double retX = rotation.X * 0.01745329f;
            double absX = Math.Abs(Math.Cos(retX));
            return new Vector3((float)-(Math.Sin(retZ) * absX), (float)(Math.Cos(retZ) * absX), (float)Math.Sin(retX));
        }

        public static Vector3 RightVector(this Vector3 position, Vector3 up)
        {
            position.Normalize();
            up.Normalize();
            return Vector3.Cross(position, up);
        }

        public static Vector3 LeftVector(this Vector3 position, Vector3 up)
        {
            position.Normalize();
            up.Normalize();
            return -(Vector3.Cross(position, up));
        }

        public static void OrthoNormalize(ref Vector3 normal, ref Vector3 tangent)
        {
            Vector3.Normalize(normal);
            var vec = Vector3.Multiply(normal, Vector3.Dot(tangent, normal));
            tangent = Vector3.Subtract(tangent, vec);
            tangent.Normalize();
        }


        public static SizeF GetScreenResolutionMantainRatio()
        {
            int screenw = Game.ScreenResolution.Width;
            int screenh = Game.ScreenResolution.Height;
            const float height = 1080f;
            float ratio = (float)screenw / screenh;
            var width = height * ratio;

            return new SizeF(width, height);
        }
    }
}
