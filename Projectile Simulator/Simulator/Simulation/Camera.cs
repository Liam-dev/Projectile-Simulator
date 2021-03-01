using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Simulator.Simulation
{
    public class Camera : IPersistent
    {
        public Matrix Transform { get; set; }

        public float ZoomMultiplier { get; set; }
        public int MaxZoomLevel { get; set; }
        public int MinZoomLevel { get; set; }

        public float Zoom { get; set; }
        public float OldZoom { get; set; }

        public Camera()
        {
            
        }

        public Camera(float multiplier, int maxLevel, int minLevel)
        {
            Transform = Matrix.Identity;
            Zoom = 1;
            OldZoom = 1;

            ZoomMultiplier = multiplier;
            MaxZoomLevel = maxLevel;
            MinZoomLevel = minLevel;
        }

        public void ZoomIn(Vector2 position)
        {
            OldZoom = Zoom;
            Zoom *= ZoomMultiplier;

            if (Zoom > MathF.Pow(ZoomMultiplier, MaxZoomLevel))
            {
                Zoom = MathF.Pow(ZoomMultiplier, MaxZoomLevel);
            }

            ZoomAtPoint(position, Zoom / OldZoom);
        }

        public void ZoomOut(Vector2 position)
        {
            OldZoom = Zoom;
            Zoom /= ZoomMultiplier;
            if (Zoom < MathF.Pow(ZoomMultiplier, MinZoomLevel))
            {
                Zoom = MathF.Pow(ZoomMultiplier, MinZoomLevel);
            }

            ZoomAtPoint(position, Zoom / OldZoom);
        }

        public void Pan(Vector2 translation)
        {
            Transform *= Matrix.CreateTranslation(new Vector3(translation, 0));
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

        public float GetZoom()
        {
            Vector3 _zoom;
            Quaternion _rotation;
            Vector3 _position;
            Transform.Decompose(out _zoom, out _rotation, out _position);
            return Zoom;
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
