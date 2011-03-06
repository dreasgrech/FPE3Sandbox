using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using FarseerPhysicsBaseFramework.GameEntities;
using FarseerPhysicsBaseFramework.GameEntities.Physics;
using FarseerPhysicsBaseFramework.Helpers;
using Microsoft.Xna.Framework;

namespace FPE3Sandbox.Entities
{
    class BigWheel:IPlayable
    {
        private TexturedPhysicsEntity wheel;
        private FixedRevoluteJoint revJoint;

        public BigWheel(Game game, World world, Vector2 position)
        {
            var texture = new TexturedGameEntity(game, position, 0, "Images/bigWheel", 1);
            var body = BodyFactory.CreateCircle(world, ConvertUnits.ToSimUnits(texture.Width/2), 1f);
            body.BodyType = BodyType.Dynamic;
            wheel = new TexturedPhysicsEntity(game, world, Category.Cat31, texture, body, new Vector2(texture.Width/2f, texture.Height/2f));

            revJoint = JointFactory.CreateFixedRevoluteJoint(world, wheel.Body, Vector2.Zero, ConvertUnits.ToSimUnits(wheel.Position));
            revJoint.MotorEnabled = true;
            revJoint.MotorSpeed = 3000;
            /*revJoint.MaxMotorTorque = 3000;
            revJoint.LimitEnabled = true;
            revJoint.UpperLimit = 3000;*/
        }
        public void Draw(GameTime gameTime)
        {
            wheel.Draw(gameTime);
        }

        public void Update(GameTime gameTime)
        {
            wheel.Update(gameTime);
        }
    }
}
