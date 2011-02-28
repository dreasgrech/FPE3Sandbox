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
        private List<Body> bridgeBodies;
        private SpriteBatch spriteBatch;
        private Texture2D texture;
        private List<RevoluteJoint> revolutes;

        public Bridge(Game game, World world, SpriteBatch spriteBatch, Vector2 worldAnchorA, Vector2 worldAnchorB)
        {
            this.spriteBatch = spriteBatch;
            bridgeBodies = new List<Body>();
            texture = game.Content.Load<Texture2D>("Images/ropeSection");
            var distance = Vector2.Distance(worldAnchorA, worldAnchorB);

            rope = new Rope(world, worldAnchorA, distance,worldAnchorA,worldAnchorB);

            foreach (var segment in rope.Segments)
            {
                segment.CollisionCategories = Category.Cat9;
                segment.CollidesWith = ~CollisionCategoriesSettings.Terrain;// | ~Category.Cat9;
            }
            return;

            var slack = 0f;
            float boxWidth = 2f, boxHeight = 16;
            Vertices box = PolygonTools.CreateRectangle(ConvertUnits.ToSimUnits(boxWidth), ConvertUnits.ToSimUnits(boxHeight)); // rope block

            Path bridgePath = new Path();
            bridgePath.Add(ConvertUnits.ToSimUnits(new Vector2(worldAnchorA.X + boxHeight / 2f, worldAnchorA.Y)));
            bridgePath.Add(ConvertUnits.ToSimUnits(new Vector2(worldAnchorB.X + boxHeight / 2f, worldAnchorB.Y)));
            bridgePath.Closed = false;

            //Vertices box = PolygonTools.CreateRectangle(ConvertUnits.ToSimUnits(texture.Width/2), ConvertUnits.ToSimUnits(texture.Height/2));

            PolygonShape shape = new PolygonShape(box, 20);
            int bodiesNeeded =(int)(Math.Ceiling(distance / boxHeight));
            bridgeBodies = PathManager.EvenlyDistributeShapesAlongPath(world, bridgePath, shape, BodyType.Dynamic, bodiesNeeded);

            foreach (var bridgeBody in bridgeBodies)
            {
                bridgeBody.CollisionCategories = Category.Cat9;
                bridgeBody.CollidesWith = ~CollisionCategoriesSettings.Terrain;// | ~Category.Cat9;
            }

            //bridgeBodies[0].Position = ConvertUnits.ToSimUnits(worldAnchorA);
            //bridgeBodies[bridgeBodies.Count - 1].Position = ConvertUnits.ToSimUnits(worldAnchorB);

            //Attach the first and last fixtures to the world
            JointFactory.CreateFixedRevoluteJoint(world, bridgeBodies[0], new Vector2(0f, -ConvertUnits.ToSimUnits(boxHeight)), ConvertUnits.ToSimUnits(worldAnchorA));
            JointFactory.CreateFixedRevoluteJoint(world, bridgeBodies[bridgeBodies.Count - 1], new Vector2(0, ConvertUnits.ToSimUnits(boxHeight)), ConvertUnits.ToSimUnits(worldAnchorB));

            revolutes = PathManager.AttachBodiesWithRevoluteJoint(world, bridgeBodies, new Vector2(0f, -ConvertUnits.ToSimUnits(boxHeight)), new Vector2(0f, ConvertUnits.ToSimUnits(boxHeight)), false, true);
        }

        public void Draw(GameTime gameTime)
        {
            //rope.Draw(gameTime);
            foreach (var body in rope.Segments)
            {
                spriteBatch.Draw(texture,ConvertUnits.ToDisplayUnits(body.Position),null,Color.White, body.Rotation, new Vector2(texture.Width/2f,texture.Height/2f), 1f,SpriteEffects.None,1f);
            }
        }

        public void Update(GameTime gameTime)
        {
        }
    }
}
