﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Simulator.Simulation;

namespace Simulator.Simulation
{
    public class Projectile : PhysicsObject
    {
        public float DragCoefficient { get; set; }
        public float Radius
        {
            get
            {
                if (texture != null)
                {
                    return texture.Width / 2;
                }
                else
                {
                    return 0;
                }              
            }
        }

        public Projectile()
        {

        }

        public Projectile(string name, Vector2 position, string textureName, float mass, float restitutionCoefficient, float dragCoefficient) : base(name, position, textureName, mass, restitutionCoefficient)
        {
            DragCoefficient = dragCoefficient;           
        }

        public override void Update(GameTime gameTime)
        {
            resultantForce = Vector2.Zero;
            resultantForce += CalculateWeight();
            resultantForce += CalulateDrag();

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position - new Vector2(Radius), Color.White);
        }

        protected Vector2 CalulateDrag()
        {
            //float area = texture.Height * texture.Width * MathF.PI * 0.25f;
            return DragCoefficient * velocity.Length() * -velocity;
        }

        protected Vector2 CalculateWeight()
        {
            return Mass * 980 * Vector2.UnitY;
        }
    }

}
