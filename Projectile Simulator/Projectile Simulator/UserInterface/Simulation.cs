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
        protected RenderTarget2D renderTarget;
        protected Vector2 resolution = new Vector2(1920, 1080);
        protected List<SimulationObject> objects;

        protected override void Initialize()
        {
            base.Initialize();

            renderTarget = new RenderTarget2D(GraphicsDevice, 1920, 1080);

            objects = new List<SimulationObject>();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            foreach (SimulationObject obj in objects)
            {
                obj.Update(gameTime);
            }
        }

        protected override void Draw()
        {
            GraphicsDevice.Clear(Color.SkyBlue);

            Editor.spriteBatch.Begin();

            foreach (SimulationObject obj in objects)
            {
                obj.Draw(Editor.spriteBatch);
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
