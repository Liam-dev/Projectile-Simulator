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

        private bool paused;

        protected TimeSpan previousDelta;
        // Lag constant
        protected float timeTolerance = 2f;

        protected MouseState lastMouseState;

        protected ISelectable selectedObject;

        public bool IsObjectSelected { get; private set; }

        public event EventHandler SelectedObjectChanged;

        public Camera Camera { get; set; }

        public new float Scale { get; set; }

        public Color BackgroundColour { get; set; }

        public bool Paused
        {
            get { return paused; }
            set
            {
                paused = value;
                if (value)
                {
                    SimulationPaused?.Invoke(this, new EventArgs());
                }
                else
                {
                    SimulationUnPaused?.Invoke(this, new EventArgs());
                }
            }
        }

        public event EventHandler ObjectAdded;

        public event EventHandler SimulationPaused;
        public event EventHandler SimulationUnPaused;

        public Vector2 ScreenCentre { get { return Camera.GetSimulationPostion(new Vector2(Width / 2, Height / 2)); } }

        public new Vector2 MousePosition { get { return Mouse.GetState().Position.ToVector2(); } }

        public bool LeftMouseButtonPressed { get { return Mouse.GetState().LeftButton == ButtonState.Pressed; } }

        public bool RightMouseButtonPressed { get { return Mouse.GetState().RightButton == ButtonState.Pressed; } }

        public bool MiddleMouseButtonPressed { get { return Mouse.GetState().MiddleButton == ButtonState.Pressed; } }

        public event EventHandler<MouseScrollArgs> MouseScrolled;

        public event EventHandler LeftMouseButtonJustPressed;
        public event EventHandler LeftMouseButtonJustReleased;
        
        public event EventHandler RightMouseButtonJustPressed;
        public event EventHandler RightMouseButtonJustReleased;

        public event EventHandler MiddleMouseButtonJustPressed;
        public event EventHandler MiddleMouseButtonJustReleased;

        protected override void Initialize()
        {
            Scale = 100;
            SimulationObject.Scale = Scale;
            
            BackgroundColour = Color.SkyBlue;

            if (Camera == null)
            {
                Camera = new Camera(1.1f, 8, -20);
            }
            
            lastMouseState = Mouse.GetState();

            MouseScrolled += Simulation_MouseScrolled;
            LeftMouseButtonJustPressed += Simulation_LeftMouseButtonJustPressed;
            LeftMouseButtonJustReleased += Simulation_LeftMouseButtonJustReleased;
            RightMouseButtonJustPressed += Simulation_RightMouseButtonJustPressed;
            RightMouseButtonJustReleased += Simulation_RightMouseButtonJustReleased;
            
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
            Editor.spriteBatch.Begin(transformMatrix : Camera.Transform);

            // Draw each of the objects
            foreach (SimulationObject @object in objects)
            {
                @object.Draw(Editor.spriteBatch, Camera.GetZoom());              
            }

            Editor.spriteBatch.End();

        }

        /// <summary>
        /// Add an object to the simulation
        /// </summary>
        /// <param name="object"></param>
        public void AddObject(SimulationObject @object)
        {
            @object.OnLoad(Editor);
            objects.Add(@object);
            if (@object is ISelectable selectable && selectable.Selectable)
            {
                SelectObject(selectable);
            }
            if (@object is IPersistent)
            {
                ObjectAdded?.Invoke(this, new EventArgs());
            }
        }

        public List<SimulationObject> GetObjects()
        {
            return objects;
        }

        public List<SimulationObject> GetObjectsToDisplay()
        {
            List<SimulationObject> list = new List<SimulationObject>();

            foreach (SimulationObject @object in objects)
            {
                if (@object is IPersistent)
                {
                    list.Add(@object);
                }
            }

            return list;
        }

        public List<object> GetObjectsToSave()
        {
            var list = new List<object>();
            list.Add(Camera);
            list.AddRange(objects);

            return list;
        }

        #region Input

        public enum ScrollDiretion
        {
            Up,
            Down
        }

        public class MouseScrollArgs : EventArgs
        {
            public ScrollDiretion ScrollDiretion { get; private set; }

            public MouseScrollArgs(ScrollDiretion scrollDiretion)
            {
                ScrollDiretion = scrollDiretion;
            }
        }

        protected void GetInput()
        {
            MouseState mouseState = Mouse.GetState();

            if (LeftMouseButtonPressed)
            {
                // Left button pressed

                if (mouseState.LeftButton != lastMouseState.LeftButton)
                {
                    // Left button just pressed
                    LeftMouseButtonJustPressed?.Invoke(this, new EventArgs());
                }
            }
            else
            {
                // Left button not pressed

                if (mouseState.LeftButton != lastMouseState.LeftButton)
                {
                    // Left button just released
                    LeftMouseButtonJustReleased?.Invoke(this, new EventArgs());
                }
            }

            if (RightMouseButtonPressed)
            {
                // Right button pressed

                if (mouseState.RightButton != lastMouseState.RightButton)
                {
                    // Right button just pressed
                    RightMouseButtonJustPressed?.Invoke(this, new EventArgs());
                }
            }
            else
            {
                // Right button not pressed

                if (mouseState.RightButton != lastMouseState.RightButton)
                {
                    // Right button just released
                    RightMouseButtonJustReleased?.Invoke(this, new EventArgs());
                }
            }

            if (MiddleMouseButtonPressed)
            {
                // Middle button pressed

                if (mouseState.MiddleButton != lastMouseState.MiddleButton)
                {
                    // Middle button just pressed
                    MiddleMouseButtonJustPressed?.Invoke(this, new EventArgs());
                }
            }
            else
            {
                // Middle button not pressed

                if (mouseState.MiddleButton != lastMouseState.MiddleButton)
                {
                    // Middle button just released
                    MiddleMouseButtonJustReleased?.Invoke(this, new EventArgs());
                }
            }


            // Scrolling
            int newMouseScroll = mouseState.ScrollWheelValue;

            // Mouse wheel up
            if (newMouseScroll > lastMouseState.ScrollWheelValue)
            {
                MouseScrolled?.Invoke(this, new MouseScrollArgs(ScrollDiretion.Up));
            }
            // Mouse wheel down
            else if (newMouseScroll < lastMouseState.ScrollWheelValue)
            {
                MouseScrolled?.Invoke(this, new MouseScrollArgs(ScrollDiretion.Down));
            }


            // Left button hold
            if (LeftMouseButtonPressed)
            {
                if (lastMouseState.LeftButton == ButtonState.Pressed)
                {
                    if (IsObjectSelected && selectedObject is IMovable movable && movable.Movable)
                    {
                        // Move object

                        if (mouseState.Position != lastMouseState.Position)
                        {
                            Vector2 mouseMovement = mouseState.Position.ToVector2() - lastMouseState.Position.ToVector2();

                            movable.Moving = true;
                            movable.Move(mouseMovement / Camera.GetZoom());
                        }
                        else
                        {
                            movable.Moving = false;
                        }
                    }
                }
            }


            // Middle button hold
            else if (MiddleMouseButtonPressed)
            {
                if (lastMouseState.MiddleButton == ButtonState.Pressed)
                {
                    // Move camera

                    if (mouseState.Position != lastMouseState.Position)
                    {
                        Vector2 mouseMovement = mouseState.Position.ToVector2() - lastMouseState.Position.ToVector2();

                        Camera.Pan(mouseMovement);
                    }
                }
            }
 

            // LAG LAG LAG BUG
            //SelectedObjectChanged?.Invoke(selectedObject, new EventArgs());
            // LAG LAG LAG BUG

            // Reset relative mouse state
            lastMouseState = mouseState;
        }

        private void Simulation_MouseScrolled(object sender, MouseScrollArgs e)
        {
            // Simulation zooming

            Vector2 mousePosition = Mouse.GetState().Position.ToVector2();

            switch (e.ScrollDiretion)
            {
                case ScrollDiretion.Up:
                    Camera.ZoomIn(mousePosition);
                    break;

                case ScrollDiretion.Down:
                    Camera.ZoomOut(mousePosition);
                    break;
            }
        }

        private void Simulation_LeftMouseButtonJustPressed(object sender, EventArgs e)
        {
            // Object selection

            if (IsObjectSelected)
            {
                if (!selectedObject.Intersects(Camera.GetSimulationPostion(MousePosition)))
                {
                    DeselectObject();
                }
            }

            foreach (SimulationObject @object in objects)
            {
                if (@object is ISelectable selectable)
                {
                    Vector2 mouseSimulationPosition = Camera.GetSimulationPostion(MousePosition);
                    if (selectable.Intersects(mouseSimulationPosition))
                    {
                        SelectObject(selectable);
                        break;
                    }
                }
            }
        }

        private void Simulation_LeftMouseButtonJustReleased(object sender, EventArgs e)
        {

        }

        private void Simulation_RightMouseButtonJustPressed(object sender, EventArgs e)
        {

        }

        private void Simulation_RightMouseButtonJustReleased(object sender, EventArgs e)
        {

        }

        public void SelectObject(ISelectable @object)
        {
            if (@object != selectedObject)
            {
                DeselectObject();

                @object.Selected = true;
                selectedObject = @object;
                IsObjectSelected = true;
                SelectedObjectChanged?.Invoke(selectedObject, new EventArgs());
            }   
        }

        public void DeselectObject()
        {
            if (IsObjectSelected)
            {
                
                selectedObject.Selected = false;
                selectedObject = null;
                IsObjectSelected = false;
                SelectedObjectChanged?.Invoke(null, new EventArgs());
            }
        }

        #endregion

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

        //TEMP
        public void FireAllCannons()
        {
            foreach (SimulationObject @object in objects.ToArray())
            {
                if (@object is Cannon cannon)
                {
                    cannon.Fire();
                }
            }
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
                                a.Position += overlap * Vector2.Normalize(collisionNormal);
                                b.Position -= overlap * Vector2.Normalize(collisionNormal);

                                // Dynamic

                                Vector2 relativeVelocity = a.GetVelocity() - b.GetVelocity();

                                float restitution = a.RestitutionCoefficient * b.RestitutionCoefficient;

                                Vector2 impulse = -(1 + restitution)
                                    * Vector2.Dot(relativeVelocity, collisionNormal) * collisionNormal
                                    / (collisionNormal.LengthSquared() * ((1 / a.Mass) + (1 / b.Mass)));

                                a.ApplyImpulse(impulse);
                                b.ApplyImpulse(-impulse);
                            }          
                        }
                        else if (j is Box c)
                        {
                            Vector2 nearestPoint = new Vector2()
                            {
                                X = MathF.Max(c.Position.X, MathF.Min(a.Centre.X, c.Position.X + c.Dimensions.X)),
                                Y = MathF.Max(c.Position.Y, MathF.Min(a.Centre.Y, c.Position.Y + c.Dimensions.Y))
                            };          
                            
                            float overlap = a.Radius - (nearestPoint - a.Centre).Length();

                            if (overlap > 0)
                            {
                                Vector2 collisionNormal = nearestPoint - a.Centre;

                                // Static
                                a.Position -= overlap * Vector2.Normalize(collisionNormal);

                                // Dynamic

                                float restitution = a.RestitutionCoefficient * c.RestitutionCoefficient;

                                Vector2 impulse = -(1 + restitution)
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
