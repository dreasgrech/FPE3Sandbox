using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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
        /*
         * A rope can also be built with a Path (see samples).  Future work maybe?
         */

        public RopeSegment FirstEnd { get; private set; }
        public RopeSegment LastEnd { get; private set; }
        public List<RopeSegment> Segments { get; set; }
        public Color Color { get; set; }

        private Texture2D pixel;
        private SpriteBatch spriteBatch;
        List<Joint> joints = new List<Joint>();
        private float length;

        private float boxWidthSimulation, boxHeightSimulation, lastSegmentWidth;


        public Rope(World world, GraphicsDevice device, SpriteBatch spriteBatch, Vector2 initialPosition, float length, Color color, Vector2 worldAnchorA) : this(world, device, spriteBatch, initialPosition, length, color)
        {
            var jointA = JointFactory.CreateFixedRevoluteJoint(world, Segments.First().Body, new Vector2(-boxWidthSimulation / 2, 0), ConvertUnits.ToSimUnits(worldAnchorA));
            joints.Insert(0,jointA);
        }

        public Rope(World world, GraphicsDevice device, SpriteBatch spriteBatch, Vector2 initialPosition, float length, Color color, Vector2 worldAnchorA, Vector2 worldAnchorB) : this(world, device, spriteBatch, initialPosition, length, color, worldAnchorA)
        {
            var jointB = JointFactory.CreateFixedRevoluteJoint(world, Segments.Last().Body, new Vector2(lastSegmentWidth / 2, 0), ConvertUnits.ToSimUnits(worldAnchorB));
            joints.Add(jointB);
        }

        public Rope(World world, GraphicsDevice device, SpriteBatch spriteBatch, Vector2 initialPosition, float length, Color color)
        {
            this.length = length;
            this.spriteBatch = spriteBatch;

            Color = color;
            pixel = new Texture2D(device, 1, 1);
            pixel.SetData(new[] { Color.White });

            Segments = new List<RopeSegment>();
            float boxWidth = 30f,
                  boxHeight = 5f;

            boxWidthSimulation = ConvertUnits.ToSimUnits(boxWidth);
            boxHeightSimulation = ConvertUnits.ToSimUnits(boxHeight);

            FirstEnd = new RopeSegment(BodyFactory.CreateRectangle(world, boxWidthSimulation, boxHeightSimulation, 10f, ConvertUnits.ToSimUnits(new Vector2(initialPosition.X + boxWidth/2f, initialPosition.Y))), boxWidth,boxHeight);
            Segments.Add(FirstEnd);
            var nextPosition = new Vector2(FirstEnd.Body.Position.X + boxWidthSimulation, FirstEnd.Body.Position.Y);
            float totalLength = boxWidthSimulation;

            while (totalLength + boxWidthSimulation <= ConvertUnits.ToSimUnits(length))
            {
                var body = BodyFactory.CreateRectangle(world, boxWidthSimulation, boxHeightSimulation, 10f, nextPosition);
                var connectingJoint = JointFactory.CreateRevoluteJoint(world, body, Segments.Last().Body, new Vector2(boxWidthSimulation / 2, 0));
                joints.Add(connectingJoint);
                Segments.Add(new RopeSegment(body, boxWidth,boxHeight));
                nextPosition.X += boxWidthSimulation;
                totalLength += boxWidthSimulation;
            }

            if ((lastSegmentWidth = ConvertUnits.ToSimUnits(length) - totalLength) > 0)
            {
                nextPosition.X -= boxWidthSimulation;
                nextPosition.X += boxWidthSimulation/2 + lastSegmentWidth/2;
                LastEnd = new RopeSegment(BodyFactory.CreateRectangle(world, lastSegmentWidth, boxHeightSimulation, 10f, nextPosition),ConvertUnits.ToDisplayUnits(lastSegmentWidth),boxHeight);
                var connectingJoint = JointFactory.CreateRevoluteJoint(world, LastEnd.Body, Segments.Last().Body, new Vector2(boxWidthSimulation / 2, 0));
                connectingJoint.LimitEnabled = true;
                connectingJoint.LowerLimit = -MathHelper.ToRadians(90);
                //connectingJoint.UpperLimit = MathHelper.ToRadians(90);
                joints.Add(connectingJoint);
                totalLength += lastSegmentWidth;
                Segments.Add(LastEnd);

            } else
            {
                lastSegmentWidth = boxWidthSimulation;
                LastEnd = Segments.Last();
            }

            foreach (var ropeSegment in Segments)
            {
                ropeSegment.Body.BodyType = BodyType.Dynamic;
            }

            foreach (var revoluteJoint in joints)
            {
                //revoluteJoint.Breakpoint = 10000f;
            }

            Debug.Assert(totalLength == ConvertUnits.ToSimUnits(length));
        }

        public void Draw(GameTime gameTime)
        {
            foreach (var ropeSegment in Segments)
            {
                var displayPosition = ConvertUnits.ToDisplayUnits(ropeSegment.Body.Position);
                var rec = new Rectangle((int)displayPosition.X, (int)displayPosition.Y, (int)ropeSegment.Width, (int)ropeSegment.Height);
                spriteBatch.Draw(pixel, displayPosition, rec, Color, ropeSegment.Body.Rotation, new Vector2(rec.Width / 2f, rec.Height / 2f), 1f, SpriteEffects.None, 1f); // TODO: z-index is 1
            }
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Cut(float lengthFromStart)
        {
            int jointNumber = (int)Math.Floor(lengthFromStart*joints.Count/length);
            joints[jointNumber - 1].Enabled = false;
        }
    }
}
