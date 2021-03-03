using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Simulator.Simulation
{
    interface ISelectable
    {
        /// <summary>
        /// Gets or sets whether the object is currently selected.
        /// </summary>
        bool Selected { get; set; }

        /// <summary>
        ///  Gets or sets whether the object can be selected or not.
        /// </summary>
        bool Selectable { get; set; }

        /// <summary>
        /// Determines if the object intersects a certain point.
        /// </summary>
        /// <param name="point">Point to test intersection for</param>
        /// <returns>Result of intersection</returns>
        bool Intersects(Vector2 point);
    }
}
