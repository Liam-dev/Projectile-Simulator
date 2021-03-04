using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.ComponentModel;
using Simulator.Converters;

namespace Simulator.Simulation
{
    /// <summary>
    /// A SimulationObject which is used to fire projectiles.
    /// </summary>
    public class Cannon : SimulationObject, IPersistent, ITrigger
    {
        /// <summary>
        /// Occurs when the cannon is fired.
        /// </summary>
        public event EventHandler<FiringArgs> Fired;

        public event EventHandler Triggered;

        /// <summary>
        /// Gets or sets the offset of firing position from position of cannon.
        /// </summary>
        [Browsable(false)]
        public Vector2 FiringPosition { get; set; }

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

        public Cannon()
        {

        }

        public Cannon(string name, Vector2 position, string textureName, Projectile projectile) : base(name, position, textureName)
        {
            Projectile = projectile;

            ProjectionAngle = 0.25f * MathF.PI;
            Speed = 1000;
            FiringPosition = new Vector2(50, 50);
        }

        /// <summary>
        /// Fires the cannon's projectile from the cannon and its projection speed and projectile angle.
        /// </summary>
        public void Fire()
        {
            Projectile projectile = new Projectile("projectile", Position + FiringPosition, Projectile.TextureName, Projectile.Mass, Projectile.RestitutionCoefficient, Projectile.Radius, Projectile.DragCoefficient);

            Vector2 impulse = projectile.Mass * Speed * new Vector2(MathF.Cos(ProjectionAngle), -MathF.Sin(ProjectionAngle));

            Fired?.Invoke(this, new FiringArgs(projectile, impulse));
            Triggered?.Invoke(this, new EventArgs());
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
