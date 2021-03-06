using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Forms.Services;

namespace Simulator.Simulation
{
    public class Trajectory : SimulationObject
    {
        public int Length { get; set; }

        public int DrawInterval { get; set; }

        protected List<Vector2> points = new List<Vector2>();

        protected Vector2 radiusVector;

        protected int drawCount;

        public Trajectory(string name, Vector2 position, string textureName, int length, int drawInterval) : base(name, position, textureName)
        {
            Length = length;
            DrawInterval = drawInterval;
        }

        public void AddPoint(Vector2 position)
        {
            if (drawCount % DrawInterval == 0)
            {
                points.Add(position);
            }   
        }

        public override void Draw(SpriteBatch spriteBatch, float zoom)
        {
            drawCount++;

            if (points.Count > Length)
            {
                points.RemoveAt(0);
            }

            foreach (Vector2 point in points)
            {
                spriteBatch.Draw(texture, point - radiusVector, Color.White);
            }
        }

        public override void OnLoad(MonoGameService Editor)
        {
            base.OnLoad(Editor);

            radiusVector = new Vector2(texture.Width, texture.Height);
        }
    }
}
