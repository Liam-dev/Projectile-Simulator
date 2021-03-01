using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Microsoft.Xna.Framework;

namespace Simulator.Converters
{
    static class VectorSizeConverter
    {
        public static SizeF VectorToSize(Vector2 value)
        {
            return new SizeF(value.X, value.Y);
        }

        public static Vector2 SizeToVector(SizeF value)
        {
            return new Vector2(value.Width, value.Height);
        }
    }
}
