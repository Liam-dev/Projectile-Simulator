using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Simulator.Simulation
{
    public class Camera
    {
        public Matrix Transform { get; protected set; }

        public float ZoomMultiplier { get; set; }
        public int MaxZoomLevel { get; set; }
        public int MinZoomLevel { get; set; }

        protected float zoom;
        protected float oldZoom;

        public Camera()
        {
            Transform = Matrix.Identity;
            zoom = 1;
            oldZoom = 1;

            ZoomMultiplier = 1.1f;
            MaxZoomLevel = 8;
            MinZoomLevel = -16;
        }

        public void ZoomIn(Vector2 position)
        {
            oldZoom = zoom;
            zoom *= ZoomMultiplier;

            if (zoom > MathF.Pow(ZoomMultiplier, MaxZoomLevel))
            {
                zoom = MathF.Pow(ZoomMultiplier, MaxZoomLevel);
            }

            ZoomAtPoint(position, zoom / oldZoom);
        }

        public void ZoomOut(Vector2 position)
        {
            oldZoom = zoom;
            zoom /= ZoomMultiplier;
            if (zoom < MathF.Pow(ZoomMultiplier, MinZoomLevel))
            {
                zoom = MathF.Pow(ZoomMultiplier, MinZoomLevel);
            }

            ZoomAtPoint(position, zoom / oldZoom);
        }

        protected void ZoomAtPoint(Vector2 position, float scaleFactor)
        {
            Matrix toPosition = Matrix.CreateTranslation(new Vector3(-position, 0));
            Matrix scale = Matrix.CreateScale(scaleFactor);
            Matrix fromPosition = Matrix.CreateTranslation(new Vector3(position, 0));

            Transform *= toPosition;
            Transform *= scale;
            Transform *= fromPosition;
        }

        public void Pan(Vector2 translation)
        {
            Transform *= Matrix.CreateTranslation(new Vector3(translation, 0));
        }

        public float GetZoom()
        {
            Vector3 _zoom;
            Quaternion _rotation;
            Vector3 _position;
            Transform.Decompose(out _zoom, out _rotation, out _position);
            return zoom;
        }

        public Vector2 GetSimulationPostion(Vector2 position)
        {
            Matrix inverse = Matrix.Invert(Transform);
            return Vector2.Transform(position, inverse);       
        }

        public Vector2 GetActualPosition(Vector2 position)
        {
            return Vector2.Transform(position, Transform);
        }
    }
}
