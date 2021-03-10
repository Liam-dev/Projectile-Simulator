using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Forms.Controls;
using Simulator.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Simulator.UserInterface
{
    /// <summary>
    /// XNA controlled simulation window.
    /// </summary>
    public class Simulation : MonoGameControl
    {
        /// <summary>
        /// List of all simulation objects to be updated and drawn.
        /// </summary>
        protected List<SimulationObject> objects;

        /// <summary>
        /// Determines if simulation objects are updated.
        /// </summary>
        protected bool paused;

        /// <summary>
        /// Timespan of previous update frame.
        /// </summary>
        protected TimeSpan previousDelta;

        /// <summary>
        /// Tolerance to how long a frame should be before the previous frame time is used.
        /// </summary>
        protected float timeTolerance = 2f;

        /// <summary>
        /// Update frame time multiplier.
        /// </summary>
        protected float speed = 1.3f;

        /// <summary>
        /// State of Mouse in the previous update frame.
        /// </summary>
        protected MouseState lastMouseState;

        /// <summary>
        /// Records if the context menu for the simulation is open
        /// </summary>
        protected bool contextMenuOpen;

        /// <summary>
        /// Records if an object is being moved by the mouse
        /// </summary>
        protected bool objectMoving;

        /// <summary>
        /// Gets the current object that is selected in the simulation.
        /// </summary>
        public ISelectable SelectedObject { get; protected set; }

        /// <summary>
        /// Gets if an object is selected in the simulation.
        /// </summary>
        public bool IsObjectSelected { get; protected set; }

        /// <summary>
        /// Occurs when the selected object in the simulation is changed.
        /// </summary>
        public event EventHandler SelectedObjectChanged;

        /// <summary>
        /// Gets or sets the Camera that the simulation is using to get the transformed view.
        /// </summary>
        public Camera Camera { get; set; }

        /// <summary>
        /// Gets or sets the length scale the simulation is using to determine how many pixels represents one metre.
        /// </summary>
        public new float Scale { get; set; }

        /// <summary>
        /// Gets or sets the colour of the simulation's background.
        /// </summary>
        public Color BackgroundColour { get; set; } = Color.SkyBlue;

        /// <summary>
        /// Gets or sets whether the simulation is paused or not.
        /// </summary>
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
                    SimulationResumed?.Invoke(this, new EventArgs());
                }
            }
        }

        /// <summary>
        /// Occurs when a SimulationObject is added to the simulation.
        /// </summary>
        public event EventHandler ObjectAdded;

        /// <summary>
        /// Occurs when the simulation has just been paused.
        /// </summary>
        public event EventHandler SimulationPaused;

        /// <summary>
        /// Occurs when the simulation has just been resumed.
        /// </summary>
        public event EventHandler SimulationResumed;

        /// <summary>
        /// Gets the transformed simulation world position of the centre of the simulation window.
        /// </summary>
        public Vector2 ScreenCentre { get { return Camera.GetSimulationPostion(new Vector2(Width / 2, Height / 2)); } }

        /// <summary>
        /// Gets the position of the mouse in the simulation window.
        /// </summary>
        public new Vector2 MousePosition { get { return Mouse.GetState().Position.ToVector2(); } }

        /// <summary>
        /// Gets if the left mouse button is pressed.
        /// </summary>
        public bool LeftMouseButtonPressed { get { return Mouse.GetState().LeftButton == ButtonState.Pressed; } }

        /// <summary>
        /// Gets if the right mouse button is pressed.
        /// </summary>
        public bool RightMouseButtonPressed { get { return Mouse.GetState().RightButton == ButtonState.Pressed; } }

        /// <summary>
        /// Gets if the middle mouse button is pressed.
        /// </summary>
        public bool MiddleMouseButtonPressed { get { return Mouse.GetState().MiddleButton == ButtonState.Pressed; } }

        /// <summary>
        /// Occurs when the mouse is scrolled.
        /// </summary>
        public event EventHandler<MouseScrollArgs> MouseScrolled;

        /// <summary>
        /// Occurs when the left mouse button has just been pressed.
        /// </summary>
        public event EventHandler LeftMouseButtonJustPressed;

        /// <summary>
        /// Occurs when the left mouse button has just been released.
        /// </summary>
        public event EventHandler LeftMouseButtonJustReleased;

        /// <summary>
        /// Occurs when the right mouse button has just been pressed.
        /// </summary>
        public event EventHandler RightMouseButtonJustPressed;

        /// <summary>
        /// Occurs when the right mouse button has just been released.
        /// </summary>
        public event EventHandler RightMouseButtonJustReleased;

        /// <summary>
        /// Occurs when the middle mouse button has just been pressed.
        /// </summary>
        public event EventHandler MiddleMouseButtonJustPressed;

        /// <summary>
        /// Occurs when the middle mouse button has just been released.
        /// </summary>
        public event EventHandler MiddleMouseButtonJustReleased;

        /// <summary>
        /// Occurs when an object has been moved.
        /// </summary>
        public event EventHandler ObjectMoved;

        protected override void Initialize()
        {
            // Set simulation scale to 100 pixels per metre
            Scale = 100;

            // Apply scale to static property of SimulationObject
            SimulationObject.Scale = Scale;


            // Assign initial mouse state
            lastMouseState = Mouse.GetState();

            // Add mouse event subscribers
            MouseScrolled += Simulation_MouseScrolled;
            LeftMouseButtonJustPressed += Simulation_LeftMouseButtonJustPressed;
            LeftMouseButtonJustReleased += Simulation_LeftMouseButtonJustReleased;
            RightMouseButtonJustPressed += Simulation_RightMouseButtonJustPressed;
            RightMouseButtonJustReleased += Simulation_RightMouseButtonJustReleased;

            MouseHover += Simulation_MouseHover;

            // Instantiate object list
            objects = new List<SimulationObject>();

            base.Initialize();
        }

        /// <summary>
        /// Loads settings for simulation from SimulationState
        /// </summary>
        /// <param name="state">State to load.</param>
        /// <param name="initialLoad">Is this the initial load of the simulation.</param>
        public void LoadState(SimulationState state, bool initialLoad = false)
        {
            // Clear any old objects
            objects.Clear();

            // Load settings
            LoadSettings(state);

            // Load objects into simulation
            foreach (object @object in state.Objects)
            {
                if (@object is SimulationObject simulationObject)
                {
                    AddObject(simulationObject);
                }
                else if (initialLoad && @object is Camera camera)
                {
                    // Load camera
                    Camera = camera;
                }
            }

            // If there is no camera, create a default one
            if (Camera.Transform == new Matrix())
            {
                Camera = new Camera(1.1f, 8, -20);
            }
        }

        /// <summary>
        /// Load settings from SimulationState.
        /// </summary>
        /// <param name="state">TState to load from.</param>
        public void LoadSettings(SimulationState state)
        {
            Paused = state.Paused;
            BackgroundColour = state.BackgroundColour;
            Projectile.GravitationalAcceleration = state.Gravity;
        }

        // Updates the simulation
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
                    Simulate(speed * gameTime.ElapsedGameTime);
                    previousDelta = gameTime.ElapsedGameTime;
                }
                else
                {
                    // If there is lag, then use elapsed time from previous update
                    Simulate(speed * previousDelta);
                }
            }
        }

        /// <summary>
        /// Updates all of the simulation objects and also checks for object collisions.
        /// </summary>
        /// <param name="delta">The time since the last simulation update.</param>
        protected void Simulate(TimeSpan delta)
        {
            // Collisions
            CheckCollisions();

            // Update each of the objects
            foreach (SimulationObject @object in objects)
            {
                @object.Update(delta);
            }
        }

        // Draws the simulation to the screen using the camera's transform.
        protected override void Draw()
        {
            // Reset and clear the simulation window
            GraphicsDevice.Clear(BackgroundColour);

            // Start spriteBatch with the camera's transform matrix applied to all of the objects drawn.
            Editor.spriteBatch.Begin(transformMatrix: Camera.Transform, sortMode: SpriteSortMode.FrontToBack);

            // Draw each of the objects
            foreach (SimulationObject @object in objects)
            {
                @object.Draw(Editor.spriteBatch, Camera.Zoom);
            }

            Editor.spriteBatch.End();
        }

        /// <summary>
        /// Loads and adds an object to the simulation.
        /// </summary>
        /// <param name="object">Object to add to simulation.</param>
        public void AddObject(SimulationObject @object)
        {
            @object.OnLoad(Editor);

            if (@object is Wall)
            {
                objects.Add(@object);
            }
            else
            {
                objects.Insert(0, @object);
            }

            if (@object is ISelectable selectable && selectable.Selectable)
            {
                SelectObject(selectable);
            }

            if (@object is IPersistent)
            {
                ObjectAdded?.Invoke(this, new EventArgs());
            }

            // Cannon test
            if (@object is Cannon cannon)
            {
                cannon.Fired -= CannonFired;
                cannon.Fired += CannonFired;
            }
        }

        /// <summary>
        /// Removes an object from the simulation.
        /// </summary>
        /// <param name="object">Object to remove from simulation.</param>
        public void RemoveObject(SimulationObject @object)
        {
            if (objects.Contains(@object))
            {
                objects.Remove(@object);
            }
        }

        /// <summary>
        /// Gets the state of the simulation.
        /// </summary>
        /// <returns>The SimulationState of the simulation.</returns>
        public SimulationState GetState()
        {
            return new SimulationState(GetObjectsToSave())
            {
                BackgroundColour = BackgroundColour,
                Gravity = Projectile.GravitationalAcceleration,
                Paused = Paused
            };
        }

        /// <summary>
        /// Gets all objects in the simulation.
        /// </summary>
        /// <returns>All objects in simulation.</returns>
        public List<SimulationObject> GetObjects()
        {
            return objects;
        }

        /// <summary>
        /// Gets all objects that are to be displayed to user.
        /// </summary>
        /// <returns>All persistent objects in the simulation.</returns>
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

        /// <summary>
        /// Gets all objects that are to be saved to a file.
        /// </summary>
        /// <returns>All persistent objects in the simulation and also the simulation's camera.</returns>
        public List<object> GetObjectsToSave()
        {
            List<object> list = new List<object>();
            list.Add(Camera);
            list.AddRange(objects);

            return list;
        }

        #region Input

        /// <summary>
        /// Enumeration of mouse scrolling direction into Up and Down.
        /// </summary>
        public enum MouseScrollDiretion
        {
            Up,
            Down
        }

        /// <summary>
        /// Event arguments for mouse scrolling.
        /// </summary>
        public class MouseScrollArgs : EventArgs
        {
            /// <summary>
            /// Direction of mouse scroll.
            /// </summary>
            public MouseScrollDiretion ScrollDiretion { get; private set; }

            public MouseScrollArgs(MouseScrollDiretion scrollDiretion)
            {
                ScrollDiretion = scrollDiretion;
            }
        }

        /// <summary>
        /// Determines the input into the simulation.
        /// </summary>
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
                MouseScrolled?.Invoke(this, new MouseScrollArgs(MouseScrollDiretion.Up));
            }
            // Mouse wheel down
            else if (newMouseScroll < lastMouseState.ScrollWheelValue)
            {
                MouseScrolled?.Invoke(this, new MouseScrollArgs(MouseScrollDiretion.Down));
            }

            // Left button hold
            if (LeftMouseButtonPressed)
            {
                if (lastMouseState.LeftButton == ButtonState.Pressed)
                {
                    if (IsObjectSelected && SelectedObject is IMovable movable && movable.Movable)
                    {
                        // Check for mouse movement

                        if (mouseState.Position != lastMouseState.Position)
                        {
                            // Move object
                            Vector2 mouseMovement = mouseState.Position.ToVector2() - lastMouseState.Position.ToVector2();

                            objectMoving = true;
                            movable.Moving = true;
                            movable.Move(mouseMovement / Camera.Zoom);
                        }
                        else
                        {
                            movable.Moving = false;
                        }
                    }
                }
            }

            // Middle button hold
            if (MiddleMouseButtonPressed && !LeftMouseButtonPressed)
            {
                if (lastMouseState.MiddleButton == ButtonState.Pressed)
                {
                    // Check for mouse movement

                    if (mouseState.Position != lastMouseState.Position)
                    {
                        // Move camera

                        Vector2 mouseMovement = mouseState.Position.ToVector2() - lastMouseState.Position.ToVector2();

                        Camera.Pan(mouseMovement);
                    }
                }
            }

            // Reset relative mouse state
            lastMouseState = mouseState;
        }

        private void Simulation_MouseScrolled(object sender, MouseScrollArgs e)
        {
            // Simulation zooming when mouse is scrolled

            Vector2 mousePosition = Mouse.GetState().Position.ToVector2();

            switch (e.ScrollDiretion)
            {
                case MouseScrollDiretion.Up:
                    Camera.ZoomIn(mousePosition);
                    break;

                case MouseScrollDiretion.Down:
                    Camera.ZoomOut(mousePosition);
                    break;
            }
        }

        private void Simulation_LeftMouseButtonJustPressed(object sender, EventArgs e)
        {
            // If context menu is open, close it
            // If context menu is closed, then check mouse click selection

            if (!contextMenuOpen)
            {
                CheckSelectionIntersection();
            }
            else
            {
                contextMenuOpen = false;
            }
        }

        private void Simulation_LeftMouseButtonJustReleased(object sender, EventArgs e)
        {
            // Cannon fire
            if (SelectedObject is Cannon cannon && !objectMoving)
            {
                cannon.Fire();
            }

            if (objectMoving)
            {
                objectMoving = false;
                ObjectMoved?.Invoke(this, new EventArgs());
            }
        }

        private void Simulation_RightMouseButtonJustPressed(object sender, EventArgs e)
        {
        }

        private void Simulation_RightMouseButtonJustReleased(object sender, EventArgs e)
        {
        }

        private void Simulation_MouseHover(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Get selected object for context menu selection.
        /// </summary>
        /// <returns>Object selected by cursor.</returns>
        public object ContextMenuOpening()
        {
            contextMenuOpen = true;

            if (CheckSelectionIntersection())
            {
                return SelectedObject;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets if the cursor is intersecting an object in the simulation and selects it if possible
        /// </summary>
        /// <returns></returns>
        protected bool CheckSelectionIntersection()
        {
            // Object selection

            if (IsObjectSelected)
            {
                if (!SelectedObject.Intersects(Camera.GetSimulationPostion(MousePosition)))
                {
                    DeselectObject();
                }
            }

            foreach (SimulationObject @object in objects)
            {
                if (@object is ISelectable selectable && selectable.Selectable)
                {
                    // Converts mouse position to simulation world position
                    Vector2 mouseSimulationPosition = Camera.GetSimulationPostion(MousePosition);

                    if (selectable.Intersects(mouseSimulationPosition))
                    {
                        SelectObject(selectable);
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Selects an object in the simulation.
        /// </summary>
        /// <param name="object">Object to select.</param>
        public void SelectObject(ISelectable @object)
        {
            if (@object != SelectedObject)
            {
                DeselectObject();

                @object.Selected = true;
                SelectedObject = @object;
                IsObjectSelected = true;
                SelectedObjectChanged?.Invoke(SelectedObject, new EventArgs());
            }
        }

        /// <summary>
        /// Deselects the simulation's selected object.
        /// </summary>
        public void DeselectObject()
        {
            if (IsObjectSelected)
            {
                SelectedObject.Selected = false;
                SelectedObject = null;
                IsObjectSelected = false;
                SelectedObjectChanged?.Invoke(null, new EventArgs());
            }
        }

        #endregion Input

        /// <summary>
        /// Gets a render of drawn simulation.
        /// </summary>
        /// <returns>Render of simulation.</returns>
        public RenderTarget2D GetDrawCapture()
        {
            // Create new render target
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

        // When a cannon is fired in the simulation
        private void CannonFired(object sender, EventArgs e)
        {
            // Check if sender is present in the simulation
            if (objects.Contains(sender))
            {
                if (e is FiringArgs args)
                {
                    // Create new projectile to fire
                    Projectile projectile = args.Projectile;

                    // Give projectile momentum
                    projectile.ApplyImpulse(args.Impulse);

                    // Add projectile to simulation
                    AddObject(projectile);
                }
            }
        }

        #region Collisions

        /// <summary>
        /// Checks for collisions between collision objects in the simulation.
        /// </summary>
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
                            // Check collisions between two projectiles

                            bool colliding = MathF.Abs(MathF.Pow(a.Centre.X - b.Centre.X, 2) + MathF.Pow(a.Centre.Y - b.Centre.Y, 2)) <= MathF.Pow(a.Radius + b.Radius, 2);

                            if (colliding)
                            {
                                ResolveProjectileToProjectileCollision(a, b);
                            }
                        }
                        else if (j is Box c)
                        {
                            // Check collisions between a projectile and a box

                            // Get nearest point on box to the centre of the projectile
                            Vector2 nearestPoint = new Vector2()
                            {
                                X = MathF.Max(c.Position.X, MathF.Min(a.Centre.X, c.Position.X + c.Dimensions.X)),
                                Y = MathF.Max(c.Position.Y, MathF.Min(a.Centre.Y, c.Position.Y + c.Dimensions.Y))
                            };

                            if ((nearestPoint - a.Centre).LengthSquared() < a.Radius * a.Radius)
                            {
                                // Colliding
                                float overlap = a.Radius - (nearestPoint - a.Centre).Length();
                                ResolveProjectileToBoxCollision(a, c, nearestPoint, overlap);
                            }
                        }
                        else if (j is Detector d)
                        {
                            // Check collisions between projectile and detector

                            // Get nearest point in detector's detection area to the centre of the projectile
                            Vector2 nearestPoint = new Vector2()
                            {
                                X = MathF.Max(d.DetectionArea.Left, MathF.Min(a.Centre.X, d.DetectionArea.Right)),
                                Y = MathF.Max(d.DetectionArea.Top, MathF.Min(a.Centre.Y, d.DetectionArea.Bottom))
                            };

                            if ((nearestPoint - a.Centre).LengthSquared() < a.Radius * a.Radius)
                            {
                                // Colliding
                                d.OnObjectEntered(a);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Resolves the static and dynamic collisions between two projectiles.
        /// </summary>
        /// <param name="a">First colliding projectile.</param>
        /// <param name="b">Second colliding projectile.</param>
        protected void ResolveProjectileToProjectileCollision(Projectile a, Projectile b)
        {
            float distance = MathF.Sqrt(MathF.Pow(a.Centre.X - b.Centre.X, 2) + MathF.Pow(a.Centre.Y - b.Centre.Y, 2));
            float overlap = (a.Radius + b.Radius - distance);

            Vector2 collisionNormal = a.Centre - b.Centre;

            // Static resolution
            a.Position += overlap * Vector2.Normalize(collisionNormal);
            b.Position -= overlap * Vector2.Normalize(collisionNormal);

            // Dynamic resolution
            Vector2 relativeVelocity = a.GetVelocity() - b.GetVelocity();

            // Get the squared geometric mean of the coefficients of restitution of the projectiles
            float restitution = a.RestitutionCoefficient * b.RestitutionCoefficient;

            // Calculate impulse
            Vector2 impulse = -(1 + restitution)
                * Vector2.Dot(relativeVelocity, collisionNormal) * collisionNormal
                / (collisionNormal.LengthSquared() * ((1 / a.Mass) + (1 / b.Mass)));

            a.ApplyImpulse(impulse);
            b.ApplyImpulse(-impulse);
        }

        /// <summary>
        /// Resolves the static and dynamic collision between a projectile and a box.
        /// </summary>
        /// <param name="p">Colliding projectile.</param>
        /// <param name="b">Colliding box.</param>
        /// <param name="collisionPoint">Point of collision.</param>
        /// <param name="overlap">Overlap of collision.</param>
        protected void ResolveProjectileToBoxCollision(Projectile p, Box b, Vector2 collisionPoint, float overlap)
        {
            Vector2 collisionNormal = collisionPoint - p.Centre;

            // Static resolution
            p.Position -= overlap * Vector2.Normalize(collisionNormal);

            // Dynamic resolution

            // Get the squared geometric mean of the coefficients of restitution of the projectile and the box
            float restitution = p.RestitutionCoefficient * b.RestitutionCoefficient;

            // Calculate impulse on projectile
            Vector2 impulse = -(1 + restitution)
                * Vector2.Dot(p.GetVelocity(), collisionNormal) * collisionNormal
                / (collisionNormal.LengthSquared() * (1 / p.Mass));

            p.ApplyImpulse(impulse);
        }

        #endregion Collisions
    }
}