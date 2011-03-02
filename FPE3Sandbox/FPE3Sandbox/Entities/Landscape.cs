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
    class Landscape:IPlayable
    {
        private List<TexturedPhysicsEntity> parts;

        public Landscape(Game game, World world, float screenHeight, string[] images, Category collisionCategory, float zIndex)
        {
            parts = new List<TexturedPhysicsEntity>();

            var position = 0;
            for (int i = 0; i < images.Length; i++)
            {
                var part = new TexturedPhysicsEntity(game, world, collisionCategory, new TexturedGameEntity(game, 0, images[i], zIndex), 1f, BodyType.Static);
                parts.Add(part);
                if (i == 0)
                {
                    part.X = part.Center.X;
                    part.Y = 300;
                    continue;
                }
                part.X = parts[i - 1].X + parts[i - 1].Width/2f +part.Center.X;
                part.Y = 300;
                position += part.Width;
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
