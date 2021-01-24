using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Forms.Controls;
using Projectile_Simulator.Simulation;

namespace Projectile_Simulator.UserInterface
{
    /// <summary>
    /// Xna controlled simulation window
    /// </summary>
    class Simulation : MonoGameControl
    {
        protected List<SimulationObject> objects;

        public float Scale { get; set; }

        public Camera camera;

        protected override void Initialize()
        {
            base.Initialize();

            Scale = 100;

            camera = new Camera(GraphicsDevice.Viewport);

            objects = new List<SimulationObject>();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            foreach (SimulationObject obj in objects)
            {
                obj.Update(gameTime);
            }

            camera.Update(objects[0].Position);
        }

        protected override void Draw()
        {
            GraphicsDevice.Clear(Color.SkyBlue);

            Editor.spriteBatch.Begin(transformMatrix : camera.Transform);

            foreach (SimulationObject obj in objects)
            {
                obj.Draw(Editor.spriteBatch, Scale);
            }

            Editor.spriteBatch.End();
        }

        public void AddObject(SimulationObject obj)
        {
            objects.Add(obj);
        }

        public List<SimulationObject> GetObjects()
        {
            return objects;
        }
    }
}
