using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Particle_Sim_2D
{
    class Metaball
    {
        public delegate Vector3 ColorPicker(float alpha, float innerGradient);

        private const float Radius = Game1.MetaballRadius * Game1.MetaballScale;

        public Vector2 Position;
        public Vector2 Velocity;
        public Texture2D Texture;
        public Color Glow;
        public int besetzt;
        public int type;

        public void Update()
        {
            Position += Velocity;

            var viewport = Game1.Device.Viewport;
            if (Position.X > viewport.Width - Radius)
                Velocity.X = -Math.Abs(Velocity.X);
            if (Position.X < -Radius)
                Velocity.X = Math.Abs(Velocity.X);
            if (Position.Y > viewport.Height - Radius)
                Velocity.Y = -Math.Abs(Velocity.Y);
            if (Position.Y < -Radius)
                Velocity.Y = Math.Abs(Velocity.Y);

        }

        public static Texture2D GenerateTexture(int radius, ColorPicker picker)
        {
            int length = radius * 2;
            Color[] colors = new Color[length * length];

            for (int y = 0; y < length; y++)
            {
                for (int x = 0; x < length; x++)
                {
                    float distance = Vector2.Distance(Vector2.One, new Vector2(x, y) / radius);

                    float alpha = Falloff(distance);

                    float innerGradient = Falloff(distance, 0.6f, 0.8f);
                    colors[y * length + x] = new Color(picker(alpha, innerGradient));
                    colors[y * length + x].A = (byte)MathHelper.Clamp(alpha * 256f + 0.5f, 0f, 255f);
                }
            }

            Texture2D tex = new Texture2D(Game1.Device, radius * 2, radius * 2);
            tex.SetData(colors);
            return tex;
        }

        public static ColorPicker CreateTwoColorPicker(Color border, Color center)
        {
            return new ColorPicker((alpha, innerGradient) =>
                    Color.Lerp(border, center, innerGradient).ToVector3());
        }


        private static float Falloff(float distance)
        {
            return Falloff(distance, 1f, 1f);
        }

        private static float Falloff(float distance, float maxDistance, float scalingFactor)
        {
            if (distance <= maxDistance / 3)
            {
                return scalingFactor * (1 - 3 * distance * distance / (maxDistance * maxDistance));
            }
            else if (distance <= maxDistance)
            {
                float x = 1 - distance / maxDistance;
                return (3f / 2f) * scalingFactor * x * x;
            }
            else
                return 0;
        }
    }
}
