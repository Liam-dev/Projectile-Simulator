using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

namespace Simulator.Simulation
{
    /// <summary>
    /// A SimulationObject that can be moved to manipulate other simulation objects.
    /// </summary>
    public class Handle : SimulationObject, IPersistent
    {
        /// <summary>
        /// The zoom of the simulation (used for drawing and selection)
        /// </summary>
        protected float zoom;

        /// <summary>
        /// Gets or sets the parent object of the handle.
        /// </summary>
        public SimulationObject Parent { get; set; }

        [JsonIgnore]
        public override Rectangle BoundingBox
        {
            get
            {
                if (texture != null)
                {
                    // Scale selection by zoom
                    return new Rectangle((int)Position.X, (int)Position.Y, (int)(texture.Width / zoom), (int)(texture.Height / zoom));
                }
                else
                {
                    return Rectangle.Empty;
                }
            }
        }

        /// <summary>
        /// Parameterless constructor for Handle.
        /// </summary>
        public Handle()
        {
        }

        /// <summary>
        /// Constructor for Handle.
        /// </summary>
        /// <param name="name">Name of object.</param>
        /// <param name="position">Position to place object.</param>
        /// <param name="textureName">Name of texture to load.</param>
        /// <param name="parent">The  parent object of the handle.</param>
        public Handle(string name, Vector2 position, string textureName, SimulationObject parent) : base(name, position, textureName)
        {
            Parent = parent;
            Selectable = true;
            Movable = true;
        }

        public override void Draw(SpriteBatch spriteBatch, float zoom)
        {
            // Scale the drawing and selection area by 1 / zoom factor
            this.zoom = zoom;
            spriteBatch.Draw(texture, Position, null, null, null, 0, new Vector2(1 / zoom), Color.White, SpriteEffects.None, 0);
        }
    }
}