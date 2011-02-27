using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;

namespace FPE3Sandbox.Entities.Platforms
{
    class StaticPlatform:Platform
    {
        public StaticPlatform(Game game, World world, string image, Vector2 position, float angle) : base(game,world, image,position,angle,1f,BodyType.Static)
        {
        }
    }
}
