using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using FarseerPhysicsBaseFramework.GameEntities;
using FarseerPhysicsBaseFramework.GameEntities.Physics;
using Microsoft.Xna.Framework;

namespace FPE3Sandbox.Entities
{
    class Landscape:IPlayable, IPlatform
    {
        private List<TexturedPhysicsEntity> parts;

        public float Width { get; private set; }

        public Landscape(Game game, World world, float screenHeight, string[] images, Category collisionCategory, float zIndex)
        {
            parts = new List<TexturedPhysicsEntity>();

            for (int i = 0; i < images.Length; i++)
            {
                var part = new TexturedPhysicsEntity(game, world, collisionCategory, new TexturedGameEntity(game, 0, images[i], zIndex), 1f, BodyType.Static);
                parts.Add(part);

                if (i == 0) // Position the first part accordingly; all of the rest will follow
                {
                    part.X = part.Center.X;
                }
                else
                {
                    part.X = part.Center.X + (parts[i - 1].Width*i);
                }
                part.Y = screenHeight - (part.Height - part.Center.Y);

                Width += part.Width;
            }
        }

        public void Draw(GameTime gameTime)
        {
            parts.ForEach(p => p.Draw(gameTime));
        }

        public void Update(GameTime gameTime)
        {
        }
    }
}
