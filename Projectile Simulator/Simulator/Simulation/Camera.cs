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

        protected float zoom;
        protected float oldZoom;

        protected float ZoomMultiplier;
        protected int MaxZoomLevel;
        protected int MinZoomLevel;

        protected Vector2 centre { get; set; }

        public Camera()
        {
            Transform = Matrix.Identity;
            zoom = 1;
            oldZoom = 1;

            ZoomMultiplier = 1.1f;
            MaxZoomLevel = 8;
            MinZoomLevel = -16;
        }

        public void ZoomIn()
        {
            oldZoom = zoom;
            zoom *= ZoomMultiplier;          
            if (zoom > MathF.Pow(ZoomMultiplier, MaxZoomLevel))
            {
                zoom = MathF.Pow(ZoomMultiplier, MaxZoomLevel);
            }
        }

        public void ZoomOut()
        {
            oldZoom = zoom;
            zoom /= ZoomMultiplier;
            if (zoom < MathF.Pow(ZoomMultiplier, MinZoomLevel))
            {
                zoom = MathF.Pow(ZoomMultiplier, MinZoomLevel);
            }
        }

        public void ZoomUpdate(Vector2 position)
        {
            Matrix toPosition = Matrix.CreateTranslation(new Vector3(-position, 0));
            Matrix scale = Matrix.CreateScale(zoom / oldZoom);
            Matrix fromPosition = Matrix.CreateTranslation(new Vector3(position, 0));

            Transform *= toPosition;
            Transform *= scale;
            Transform *= fromPosition;
        }

        public void Translate(Vector2 displacement)
        {
            Transform *= Matrix.CreateTranslation(new Vector3(displacement, 0));
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
