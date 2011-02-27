using System.Collections.Generic;
using System.Linq;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using FarseerPhysicsBaseFramework.GameEntities;
using FarseerPhysicsBaseFramework.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FPE3Sandbox.Entities
{
    class Rope:IPlayable
    {
        public Texture2D RopeTexture { get; set; }

        private SpriteBatch spriteBatch;

        private List<Body> chainLinks;

        public Body FirstSection
        {
            get
            {
                return chainLinks.First();
            }
        }

        public Body LastSection
        {
            get
            {
                return chainLinks.Last();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        /// <param name="world"></param>
        /// <param name="firstLinkPosition">The position of the first end of the rope in display units (pixels)</param>
        public Rope(Game game, World world, SpriteBatch spriteBatch, Vector2 firstLinkPosition, float length, Texture2D ropeTexture)
        {
            this.spriteBatch = spriteBatch;
            RopeTexture = ropeTexture;

            float chainWidth = ropeTexture.Width / 2f, // Why do I need to divide the size of the texture for FPE to build them with the correct size?
                chainHeight = ropeTexture.Height / 2f;

            var pathVertices = new[] { firstLinkPosition, new Vector2(firstLinkPosition.X + length, firstLinkPosition.Y) }.Select(ConvertUnits.ToSimUnits).ToList();
            Path path = new Path(pathVertices);

            Vertices rec = PolygonTools.CreateRectangle(ConvertUnits.ToSimUnits(chainWidth), ConvertUnits.ToSimUnits(chainHeight));

            PolygonShape shape = new PolygonShape(rec, 20);

            //var ropeLength = Vector2.Distance(pathVertices[0], pathVertices[1]);

            int neededBodies = (int)(ConvertUnits.ToSimUnits(length) / ConvertUnits.ToSimUnits(chainHeight)) / 2;
            chainLinks = PathManager.EvenlyDistributeShapesAlongPath(world, path, shape, BodyType.Dynamic, neededBodies);

            foreach (Body chainLink in chainLinks)
            {
                foreach (Fixture f in chainLink.FixtureList)
                {
                    f.Friction = 0.02f;
                }
            }

            List<RevoluteJoint> joints = PathManager.AttachBodiesWithRevoluteJoint(world, chainLinks,
                                                                                   ConvertUnits.ToSimUnits(0, -chainHeight),
                                                                                   ConvertUnits.ToSimUnits(0, chainHeight),
                                                                                   false, false);
        }

        public void Draw(GameTime gameTime)
        {
            foreach (var chainLink in chainLinks)
            {
                spriteBatch.Draw(RopeTexture, ConvertUnits.ToDisplayUnits(chainLink.Position), null, Color.White, chainLink.Rotation, new Vector2(RopeTexture.Width / 2f, RopeTexture.Height / 2f), 1f, SpriteEffects.None, 1f);
            }
        }

        public void Update(GameTime gameTime)
        {
        }
    }
}
