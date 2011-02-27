using FarseerPhysics.Dynamics;
using FarseerPhysicsBaseFramework.GameEntities;
using FarseerPhysicsBaseFramework.GameEntities.Physics;
using Microsoft.Xna.Framework;

namespace FPE3Sandbox.Entities
{
    sealed class Terrain:TexturedPhysicsEntity, IPlatform
    {
        private TexturedGameEntity foreground;

        public Terrain(Game game, World world, float screenHeight): base(game, world, new TexturedGameEntity(game, 0, "Images/Terrain/terrain-rocky", 0.9f),1f,BodyType.Static)
        {
            X = Center.X;
            Y = screenHeight - (Height - Center.Y);

            foreground = new TexturedGameEntity(game, new Vector2(0, screenHeight), 0, "Images/Terrain/terrain3_foreground", 1f);
            foreground.X += foreground.Width / 2f;
            foreground.Y -= foreground.Height / 2f;
            Body.Friction = 500f;
        }

        public override void Draw(GameTime gametime)
        {
            //foreground.Draw(gametime);
            base.Draw(gametime);
        }

    }
}