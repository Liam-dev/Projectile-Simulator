using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.ComponentModel;
using Simulator.Converters;

namespace Simulator.Simulation
{
    public class Cannon : SimulationObject, IPersistent
    {
        public event EventHandler<FiringArgs> Fired;

        [Browsable(false)]
        public Vector2 FiringPosition { get; set; }

        [Browsable(false)]
        public float ProjectionAngle { get; set; }

        [JsonIgnore]
        [Browsable(true)]
        [Category("Cannon")]
        [DisplayName("Projection angle")]
        public float DisplayProjectionAngle
        {
            get { return ProjectionAngle * 180 / MathF.PI; }
            set { ProjectionAngle = value * MathF.PI / 180; }
        }

        [Browsable(false)]
        [Category("Cannon")]
        [DisplayName("Projection speed")]
        public float Speed { get; set; }

        [JsonIgnore]
        [Browsable(true)]
        [Category("Cannon")]
        public float DisplaySpeed
        {
            get { return ScaleConverter.Scale(Speed, Scale, 1, true, 2); }
            set { Speed = ScaleConverter.InverseScale(value, Scale, 1); }
        }

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

        public void Fire()
        {
            Projectile projectile = new Projectile("projectile", Position + FiringPosition, Projectile.TextureName, Projectile.Mass, Projectile.RestitutionCoefficient, Projectile.Radius, Projectile.DragCoefficient);

            Vector2 impulse = projectile.Mass * Speed * new Vector2(MathF.Cos(ProjectionAngle), -MathF.Sin(ProjectionAngle));

            Fired?.Invoke(this, new FiringArgs(projectile, impulse));
        }
    }

    public class FiringArgs : EventArgs
    {
        public Projectile Projectile { get; protected set; }

        public Vector2 Impulse { get; protected set; }

        public FiringArgs(Projectile projectile, Vector2 impulse)
        {
            Projectile = projectile;
            Impulse = impulse;
        }
    }
}
