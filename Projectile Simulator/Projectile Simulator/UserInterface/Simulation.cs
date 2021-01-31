using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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

        protected Camera camera;
        protected int mouseScroll;

        protected override void Initialize()
        {
            base.Initialize();

            Scale = 100;

            camera = new Camera();
            mouseScroll = Mouse.GetState().ScrollWheelValue;
            
            objects = new List<SimulationObject>();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Check collisions
            CheckCollisions();

            // Update each of the objects
            foreach (SimulationObject obj in objects)
            {
                obj.Update(gameTime);
            }

            int newMouseScroll = Mouse.GetState().ScrollWheelValue;
            Vector2 mousePosition = Mouse.GetState().Position.ToVector2();

            // Mouse wheel up
            if (newMouseScroll > mouseScroll)
            {                
                camera.ZoomIn();
                camera.Update(mousePosition);
            }
            // Mouse wheel down
            else if (newMouseScroll < mouseScroll)
            {
                camera.ZoomOut();
                camera.Update(mousePosition);
            }

            // Reset relative mouse wheel value;
            mouseScroll = newMouseScroll;          
        }

        protected override void Draw()
        {
            // Reset and clear the simulation window
            GraphicsDevice.Clear(Color.SkyBlue);

            // Start spriteBatch with the camera's transform matrix applied to all of the objects drawn.
            Editor.spriteBatch.Begin(transformMatrix : camera.Transform);

            // Draw each of the objects
            foreach (SimulationObject obj in objects)
            {
                obj.Draw(Editor.spriteBatch);
            }

            Editor.spriteBatch.End();
        }

        /// <summary>
        /// Add an object to the simulation
        /// </summary>
        /// <param name="object"></param>
        public void AddObject(SimulationObject @object)
        {
            objects.Add(@object);
        }

        public List<SimulationObject> GetObjects()
        {
            List<(SimulationObject, Type)> list = new List<(SimulationObject, Type)>();

            foreach (SimulationObject @object in objects)
            {
                list.Add((@object, @object.GetType()));
            }

            //return list;

            return objects;
        }


        #region Collisions

        protected void CheckCollisions()
        {
            foreach (SimulationObject i in objects)
            {
                if (i is Projectile a)
                {
                    foreach (SimulationObject j in objects)
                    {
                        if (j is Projectile b && b != a)
                        {
                            bool colliding = MathF.Abs(MathF.Pow(a.Position.X - b.Position.X, 2) + MathF.Pow(a.Position.Y - b.Position.Y, 2)) <= MathF.Pow(a.Radius + b.Radius, 2);
                            //bool colliding = (a.Position - b.Position).LengthSquared() <= a.Radius + b.Radius;

                            if (colliding)
                            {
                                
                                float distance = MathF.Sqrt(MathF.Pow(a.Position.X - b.Position.X, 2) + MathF.Pow(a.Position.Y - b.Position.Y, 2));
                                float overlap = 0.1f * (distance - a.Radius - b.Radius);

                                
                                Vector2 collisionNormal = Vector2.Normalize(b.Position - a.Position);

                                // Static
                                a.Position += overlap * collisionNormal;
                                b.Position -= overlap * collisionNormal;

                                // Dynamic
                                Vector2 relativeVelocity = a.GetVelocity() - b.GetVelocity();
                                Vector2 impulse = -(a.RestitutionCoefficient * b.RestitutionCoefficient)
                                    * Vector2.Dot(relativeVelocity, collisionNormal) * collisionNormal
                                    / (collisionNormal.LengthSquared() * ((1 / a.Mass) + (1 / b.Mass)));

                                a.ApplyImpulse(impulse);
                                b.ApplyImpulse(-impulse);
                            }
                        }
                    }
                }
            }
        }

        #endregion
    }
}
