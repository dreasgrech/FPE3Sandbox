using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysicsBaseFramework.GameEntities;
using FarseerPhysicsBaseFramework.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FPE3Sandbox.Entities
{
    class Bridge:IPlayable
    {
        private Rope rope;
        public Bridge(Game game, World world, SpriteBatch spriteBatch, Vector2 worldAnchorA, Vector2 worldAnchorB)
        {
            rope = new Rope(game, world, spriteBatch, worldAnchorA, Vector2.Distance(worldAnchorA, worldAnchorB), game.Content.Load<Texture2D>("Images/ropeSection"));

            rope.LastSection.Position = ConvertUnits.ToSimUnits(worldAnchorB);
            FixedRevoluteJoint anchorA = new FixedRevoluteJoint(rope.FirstSection, rope.FirstSection.GetLocalPoint(worldAnchorA), rope.FirstSection.Position),
                                anchorB = new FixedRevoluteJoint(rope.LastSection,Vector2.Zero, ConvertUnits.ToSimUnits(worldAnchorB));
            world.AddJoint(anchorA);
            //world.AddJoint(anchorB);

            rope.FirstSection.BodyType = BodyType.Static;
            //rope.LastSection.BodyType = BodyType.Static;
            
        }

        public void Draw(GameTime gameTime)
        {
            rope.Draw(gameTime);
        }

        public void Update(GameTime gameTime)
        {
        }
    }
}
