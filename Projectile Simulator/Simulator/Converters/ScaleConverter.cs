using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using Microsoft.Xna.Framework;

namespace Simulator.Converters
{
    static public class ScaleConverter
    {
        public static float Scale(float value, float scale, float factor, bool round, int digits)
        {
            float scaled = value / MathF.Pow(scale, factor);
            if (round)
            {
                return MathF.Round(scaled, digits);
            }
            else
            {
                return scaled;
            }
        }
        
        public static float InverseScale(float value, float scale, float factor)
        {
            return value * MathF.Pow(scale, factor);
        }

        public static Vector2 ScaleVector(Vector2 value, float scale, float factor, bool round, int digits)
        {
            float x = value.X / MathF.Pow(scale, factor);
            float y = value.Y / MathF.Pow(scale, factor);

            if (round)
            {
                return new Vector2(MathF.Round(x, digits), MathF.Round(y, digits));
            }
            else
            {
                return new Vector2(x, y);
            }
        }

        public static Vector2 InverseScaleVector(Vector2 value, float scale, float factor)
        {
            return value * MathF.Pow(scale, factor);
        }
    }
}
