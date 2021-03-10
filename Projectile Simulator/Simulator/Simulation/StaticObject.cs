using Microsoft.Xna.Framework;

namespace Simulator.Simulation
{
    /// <summary>
    /// A CollisionObject that does not move.
    /// </summary>
    public class StaticObject : CollisionObject, IPersistent
    {
        /// <summary>
        /// Parameterless constructor for StaticObject.
        /// </summary>
        public StaticObject()
        {
        }

        /// <summary>
        /// Constructor for StaticObject.
        /// </summary>
        /// <param name="name">Name of object.</param>
        /// <param name="position">Position to place object.</param>
        /// <param name="textureName">Name of texture to load.</param>
        /// <param name="restitutionCoefficient">Coefficient of restitution of the object.</param>
        public StaticObject(string name, Vector2 position, string textureName, float restitutionCoefficient) : base(name, position, textureName, restitutionCoefficient)
        {
        }
    }
}