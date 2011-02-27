using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;

namespace FPE3Sandbox.Entities.Platforms
{
    class RotatingPlatform:Platform
    {
        private FixedRevoluteJoint joint;

        public RotatingPlatform(Game game, World world, string image, Vector2 position, float angle, float density) : base(game, world,image, position, angle, density, BodyType.Dynamic)
        {
            joint = JointFactory.CreateFixedRevoluteJoint(World, Body, Vector2.Zero, Body.WorldCenter);
            //joint.MotorSpeed = 5;
            //joint.MotorEnabled = true;
            //joint.MotorTorque = 50;
            //joint.MaxMotorTorque = 100;
        }

        public override void Update(GameTime gametime)
        {
            base.Update(gametime);
        }
    }
}
