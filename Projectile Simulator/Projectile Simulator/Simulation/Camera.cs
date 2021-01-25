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
        protected float oldZoom = 1;

        protected float zoomMultiplier = 1.05f;
        protected int maxZoomLevel = 20;
        protected int minZoomLevel = -5;

        protected Vector2 centre { get; set; }

        public float Zoom
        {
            get { return zoom; }
            set
            {
                oldZoom = zoom;
                zoom = value;
                if (zoom < MathF.Pow(zoomMultiplier, minZoomLevel)) zoom = MathF.Pow(zoomMultiplier, minZoomLevel);
                if (zoom > MathF.Pow(zoomMultiplier, maxZoomLevel)) zoom = MathF.Pow(zoomMultiplier, maxZoomLevel);
            }
        }

        public Camera(Viewport viewport)
        {
            this.viewport = viewport;
        }

        public void ZoomIn()
        {
            Zoom *= zoomMultiplier;
        }

        public void ZoomOut()
        {
            Zoom /= zoomMultiplier;
        }


        public void Focus(Vector2 position)
        {
            centre = position + (oldZoom / zoom) * (centre - position);
        }

        public void Update()
        {
            Transform = Matrix.CreateTranslation(new Vector3(-centre, 0)) * Matrix.CreateScale(zoom);                      
        }
    }
}
