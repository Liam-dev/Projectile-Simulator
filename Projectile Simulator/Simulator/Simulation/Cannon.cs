using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.ComponentModel;
using Simulator.Converters;
using MonoGame.Forms.Services;

namespace Simulator.Simulation
{
    /// <summary>
    /// A SimulationObject which is used to fire projectiles.
    /// </summary>
    public class Cannon : SimulationObject, IPersistent, ITrigger
    {
        protected List<Projectile> projectiles = new List<Projectile>();

        // The offset of firing position from position of cannon
        protected Vector2 firingPosition = new Vector2(40, 80);

        // The offset position to rotate around
        protected Vector2 rotationCentre = new Vector2(66, 95);

        public enum FacingDirection
        {
            Right = 1,
            Left = -1
        }

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

        public Cannon()
        {

        }

        public Cannon(string name, Vector2 position, string textureName, Projectile projectile) : base(name, position, textureName)
        {
            Projectile = projectile;

            Facing = FacingDirection.Right;
            ProjectionAngle = 0.25f * MathF.PI;
            Speed = 1000;
        }

        public override void OnLoad(MonoGameService Editor)
        {
            base.OnLoad(Editor);
            firingPosition = new Vector2(texture.Width, texture.Height) / 2;
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

            Projectile projectile = new Projectile("projectile", Position, Projectile.TextureName, Projectile.Mass, Projectile.RestitutionCoefficient, Projectile.Radius, Projectile.DragCoefficient);
            projectile.Centre = DetermineFiringPosition();
            projectiles.Add(projectile);

            // Takes into account facing direction
            Vector2 impulse = projectile.Mass * Speed * new Vector2((int)Facing * MathF.Cos(ProjectionAngle), -MathF.Sin(ProjectionAngle));

            Fired?.Invoke(this, new FiringArgs(projectile, impulse));
            Triggered?.Invoke(this, new EventArgs());
        }

        public override void Draw(SpriteBatch spriteBatch, float zoom)
        {
            // Flip if facing left
            if (Facing == FacingDirection.Left)
            {
                spriteBatch.Draw(texture, Position + new Vector2(66, 95), null, Color.White, ProjectionAngle + 1.75f * MathF.PI, rotationCentre, 1, SpriteEffects.FlipHorizontally, 0.07f);
            }
            else
            {
                spriteBatch.Draw(texture, Position + new Vector2(66, 95), null, Color.White, -ProjectionAngle + 0.25f * MathF.PI, rotationCentre, 1, SpriteEffects.None, 0.07f);
            }
            
            if (Selected)
            {
                //DrawBorder(spriteBatch, zoom, BoundingBox, 4);
            }
        }

        // Gets the transformed position of the cannon's firing position
        protected Vector2 DetermineFiringPosition()
        {
            Matrix transform = Matrix.Identity;
            transform *= Matrix.CreateTranslation(new Vector3(-(Position + rotationCentre), 1));
            transform *= Matrix.CreateRotationZ(-ProjectionAngle);
            transform *= Matrix.CreateTranslation(new Vector3(Position + rotationCentre, 1));

            Vector2 transformedPosition = Vector2.Transform(Position + firingPosition, transform);

            if (Facing == FacingDirection.Right)
            {
                return transformedPosition;
            }
            else
            {
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

        public FiringArgs(Projectile projectile, Vector2 impulse)
        {
            Projectile = projectile;
            Impulse = impulse;
        }
    }
}
