using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Collision;
using FarseerPhysics.Controllers;
using FarseerPhysics.Dynamics;
using FarseerPhysicsBaseFramework.GameEntities;
using FarseerPhysicsBaseFramework.Helpers;
using Microsoft.Xna.Framework;

namespace FPE3Sandbox.Entities
{
    class Water:IPlayable
    {
        private TexturedGameEntity texture;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        /// <param name="world"></param>
        /// <param name="position">The top left corner</param>
        /// <param name="width">The width of the container</param>
        /// <param name="height">The height of a container</param>
        public Water(Game game, World world, Vector2 position, float width, float height)
        {
            position = new Vector2(position.X + width/2,position.Y + height/2);
            texture = new TexturedGameEntity(game, position,0,"Images/water",0.3f);
            var container = new AABB(ConvertUnits.ToSimUnits(position),ConvertUnits.ToSimUnits(width),ConvertUnits.ToSimUnits(height));
            var buoyancy = new BuoyancyController(container, 1.1f, 2, 1, world.Gravity);
            world.AddController(buoyancy);
        }
        public void Draw(GameTime gameTime)
        {
            texture.Draw(gameTime);
        }

        public void Update(GameTime gameTime)
        {
        }
    }
}
