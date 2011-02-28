using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysicsBaseFramework.GameEntities;
using FarseerPhysicsBaseFramework.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FPE3Sandbox.Entities
{
    public struct RopeSegment
    {
        public Body Body { get; private set; }
        public float Width { get; private set; }
        public float Height { get; private set; }

        public RopeSegment(Body body, float width, float height) : this()
        {
            Body = body;
            Width = width;
            Height = height;
        }
    }
    public class Rope:IPlayable
    {
        public RopeSegment FirstEnd { get; private set; }
        public RopeSegment LastEnd { get; private set; }
        public List<RopeSegment> Segments { get; set; }
        public Color Color { get; set; }

        private Texture2D pixel;
        private SpriteBatch spriteBatch;

        private float boxWidthSimulation, boxHeightSimulation, lastSegmentWidth;


        public Rope(World world, GraphicsDevice device, SpriteBatch spriteBatch, Vector2 initialPosition, float length, Color color, Vector2 worldAnchorA) : this(world, device, spriteBatch, initialPosition, length, color)
        {
            JointFactory.CreateFixedRevoluteJoint(world, Segments.First().Body, new Vector2(-boxWidthSimulation / 2, 0), ConvertUnits.ToSimUnits(worldAnchorA));
        }

        public Rope(World world, GraphicsDevice device, SpriteBatch spriteBatch, Vector2 initialPosition, float length, Color color, Vector2 worldAnchorA, Vector2 worldAnchorB) : this(world, device, spriteBatch, initialPosition, length, color, worldAnchorA)
        {
            JointFactory.CreateFixedRevoluteJoint(world, Segments.Last().Body, new Vector2(lastSegmentWidth / 2, 0), ConvertUnits.ToSimUnits(worldAnchorB));
        }

        public Rope(World world, GraphicsDevice device, SpriteBatch spriteBatch, Vector2 initialPosition, float length, Color color)
        {
            this.spriteBatch = spriteBatch;

            Color = color;
            pixel = new Texture2D(device, 1, 1);
            pixel.SetData(new[] { Color.White });

            var type = BodyType.Dynamic;
            Segments = new List<RopeSegment>();
            float boxWidth = 30f,
                  boxHeight = 5f;

            boxWidthSimulation = ConvertUnits.ToSimUnits(boxWidth);
            boxHeightSimulation = ConvertUnits.ToSimUnits(boxHeight);

            var position = new Vector2(initialPosition.X + boxWidth/2f, initialPosition.Y);

            FirstEnd = new RopeSegment(BodyFactory.CreateRectangle(world, boxWidthSimulation, boxHeightSimulation, 10f, ConvertUnits.ToSimUnits(position)), boxWidth,boxHeight);
            FirstEnd.Body.BodyType = type;
            Segments.Add(FirstEnd);
            var nextPosition = new Vector2(FirstEnd.Body.Position.X + boxWidthSimulation, FirstEnd.Body.Position.Y);
            float totalLength = boxWidthSimulation;

            while (totalLength + boxWidthSimulation <= ConvertUnits.ToSimUnits(length))
            {
                var body = BodyFactory.CreateRectangle(world, boxWidthSimulation, boxHeightSimulation, 10f, nextPosition);
                body.BodyType = type;
                JointFactory.CreateRevoluteJoint(world, body, Segments.Last().Body, new Vector2(boxWidthSimulation / 2, 0));
                Segments.Add(new RopeSegment(body, boxWidth,boxHeight));
                nextPosition.X += boxWidthSimulation;
                totalLength += boxWidthSimulation;
            }

            lastSegmentWidth = ConvertUnits.ToSimUnits(length) - totalLength;
            if (lastSegmentWidth > 0)
            {
                nextPosition.X -= boxWidthSimulation;
                nextPosition.X += boxWidthSimulation/2 + lastSegmentWidth/2;
                LastEnd = new RopeSegment(BodyFactory.CreateRectangle(world, lastSegmentWidth, boxHeightSimulation, 10f, nextPosition),ConvertUnits.ToDisplayUnits(lastSegmentWidth),boxHeight);
                JointFactory.CreateRevoluteJoint(world, LastEnd.Body, Segments.Last().Body, new Vector2(boxWidthSimulation/2,0));
                LastEnd.Body.BodyType = type;
                totalLength += lastSegmentWidth;
                Segments.Add(LastEnd);
            } else
            {
                lastSegmentWidth = boxWidthSimulation;
                LastEnd = Segments.Last();
            }

            Debug.Assert(totalLength == ConvertUnits.ToSimUnits(length));
        }

        public void Draw(GameTime gameTime)
        {
            foreach (var ropeSegment in Segments)
            {
                var rec = new Rectangle((int)ConvertUnits.ToDisplayUnits(ropeSegment.Body.Position).X, (int)ConvertUnits.ToDisplayUnits(ropeSegment.Body.Position).Y, (int)ropeSegment.Width, (int)ropeSegment.Height);
                spriteBatch.Draw(pixel, ConvertUnits.ToDisplayUnits(ropeSegment.Body.Position), rec, Color, ropeSegment.Body.Rotation, new Vector2(rec.Width/2f,rec.Height/2f), 1f, SpriteEffects.None, 1f);
            }
        }

        public void Update(GameTime gameTime)
        {
        }
    }
}
