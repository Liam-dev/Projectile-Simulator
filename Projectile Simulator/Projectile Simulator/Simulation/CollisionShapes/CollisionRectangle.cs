using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Projectile_Simulator.Simulation.CollisionShapes
{
    class CollisionRectangle : ICollisionShape
    {
        public Rectangle Rectangle { get; }
        
        public CollisionRectangle(Rectangle rectangle)
        {
            this.Rectangle = rectangle;
        }

        public bool Colliding(ICollisionShape shape)
        {
            if (shape is CollisionRectangle collisionRectangle)
            {
                return Rectangle.Intersects(collisionRectangle.Rectangle);
            }
            else if (shape is CollisionCircle collisionCircle)
            {
                return default;
            }
            else
            {
                return default;
            }
            
        }
    }
}
