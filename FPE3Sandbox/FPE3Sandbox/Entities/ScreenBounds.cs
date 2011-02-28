using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysicsBaseFramework.GameEntities;
using FarseerPhysicsBaseFramework.Helpers;
using Microsoft.Xna.Framework;

namespace FPE3Sandbox.Entities
{
    class ScreenBounds:IPlayable
    {
        private Body left, top, right;

        public ScreenBounds(World world, float screenWidth, float screenHeight)
        {
            left = CreateBarrier(world, new Vector2(0, screenHeight), new Vector2(0, -1000));
            right = CreateBarrier(world, new Vector2(screenWidth, screenHeight), new Vector2(screenWidth, -1000));
        }

        public void Draw(GameTime gameTime)
        {
        }

        public void Update(GameTime gameTime)
        {
        }

        static Body CreateBarrier(World world, Vector2 start, Vector2 end)
        {
            return BodyFactory.CreateEdge(world, ConvertUnits.ToSimUnits(start), ConvertUnits.ToSimUnits(end));
        }
    }
}
