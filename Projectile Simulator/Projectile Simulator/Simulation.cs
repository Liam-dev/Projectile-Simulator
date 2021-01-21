﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Forms.Controls;

namespace Projectile_Simulator
{
    class Simulation : MonoGameControl
    {
        protected List<SimulationObject> objects;

        protected override void Initialize()
        {
            base.Initialize();

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
            base.Draw();

            Editor.spriteBatch.Begin();

            foreach (SimulationObject obj in objects)
            {
                Editor.spriteBatch.Draw(obj.Texture, obj.Position, Color.White);
            }

            Editor.spriteBatch.End();
        }

        public void AddObject(SimulationObject obj)
        {
            objects.Add(obj);
        }
    }
}
