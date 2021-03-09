using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Forms.Services;
using Simulator.Simulation;
using Simulator.Converters;

namespace Simulator.Simulation
{
    /// <summary>
    /// A PhysicsObject which is fired from a Cannon.
    /// </summary>
    public class Projectile : PhysicsObject
    {
        /// <summary>
        /// The trajectory of the projectile.
        /// </summary>
        protected Trajectory trajectory;

        /// <summary>
        /// Gets or sets the strength and direction of gravity.
        /// </summary>
        public static Vector2 GravitationalAcceleration { get; set; } = Scale * -9.8f * Vector2.UnitY;

        /// <summary>
        /// Gets or sets the drag coefficient of the projectile (quantity is dimensionless).
        /// </summary>
        [Browsable(true)]
        [DisplayName("Drag coefficient")]
        // Dimensionless constant
        public float DragCoefficient { get; set; }

        /// <summary>
        /// Gets or sets the circular radius of the projectile
        /// </summary>
        [Browsable(false)]
        public float Radius { get; set; }

        /// <summary>
        /// Gets or sets the displayed scaled radius of the object. Only to be used for display.
        /// </summary>
        [JsonIgnore]
        [Browsable(true)]
        [DisplayName("Radius")]
        public float DisplayRadius
        {
            get { return ScaleConverter.Scale(Radius, Scale, 1, true, 2); }
            set { Radius = ScaleConverter.InverseScale(value, Scale, 1); }
        }

        [JsonIgnore]
        [Browsable(false)]
        public override Rectangle BoundingBox
        {
            get { return new Rectangle((int)Position.X, (int)Position.Y, (int)(2 * Radius), (int)(2 * Radius)); }
        }

        [JsonIgnore]
        [Browsable(false)]
        public override Vector2 Centre
        {
            get { return Position + new Vector2(Radius); }
            set { Position = value - new Vector2(Radius); }
        }

        /// <summary>
        /// Parameterless constructor for Projectile.
        /// </summary>
        public Projectile()
        {

        }

        /// <summary>
        /// Constructor for Projectile
        /// </summary>
        /// <param name="name">Name of object.</param>
        /// <param name="position">Position to place object.</param>
        /// <param name="textureName">Name of texture to load.</param>
        /// <param name="mass">The mass of the object.</param>
        /// <param name="restitutionCoefficient">Coefficient of restitution of the object.</param>
        /// <param name="radius">The radius of the circular projectile.</param>
        /// <param name="dragCoefficient">The drag coefficient of the projectile.</param>
        public Projectile(string name, Vector2 position, string textureName, float mass, float restitutionCoefficient, float radius, float dragCoefficient) : base(name, position, textureName, mass, restitutionCoefficient)
        {
            Radius = radius;
            DragCoefficient = dragCoefficient;
            Selectable = false;
            Movable = false;
        }

        public override void OnLoad(MonoGameService Editor)
        {
            base.OnLoad(Editor);

            trajectory = new Trajectory(Name + "Trajectory", Position, "dot", 300, 50);
            trajectory.OnLoad(Editor);
        }

        public override void Update(TimeSpan delta)
        {
            resultantForce = Vector2.Zero;
            
            ApplyForce(CalculateWeight());
            ApplyForce(CalulateDrag());

            if (trajectory != null && Trajectory.Visible)
            {
                trajectory?.AddPoint(Centre);
            }

            base.Update(delta);
        }

        public override void Draw(SpriteBatch spriteBatch, float zoom)
        {
            spriteBatch.Draw(texture, new Rectangle((int)Position.X, (int)Position.Y, 2 * (int)Radius, 2 * (int)Radius), Color.White);

            // Trajectory drawing
            if (trajectory != null && Trajectory.Visible)
            {
                trajectory.Draw(spriteBatch, zoom);
            }   
        }

        /// <summary>
        /// Gets the drag due to air resistance of the object.
        /// </summary>
        /// <returns>Drag force vector.</returns>
        protected Vector2 CalulateDrag()
        {
            return 0.5f * MathF.PI * DragCoefficient * Mass * velocity.Length() * -velocity / Radius;
        }

        /// <summary>
        /// Gets the weight of the object.
        /// </summary>
        /// <returns>Weight vector of object.</returns>
        protected Vector2 CalculateWeight()
        {
            return Mass * GravitationalAcceleration;
        }

        /// <summary>
        /// Clears the projectile's trajectory.
        /// </summary>
        public void RemoveTrajectory()
        {
            trajectory = null;
        }
    }

}
