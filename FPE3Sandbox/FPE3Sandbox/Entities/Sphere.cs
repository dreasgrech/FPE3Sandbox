using System;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysicsBaseFramework.GameEntities;
using FarseerPhysicsBaseFramework.GameEntities.Physics;
using FarseerPhysicsBaseFramework.Helpers;
using FarseerPhysicsBaseFramework.Helpers.Camera;
using FPE3Sandbox.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace FPE3Sandbox.Entities
{
    public class Sphere:IPlayable, ITouchRespondant, IFocusable
    {
        private TexturedPhysicsEntity sphere;
        private TexturedGameEntity texture;

        private uint flickLimit = 30, consecutiveJumps = 2, finalJumpIntervalMs = 1000, currentJumpCount = 0;
        private float moveForce = 50, jumpForce = 500;
        private DateTime lastJumpTime = DateTime.Now;
        private bool isOnGround;

        public Sphere(Game game, World world, Vector2 position, float density)
        {
            texture = new TexturedGameEntity(game, position, 0f, "Images/sphere1", 1f);

            var origin = new Vector2(texture.Width/2f, texture.Height/2f);
            var body = BodyFactory.CreateCircle(world, ConvertUnits.ToSimUnits(origin.X), density);
            body.Restitution = 0.4f;
            body.OnCollision += Body_OnCollision;
            body.Friction = 50f;
            body.BodyType = BodyType.Dynamic;
            sphere = new TexturedPhysicsEntity(game, world, texture, body, origin);
        }
        public void Draw(GameTime gameTime)
        {
            sphere.Draw(gameTime);
        }

        bool Body_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            if (fixtureB.Body.UserData is IPlatform)
            {
                isOnGround = true;
            }
            return true;
        }

        public void Update(GameTime gametime)
        {
            var reading = AccelerometerSensor.GetReading();

            //Debug.WriteLine(reading.Position.X * moveForce);
            var x = reading.Position.X * moveForce;
            if (isOnGround)
            {
                x = MathHelper.Clamp(x, -8f, 8f);
                sphere.Body.ApplyTorque(x);
            }
            else
            {
                sphere.Body.ApplyForce(new Vector2(x, 0));
            }

            //Body.ApplyTorque(reading.Position.X * moveForce);
            //Body.ApplyForce(new Vector2(reading.Position.X * moveForce, 0));

            var now = DateTime.Now;
            //if (currentJumpCount >= consecutiveJumps && (now - lastJumpTime).TotalMilliseconds >= finalJumpIntervalMs)
            if (isOnGround && (now - lastJumpTime).TotalMilliseconds >= finalJumpIntervalMs)
            {
                currentJumpCount = 0;
            }
        }

        public void RespondToTouch(GestureSample gesture)
        {
            //if (gesture.GestureType == GestureType.Flick)
            //{
            //    var move = new Vector2(MathHelper.Clamp(gesture.Delta.X, -flickLimit, flickLimit), MathHelper.Clamp(gesture.Delta.Y, -flickLimit, flickLimit));
            //    Body.ApplyLinearImpulse(ConvertUnits.ToSimUnits(move * 50));
            //}

            if (gesture.GestureType == GestureType.Tap)
            {
                if (currentJumpCount >= consecutiveJumps)
                {
                    return;
                }

                lastJumpTime = DateTime.Now;
                currentJumpCount++;

                if (!isOnGround)
                {
                    //Body.ResetDynamics();
                }
                sphere.Body.ApplyForce(new Vector2(0, -jumpForce));
                isOnGround = false;
            }

        }

        public Vector2 Position
        {
            get { return sphere.Position; }
        }
    }
}
