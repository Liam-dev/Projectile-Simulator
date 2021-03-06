﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Forms.Services;
using Newtonsoft.Json;
using Simulator.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Simulator.Simulation
{
    /// <summary>
    /// A SimulationObject which is used to fire projectiles.
    /// </summary>
    public class Cannon : SimulationObject, IPersistent, ITrigger
    {
        /// <summary>
        /// List of projectiles that the cannon has fired.
        /// </summary>
        protected List<Projectile> projectiles = new List<Projectile>();

        /// <summary>
        /// The offset of firing position from position of cannon.
        /// </summary>
        protected Vector2 firingPosition = new Vector2(45, 65);

        /// <summary>
        /// The offset position to rotate around.
        /// </summary>
        protected Vector2 rotationCentre = new Vector2(66, 95);

        /// <summary>
        /// An enumeration of the horizontal direction a cannon is facing.
        /// </summary>
        public enum FacingDirection
        {
            Right = 1,
            Left = -1
        }

        /// <summary>
        /// Gets or sets the horizontal direction the cannon is facing.
        /// </summary>
        [Browsable(true)]
        [Category("Cannon")]
        [DisplayName("Facing direction")]
        public FacingDirection Facing { get; set; }

        /// <summary>
        /// Gets or sets the projectile angle of the cannon (in radians).
        /// </summary>
        [Browsable(false)]
        public float ProjectionAngle { get; set; }

        /// <summary>
        /// Gets or sets the displayed projectile angle of the object (in degrees). Only to be used for display.
        /// </summary>
        [JsonIgnore]
        [Browsable(true)]
        [Category("Cannon")]
        [DisplayName("Projection angle")]
        public float DisplayProjectionAngle
        {
            get { return ProjectionAngle * 180 / MathF.PI; }
            set { ProjectionAngle = value * MathF.PI / 180; }
        }

        /// <summary>
        /// Gets or sets the projectile speed of the cannon.
        /// </summary>
        [Browsable(false)]
        [Category("Cannon")]
        public float Speed { get; set; }

        /// <summary>
        /// Gets or sets the displayed scaled projectile speed of the object. Only to be used for display.
        /// </summary>
        [JsonIgnore]
        [Browsable(true)]
        [Category("Cannon")]
        [DisplayName("Projection speed")]
        public float DisplaySpeed
        {
            get { return ScaleConverter.Scale(Speed, Scale, 1, true, 2); }
            set { Speed = ScaleConverter.InverseScale(value, Scale, 1); }
        }

        /// <summary>
        /// Gets or sets the projectile that the cannon fires.
        /// </summary>
        [Browsable(true)]
        [Category("Cannon")]
        public Projectile Projectile { get; set; }

        /// <summary>
        /// Occurs when the cannon is fired.
        /// </summary>
        public event EventHandler<FiringArgs> Fired;

        public event EventHandler Triggered;

        /// <summary>
        /// Parameterless constructor for Cannon.
        /// </summary>
        public Cannon()
        {
        }

        /// <summary>
        /// Constructor for Cannon.
        /// </summary>
        /// <param name="name">Name of object.</param>
        /// <param name="position">Position to place object.</param>
        /// <param name="textureName">Name of texture to load.</param>
        /// <param name="projectile">The projectile the cannon will fire.</param>
        public Cannon(string name, Vector2 position, string textureName, Projectile projectile) : base(name, position, textureName)
        {
            Projectile = projectile;

            Facing = FacingDirection.Right;
            ProjectionAngle = 0.25f * MathF.PI;
            Speed = 1000;
        }

        /// <summary>
        /// Fires the cannon's projectile from the cannon and its projection speed and projectile angle.
        /// </summary>
        public void Fire()
        {
            // Clear other trajectories
            foreach (Projectile p in projectiles)
            {
                p.RemoveTrajectory();
            }

            // Create a copy of the cannon's projectile
            Projectile projectile = new Projectile("projectile", Position, Projectile.TextureName, Projectile.Mass, Projectile.RestitutionCoefficient, Projectile.Radius, Projectile.DragCoefficient);
            
            // Determine the transformed position to fire the projectile from.
            projectile.Centre = DetermineFiringPosition();
            projectiles.Add(projectile);

            // Takes into account facing direction
            Vector2 impulse = projectile.Mass * Speed * new Vector2((int)Facing * MathF.Cos(ProjectionAngle), -MathF.Sin(ProjectionAngle));

            // Invoke events
            Fired?.Invoke(this, new FiringArgs(projectile, impulse));
            Triggered?.Invoke(this, new EventArgs());
        }

        public override void Draw(SpriteBatch spriteBatch, float zoom)
        {
            // Reflect the texture in the y axis (horizontally) if facing left
            if (Facing == FacingDirection.Left)
            {
                spriteBatch.Draw(texture, Position + new Vector2(66, 95), null, Color.White, ProjectionAngle + 1.75f * MathF.PI, rotationCentre, 1, SpriteEffects.FlipHorizontally, 0.07f);
            }
            else
            {
                spriteBatch.Draw(texture, Position + new Vector2(66, 95), null, Color.White, -ProjectionAngle + 0.25f * MathF.PI, rotationCentre, 1, SpriteEffects.None, 0.07f);
            }
        }

        /// <summary>
        /// Gets the transformed position of the cannon's firing position
        /// </summary>
        /// <returns>Global position of transformed firing position</returns>
        protected Vector2 DetermineFiringPosition()
        {
            // Create matrix which represents the transformation of a point in the cannon by a rotation around the cannon's rotation centre
            Matrix transform = Matrix.Identity;
            transform *= Matrix.CreateTranslation(new Vector3(-(Position + rotationCentre), 0));
            transform *= Matrix.CreateRotationZ(-ProjectionAngle);
            transform *= Matrix.CreateTranslation(new Vector3(Position + rotationCentre, 0));

            // Transform the global firing position by the matrix
            Vector2 transformedPosition = Vector2.Transform(Position + firingPosition, transform);

            if (Facing == FacingDirection.Right)
            {
                // If cannon facing right (orientation of texture), then do not transform position further
                return transformedPosition;
            }
            else
            {
                // If cannon facing left, then  reflect the transformed firing position in the vertical midpoint of the texture
                float reflectedX = BoundingBox.Right - (transformedPosition.X - BoundingBox.Left);
                return new Vector2(reflectedX, transformedPosition.Y);
            }
        }
    }

    /// <summary>
    /// Event arguments for cannon firing.
    /// </summary>
    public class FiringArgs : EventArgs
    {
        /// <summary>
        /// Gets the projectile that is fired.
        /// </summary>
        public Projectile Projectile { get; protected set; }

        /// <summary>
        /// Gets the impulse to be applied the projectile.
        /// </summary>
        public Vector2 Impulse { get; protected set; }

        /// <summary>
        /// Constructor for FiringArgs.
        /// </summary>
        /// <param name="projectile">Projectile that is being fired.</param>
        /// <param name="impulse">Impulse to apply to projectile when fired.</param>
        public FiringArgs(Projectile projectile, Vector2 impulse)
        {
            Projectile = projectile;
            Impulse = impulse;
        }
    }
}