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

namespace FPE3Sandbox.Entities
{
    public class Rope:IPlayable
    {
        public Body FirstEnd { get; private set; }
        public Body LastEnd { get; private set; }
        public List<Body> Segments { get; set; }

        private float boxWidthSimulation, boxHeightSimulation, lastSegmentWidth;


        public Rope(World world, Vector2 initialPosition, float length, Vector2 worldAnchorA) : this(world,initialPosition,length)
        {
            JointFactory.CreateFixedRevoluteJoint(world, Segments.First(), new Vector2(-boxWidthSimulation / 2, 0), ConvertUnits.ToSimUnits(worldAnchorA));
        }

        public Rope(World world, Vector2 initialPosition, float length, Vector2 worldAnchorA, Vector2 worldAnchorB) : this(world,initialPosition,length,worldAnchorA)
        {
            JointFactory.CreateFixedRevoluteJoint(world, Segments.Last(), new Vector2(lastSegmentWidth / 2, 0), ConvertUnits.ToSimUnits(worldAnchorB));
        }

        private Rope(World world, Vector2 initialPosition, float length)
        {
            var type = BodyType.Dynamic;
            Segments = new List<Body>();
            float boxWidth = 30f,
                  boxHeight = 5f;

            boxWidthSimulation = ConvertUnits.ToSimUnits(boxWidth);
            boxHeightSimulation = ConvertUnits.ToSimUnits(boxHeight);

            var position = new Vector2(initialPosition.X + boxWidth/2f, initialPosition.Y);

            FirstEnd = BodyFactory.CreateRectangle(world, boxWidthSimulation, boxHeightSimulation, 10f, ConvertUnits.ToSimUnits(position));
            FirstEnd.BodyType = type;
            Segments.Add(FirstEnd);
            var nextPosition = new Vector2(FirstEnd.Position.X + boxWidthSimulation, FirstEnd.Position.Y);
            float totalLength = boxWidthSimulation;

            while (totalLength + boxWidthSimulation <= ConvertUnits.ToSimUnits(length))
            {
                var body = BodyFactory.CreateRectangle(world, boxWidthSimulation, boxHeightSimulation, 10f, nextPosition);
                body.BodyType = type;
                JointFactory.CreateRevoluteJoint(world, body, Segments.Last(), new Vector2(boxWidthSimulation / 2, 0));
                Segments.Add(body);
                nextPosition.X += boxWidthSimulation;
                totalLength += boxWidthSimulation;
            }

            lastSegmentWidth = ConvertUnits.ToSimUnits(length) - totalLength;
            if (lastSegmentWidth > 0)
            {
                nextPosition.X -= boxWidthSimulation;
                nextPosition.X += boxWidthSimulation/2 + lastSegmentWidth/2;
                LastEnd = BodyFactory.CreateRectangle(world, lastSegmentWidth, boxHeightSimulation, 10f, nextPosition);
                JointFactory.CreateRevoluteJoint(world, LastEnd, Segments.Last(), new Vector2(boxWidthSimulation/2,0));
                LastEnd.BodyType = type;
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
        }

        public void Update(GameTime gameTime)
        {
        }
    }
}
