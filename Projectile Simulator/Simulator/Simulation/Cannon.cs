using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Simulator.Simulation
{
    public class Cannon : SimulationObject, IPersistent
    {
        public event EventHandler<FiringArgs> Fired;

        public Vector2 FiringPosition { get; set; }
        public float ProjectionAngle { get; set; }
        public float Speed { get; set; }
        public Projectile Projectile { get; set; }

        public Cannon()
        {

        }

        public Cannon(Vector2 position, string textureName, Projectile projectile) : base(position, textureName)
        {
            Projectile = projectile;

            ProjectionAngle = 0.25f * MathF.PI;
            Speed = 1000;
            FiringPosition = new Vector2(50, 50);
        }

        public void Fire()
        {
            Projectile projectile = new Projectile(Position + FiringPosition, Projectile.TextureName, Projectile.Mass, Projectile.RestitutionCoefficient, Projectile.DragCoefficient);

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
