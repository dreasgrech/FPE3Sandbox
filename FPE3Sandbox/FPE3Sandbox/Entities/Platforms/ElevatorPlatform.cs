using System;
using System.Collections.Generic;
using System.Linq;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;

namespace FPE3Sandbox.Entities.Platforms
{
    class ElevatorPlatform:Platform
    {
        private int toEdge;
        private float percentage;
        private Vector2 lastEdgePast, nextEdge;

        public List<Vector2> Edges { get; set; }
        public float Speed { get; set; }

        public ElevatorPlatform(Game game, World world, string image, Vector2 position, float angle, float density, List<Vector2> edges, float speed) : base(game, world, image, position, angle, density, BodyType.Kinematic)
        {
            if (edges.Count < 2)
            {
                throw new Exception("There must be at least 2 points");
            }
            Speed = speed;
            Body.IgnoreGravity = true;
            Body.Friction = 50f;
            Edges = MapWorldCoordinates(edges);
            toEdge = 0;
            AdvanceEdge();
        }

        public override void Update(GameTime gametime)
        {
            Position = Vector2.SmoothStep(lastEdgePast, nextEdge, percentage);
            //Body.ApplyForce(Vector2.SmoothStep(lastEdgePast, nextEdge, percentage));
            percentage += 0.001f * Speed;
            if (Math.Round(Position.X) == nextEdge.X && Math.Round(Position.Y) == nextEdge.Y)
            {
                AdvanceEdge();
            }
            base.Update(gametime);
        }

        private Vector2 AdvanceEdge()
        {
            percentage = 0;
            lastEdgePast = Edges[toEdge];
            toEdge = (toEdge + 1)%Edges.Count;
            return (nextEdge = Edges[toEdge]);
        }

        private List<Vector2> MapWorldCoordinates(IEnumerable<Vector2> edges)
        {
            return edges.Select(e => e + Position).ToList();
        }
    }
}
