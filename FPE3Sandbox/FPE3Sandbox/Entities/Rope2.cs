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
    public class Rope2:IPlayable
    {
        public Body FirstEnd { get; private set; }
        public Body LastEnd { get; private set; }
        public List<Body> Segments { get; set; }

        public Rope2(Game game, World world, Vector2 initialPosition, float length)
        {
            var type = BodyType.Dynamic;
            Segments = new List<Body>();
            float boxWidth = 30f, boxHeight = 5f;
            Vertices box = PolygonTools.CreateRectangle(ConvertUnits.ToSimUnits(boxWidth), ConvertUnits.ToSimUnits(boxHeight)); // rope block

            var position = new Vector2(initialPosition.X + boxWidth/2f, initialPosition.Y);

            FirstEnd = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(boxWidth), ConvertUnits.ToSimUnits(boxHeight), 10f, ConvertUnits.ToSimUnits(position));
            FirstEnd.BodyType = type;
            Segments.Add(FirstEnd);
            var nextPosition = new Vector2(FirstEnd.Position.X + ConvertUnits.ToSimUnits(boxWidth), FirstEnd.Position.Y);
            float totalLength = ConvertUnits.ToSimUnits(boxWidth);

            while (totalLength + ConvertUnits.ToSimUnits(boxWidth) <= ConvertUnits.ToSimUnits(length))
            {
                var body = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(boxWidth), ConvertUnits.ToSimUnits(boxHeight), 10f, nextPosition);
                body.BodyType = type;
                JointFactory.CreateRevoluteJoint(world, body, Segments.Last(), new Vector2(ConvertUnits.ToSimUnits(boxWidth) / 2, 0));
                Segments.Add(body);
                nextPosition.X += ConvertUnits.ToSimUnits(boxWidth);
                totalLength += ConvertUnits.ToSimUnits(boxWidth);
            }

            var lastSegmentWidth = ConvertUnits.ToSimUnits(length) - totalLength;
            if (lastSegmentWidth > 0)
            {
                nextPosition.X -= ConvertUnits.ToSimUnits(boxWidth);
                nextPosition.X += ConvertUnits.ToSimUnits(boxWidth)/2 + lastSegmentWidth/2;
                LastEnd = BodyFactory.CreateRectangle(world, lastSegmentWidth, ConvertUnits.ToSimUnits(boxHeight), 10f, nextPosition);
                JointFactory.CreateRevoluteJoint(world, LastEnd, Segments.Last(), new Vector2(ConvertUnits.ToSimUnits(boxWidth)/2,0));
                LastEnd.BodyType = type;
                totalLength += lastSegmentWidth;
                Segments.Add(LastEnd);
            } else
            {
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
