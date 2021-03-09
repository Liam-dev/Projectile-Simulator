using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Simulator.Simulation
{
    /// <summary>
    /// A SimulationObject that can be moved to manipulate other SimulationObjects.
    /// </summary>
    public class Handle : SimulationObject , IPersistent
    {
        // The zoom of the simulation (used for drawing and selection)
        protected float zoom;

        public SimulationObject Parent { get; set; }

        /// <summary>
        /// Occurs when the position of the handle is changed.
        /// </summary>
        public event EventHandler PositionChanged;

        [JsonIgnore]
        public override Rectangle BoundingBox
        {
            get
            {
                if (texture != null)
                {
                    return new Rectangle((int)Position.X, (int)Position.Y, (int)(texture.Width / zoom), (int)(texture.Height / zoom));
                }
                else
                {
                    return Rectangle.Empty;
                }
            }
        }

        public Handle()
        {

        }

        public Handle(string name, Vector2 position, string textureName, SimulationObject parent) : base(name, position, textureName)
        {
            Parent = parent;
            Selectable = true;
            Movable = true;
        }

        public override void Draw(SpriteBatch spriteBatch, float zoom)
        {
            this.zoom = zoom;
            spriteBatch.Draw(texture, Position, null, null, null, 0, new Vector2(1 / zoom), Color.White, SpriteEffects.None, 0);
        }
    }
}
