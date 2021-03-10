using Microsoft.Xna.Framework;

namespace Simulator.Simulation
{
    /// <summary>
    /// An interface for moving selected objects.
    /// </summary>
     interface IMovable
    {
        /// <summary>
        /// Gets or sets whether the object is currently moving.
        /// </summary>
        bool Moving { get; set; }

        /// <summary>
        /// Gets or sets whether the object can be moved or not.
        /// </summary>
        bool Movable { get; set; }

        /// <summary>
        /// Moves the object by a certain displacement.
        /// </summary>
        /// <param name="displacement">Displacement to move object by</param>
        void Move(Vector2 displacement);
    }
}