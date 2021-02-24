using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Forms.Controls;
using Simulator.Simulation;

namespace Simulator.UserInterface
{
    /// <summary>
    /// Xna controlled simulation window
    /// </summary>
    class Simulation : MonoGameControl
    {
        protected List<SimulationObject> objects;

        protected Camera camera;
        protected int mouseScroll;

        protected TimeSpan previousDelta;

        // Lag constant
        protected float timeTolerance = 2f;

        // TEMP
        public Cannon cannon;

        public float Scale { get; set; }

        public Color BackgroundColour { get; set; }

        public bool Paused { get; set; }

        public bool IsObjectSelected { get; set; }

        public SimulationObject SelectedObject { get; set; }

       
        protected override void Initialize()
        {
            Scale = 100;
            BackgroundColour = Color.SkyBlue;

            camera = new Camera();
            mouseScroll = Mouse.GetState().ScrollWheelValue;
            
            objects = new List<SimulationObject>();

            base.Initialize();
        }
        
        protected override void Update(GameTime gameTime)
        {
            if (Focused)
            {
                GetInput();
            }

            if (!Paused)
            {
                // Check for a percentage discrepancy in the elapsed game time to allow for lag (such as from window adjustments)
                if (previousDelta == TimeSpan.Zero || gameTime.ElapsedGameTime.Duration() - previousDelta.Duration() < timeTolerance * previousDelta.Duration())
                {
                    Simulate(gameTime.ElapsedGameTime);
                    previousDelta = gameTime.ElapsedGameTime;
                }
                else
                {
                    // If there is lag, then use elapsed time from previous update
                    Simulate(previousDelta);
                }
            }
        }

        protected void Simulate(TimeSpan delta)
        {
            // Collisions
            CheckCollisions();

            // Update each of the objects
            foreach (SimulationObject obj in objects)
            {
                obj.Update(delta);
            }
        }

        protected override void Draw()
        {
            // Reset and clear the simulation window
            GraphicsDevice.Clear(BackgroundColour);

            // Start spriteBatch with the camera's transform matrix applied to all of the objects drawn.
            Editor.spriteBatch.Begin(transformMatrix : camera.Transform);

            // Draw each of the objects
            foreach (SimulationObject @object in objects)
            {
                @object.Draw(Editor.spriteBatch, camera.GetZoom());              
            }

            cannon.Draw(Editor.spriteBatch, camera.GetZoom());

            Editor.spriteBatch.End();

        }

        protected void GetInput()
        {
            MouseState mouseState = Mouse.GetState();

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (IsObjectSelected)
                {
                    // Move cusor to object
                    
                }
            }

            int newMouseScroll = mouseState.ScrollWheelValue;
            Vector2 mousePosition = mouseState.Position.ToVector2();

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

        public RenderTarget2D GetDrawCapture()
        {
            RenderTarget2D renderTarget = new RenderTarget2D(
                GraphicsDevice,
                GraphicsDevice.PresentationParameters.BackBufferWidth,
                GraphicsDevice.PresentationParameters.BackBufferHeight,
                false,
                GraphicsDevice.PresentationParameters.BackBufferFormat,
                DepthFormat.Depth24);

            // Set the render target
            GraphicsDevice.SetRenderTarget(renderTarget);

            GraphicsDevice.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };

            // Draw
            Draw();

            // Reset render target
            GraphicsDevice.SetRenderTarget(null);

            return renderTarget;
        }

        /// <summary>
        /// Add an object to the simulation
        /// </summary>
        /// <param name="object"></param>
        public void AddObject(SimulationObject @object)
        {
            @object.OnLoad(Editor.Content);
            objects.Add(@object);
        }

        public List<SimulationObject> GetObjects()
        {
            return objects;
        }

        public void CannonFired(object sender, EventArgs e)
        {
            if (e is FiringArgs args)
            {
                Projectile projectile = args.Projectile;
                projectile.ApplyImpulse(args.Impulse);
                AddObject(projectile);
            }
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
                            bool colliding = MathF.Abs(MathF.Pow(a.Centre.X - b.Centre.X, 2) + MathF.Pow(a.Centre.Y - b.Centre.Y, 2)) <= MathF.Pow(a.Radius + b.Radius, 2);

                            if (colliding)
                            {                                
                                float distance = MathF.Sqrt(MathF.Pow(a.Centre.X - b.Centre.X, 2) + MathF.Pow(a.Centre.Y - b.Centre.Y, 2));
                                float overlap = (a.Radius + b.Radius - distance);


                                Vector2 collisionNormal = a.Centre - b.Centre;

                                // Static
                                if (overlap > 0)
                                {
                                    a.Position += overlap * Vector2.Normalize(collisionNormal);
                                    b.Position -= overlap * Vector2.Normalize(collisionNormal);
                                }

                                // Dynamic
                                Vector2 relativeVelocity = a.GetVelocity() - b.GetVelocity();
                                Vector2 impulse = -(1 + (a.RestitutionCoefficient * b.RestitutionCoefficient))
                                    * Vector2.Dot(relativeVelocity, collisionNormal) * collisionNormal
                                    / (collisionNormal.LengthSquared() * ((1 / a.Mass) + (1 / b.Mass)));

                                a.ApplyImpulse(impulse);
                                b.ApplyImpulse(-impulse);
                            }          
                        }
                        else if (j is Box c)
                        {
                            Vector2 nearestPoint = new Vector2();
 
                            nearestPoint.X = MathF.Max(c.Position.X, MathF.Min(a.Centre.X, c.Position.X + c.Dimensions.X));
                            nearestPoint.Y = MathF.Max(c.Position.Y, MathF.Min(a.Centre.Y, c.Position.Y + c.Dimensions.Y));
                            
                            float overlap = a.Radius - (nearestPoint - a.Centre).Length();

                            if (overlap > 0)
                            {
                                Vector2 collisionNormal = nearestPoint - a.Centre;

                                // Static
                                a.Position -= overlap * Vector2.Normalize(collisionNormal);

                                // Dynamic
                                
                                Vector2 impulse = -(1 + (a.RestitutionCoefficient * c.RestitutionCoefficient))
                                    * Vector2.Dot(a.GetVelocity(), collisionNormal) * collisionNormal
                                    / (collisionNormal.LengthSquared() * (1 / a.Mass));

                                a.ApplyImpulse(impulse);
                                
                            }                           
                        }
                    }
                }
            }
        }

        #endregion
    }
}
