using FarseerPhysics.Dynamics;
using FarseerPhysicsBaseFramework.GameEntities;
using FarseerPhysicsBaseFramework.GameEntities.Physics;
using Microsoft.Xna.Framework;

namespace FPE3Sandbox.Entities.Platforms
{
    abstract class Platform:TexturedPhysicsEntity, IPlatform
    {
        protected Platform(Game game, World world, string image, Vector2 position, float angle, float density, BodyType type) : base(game, world, new TexturedGameEntity(game,position,angle,"Images/Platforms/" + image,1), density,type)
        {
        }
    }
}
