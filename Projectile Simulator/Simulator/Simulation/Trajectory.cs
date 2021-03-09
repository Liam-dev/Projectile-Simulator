using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Forms.Services;
using System.Collections.Generic;

namespace Simulator.Simulation
{
    /// <summary>
    /// A SimulationObject which displays the trajectory of a projectile.
    /// </summary>
    public class Trajectory : SimulationObject
    {
        /// <summary>
        /// The points in the trajectory.
        /// </summary>
        protected List<Vector2> points = new List<Vector2>();

        /// <summary>
        /// The radius vector of the trajectory texture.
        /// </summary>
        protected Vector2 radiusVector;

        /// <summary>
        /// The number of times the draw method is called.
        /// </summary>
        protected int drawCount;

        /// <summary>
        /// Gets or sets if all trajectories are visible.
        /// </summary>
        public static bool Visible { get; set; } = true;

        /// <summary>
        /// Gets or sets the length (number of points) of the trajectory.
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// Gets or sets the interval (in number of potential points) between points.
        /// </summary>
        public int DrawInterval { get; set; }

        /// <summary>
        /// Constructor for Trajectory.
        /// </summary>
        /// <param name="name">Name of object.</param>
        /// <param name="position">Position to place object.</param>
        /// <param name="textureName">Name of texture to load.</param>
        /// <param name="length">Length of trajectory in points.</param>
        /// <param name="drawInterval">Interval (in number of potential points) between points.</param>
        public Trajectory(string name, Vector2 position, string textureName, int length, int drawInterval) : base(name, position, textureName)
        {
            Length = length;
            DrawInterval = drawInterval;
        }

        /// <summary>
        /// Adds a point to the trajectory.
        /// </summary>
        /// <param name="position">Position to add point.</param>
        public void AddPoint(Vector2 position)
        {
            if (drawCount % DrawInterval == 0)
            {
                points.Add(position);
            }
        }

        public override void Draw(SpriteBatch spriteBatch, float zoom)
        {
            drawCount++;

            if (points.Count > Length)
            {
                points.RemoveAt(0);
            }

            foreach (Vector2 point in points)
            {
                spriteBatch.Draw(texture, point - radiusVector, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.06f);
            }
        }

        public override void OnLoad(MonoGameService Editor)
        {
            base.OnLoad(Editor);

            radiusVector = new Vector2(texture.Width, texture.Height);
        }
    }
}