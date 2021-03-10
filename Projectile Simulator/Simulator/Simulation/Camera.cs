using Microsoft.Xna.Framework;
using System;

namespace Simulator.Simulation
{
    /// <summary>
    /// Object which encapsulates the transformation of a simulation.
    /// </summary>
    public class Camera : IPersistent
    {
        /// <summary>
        /// Gets or sets the matrix which represents the transformation of the camera.
        /// </summary>
        public Matrix Transform { get; set; }

        /// <summary>
        /// Gets or sets the factor to zoom the camera by.
        /// </summary>
        public float ZoomMultiplier { get; set; }

        /// <summary>
        /// Gets or sets the maximum zoom level of the camera.
        /// </summary>
        public int MaxZoomLevel { get; set; }

        /// <summary>
        /// Gets or sets the minimum zoom level of the camera.
        /// </summary>
        public int MinZoomLevel { get; set; }

        /// <summary>
        /// Gets or sets the current zoom of the camera.
        /// </summary>
        public float Zoom { get; set; }

        /// <summary>
        /// Gets or sets the previous zoom of the camera.
        /// </summary>
        public float OldZoom { get; set; }

        /// <summary>
        /// Parameterless constructor for Camera.
        /// </summary>
        public Camera()
        {
        }

        /// <summary>
        /// Constructor for Camera.
        /// </summary>
        /// <param name="multiplier">Zoom multiplier for camera.</param>
        /// <param name="maxZoomLevel">Maximum level for zooming.</param>
        /// <param name="minZoomLevel">Minimum level for zooming.</param>
        public Camera(float multiplier, int maxZoomLevel, int minZoomLevel)
        {
            Transform = Matrix.Identity;
            Zoom = 1;
            OldZoom = 1;

            ZoomMultiplier = multiplier;
            MaxZoomLevel = maxZoomLevel;
            MinZoomLevel = minZoomLevel;
        }

        /// <summary>
        /// Zooms the camera in on a position.
        /// </summary>
        /// <param name="position">The position to zoom in on in simulation coordinates.</param>
        public void ZoomIn(Vector2 position)
        {
            OldZoom = Zoom;
            Zoom *= ZoomMultiplier;

            // Clamp the zoom to its maximum level
            if (Zoom > MathF.Pow(ZoomMultiplier, MaxZoomLevel))
            {
                Zoom = MathF.Pow(ZoomMultiplier, MaxZoomLevel);
            }

            ZoomAtPoint(position, Zoom / OldZoom);
        }

        /// <summary>
        /// Zooms the camera out on a position.
        /// </summary>
        /// <param name="position">The position to zoom out on in simulation coordinates.</param>
        public void ZoomOut(Vector2 position)
        {
            OldZoom = Zoom;
            Zoom /= ZoomMultiplier;

            //Clamp the zoom to its minimum level
            if (Zoom < MathF.Pow(ZoomMultiplier, MinZoomLevel))
            {
                Zoom = MathF.Pow(ZoomMultiplier, MinZoomLevel);
            }

            ZoomAtPoint(position, Zoom / OldZoom);
        }

        /// <summary>
        /// Pans the camera by a translation.
        /// </summary>
        /// <param name="translation">Translation vector.</param>
        public void Pan(Vector2 translation)
        {
            Transform *= Matrix.CreateTranslation(new Vector3(translation, 0));
        }

        /// <summary>
        /// Zooms the camera at a point by a scale factor.
        /// </summary>
        /// <param name="position">The position to zoom about simulation coordinates.</param>
        /// <param name="scaleFactor">The scale factor the zoom by.</param>
        protected void ZoomAtPoint(Vector2 position, float scaleFactor)
        {
            // Multiply matrix by new zoom transformation matrices of an enlargement centred on the specified position
            Matrix toPosition = Matrix.CreateTranslation(new Vector3(-position, 0));
            Matrix scale = Matrix.CreateScale(scaleFactor);
            Matrix fromPosition = Matrix.CreateTranslation(new Vector3(position, 0));

            Transform *= toPosition;
            Transform *= scale;
            Transform *= fromPosition;
        }

        /// <summary>
        /// Gets the simulation position of a screen location.
        /// </summary>
        /// <param name="position">The screen location to convert.</param>
        /// <returns>The corresponding simulation position.</returns>
        public Vector2 GetSimulationPostion(Vector2 position)
        {
            // Transforms the given position buy the inverse of the camera's transformation matrix
            Matrix inverse = Matrix.Invert(Transform);
            return Vector2.Transform(position, inverse);
        }

        /// <summary>
        /// Gets the screen location of a simulation position.
        /// </summary>
        /// <param name="position">The simulation position to convert.</param>
        /// <returns>The corresponding screen location.</returns>
        public Vector2 GetActualPosition(Vector2 position)
        {
            //Transforms the given position buy the camera's transformation matrix
            return Vector2.Transform(position, Transform);
        }
    }
}