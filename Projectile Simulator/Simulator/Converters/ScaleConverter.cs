using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using Microsoft.Xna.Framework;

namespace Simulator.Converters
{
    /// <summary>
    /// Static class to convert and round lengths between units.
    /// </summary>
    static public class ScaleConverter
    {
        /// <summary>
        /// Scales float values.
        /// </summary>
        /// <param name="value">Value to be scaled.</param>
        /// <param name="scale">Scale of conversion.</param>
        /// <param name="factor">Order of quantity.</param>
        /// <param name="round">Should scaled value be rounded</param>
        /// <param name="digits">Number of decimal places to round to.</param>
        /// <returns>Scaled float value.</returns>
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

        /// <summary>
        /// Scales float values by inverse of scale.
        /// </summary>
        /// <param name="value">Scaled value scaled.</param>
        /// <param name="scale">Scale of original conversion.</param>
        /// <param name="factor">Order of quantity.</param>
        /// <returns>Original float value</returns>
        public static float InverseScale(float value, float scale, float factor)
        {
            return value * MathF.Pow(scale, factor);
        }

        /// <summary>
        /// Scales vector values.
        /// </summary>
        /// <param name="value">Value to be scaled.</param>
        /// <param name="scale">Scale of conversion.</param>
        /// <param name="factor">Order of quantity.</param>
        /// <param name="round">Should scaled value be rounded</param>
        /// <param name="digits">Number of decimal places to round to.</param>
        /// <returns>Scaled vector value</returns>
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

        /// <summary>
        /// Scales vector values by inverse of scale.
        /// </summary>
        /// <param name="value">Scaled value scaled.</param>
        /// <param name="scale">Scale of original conversion.</param>
        /// <param name="factor">Order of quantity.</param>
        /// <returns>Original vector value</returns>
        public static Vector2 InverseScaleVector(Vector2 value, float scale, float factor)
        {
            return value * MathF.Pow(scale, factor);
        }
    }
}
