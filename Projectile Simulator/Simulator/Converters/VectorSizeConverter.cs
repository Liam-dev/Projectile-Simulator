using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Microsoft.Xna.Framework;

namespace Simulator.Converters
{
    /// <summary>
    /// Converter between Vector2 and Size types.
    /// </summary>
    static class VectorSizeConverter
    {
        /// <summary>
        /// Converts Vector2 to Size.
        /// </summary>
        /// <param name="value">Vector to convert</param>
        /// <returns>Converted Size</returns>
        public static SizeF VectorToSize(Vector2 value)
        {
            return new SizeF(value.X, value.Y);
        }

        /// <summary>
        /// Converts Size to Vector2.
        /// </summary>
        /// <param name="value">Size to convert</param>
        /// <returns>Converted Vector2</returns>
        public static Vector2 SizeToVector(SizeF value)
        {
            return new Vector2(value.Width, value.Height);
        }
    }
}
