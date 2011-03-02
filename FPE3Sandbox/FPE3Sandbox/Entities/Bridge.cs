using System;
using System.Collections.Generic;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using FarseerPhysicsBaseFramework.GameEntities;
using FarseerPhysicsBaseFramework.Helpers;
using FPE3Sandbox.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FPE3Sandbox.Entities
{
    class Bridge:IPlayable
    {
        private Rope rope;

        public float Length { get; private set; }

        public Bridge(World world, SpriteBatch spriteBatch, GraphicsDevice device, Vector2 worldAnchorA, Vector2 worldAnchorB, Color ropeColor)
        {
            Length = Vector2.Distance(worldAnchorA, worldAnchorB);

            rope = new Rope(world, device, spriteBatch, worldAnchorA, Length, ropeColor, worldAnchorA, worldAnchorB);

            foreach (var segment in rope.Segments)
            {
                segment.Body.CollisionCategories = Category.Cat9;
                segment.Body.CollidesWith = ~CollisionCategoriesSettings.Terrain;
            }

        }

        public void Draw(GameTime gameTime)
        {
            rope.Draw(gameTime);
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Break(float lengthFromStart)
        {
            rope.Cut(lengthFromStart);
        }
    }
}
