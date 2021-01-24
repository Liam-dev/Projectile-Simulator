using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Projectile_Simulator.Simulation
{
    public class Camera
    {
        public Matrix Transform { get; protected set; }

        protected Viewport viewport;

        protected float zoom = 1;

        public Vector2 Centre { get; set; }

        public float Zoom
        {
            get { return zoom; }
            set { zoom = value; if (zoom < 0.1f) zoom = 0.1f; if (zoom > 1.5f) zoom = 1.5f; }
        }

        public Camera(Viewport viewport)
        {
            this.viewport = viewport;
        }

        public void Update(Vector2 position)
        {
            Centre = position;
            Transform = Matrix.CreateTranslation(new Vector3(-Centre, 0)) *
                Matrix.CreateScale(Zoom) *
                Matrix.CreateTranslation(new Vector3(viewport.Width / 2, viewport.Height / 2, 0));
        }
    }
}
