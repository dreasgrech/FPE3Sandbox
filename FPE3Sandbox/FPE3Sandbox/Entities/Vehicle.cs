using System;
using System.Collections.Generic;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using FarseerPhysicsBaseFramework.GameEntities;
using FarseerPhysicsBaseFramework.GameEntities.Physics;
using FarseerPhysicsBaseFramework.Helpers;
using FarseerPhysicsBaseFramework.Helpers.Camera;
using FPE3Sandbox.Helpers;
using Microsoft.Xna.Framework;

namespace FPE3Sandbox.Entities
{
    public class Vehicle:IPlayable, IFocusable
    {
        private List<PhysicsGameEntity> parts;

        private TexturedPhysicsEntity
            leftWheel, rightWheel, body;

        private RevoluteJoint leftAxisJoint, rightAxisJoint;

        private float speed = 15;
        public Vehicle(Game game, World world, Vector2 position)
        {
            var chassisTexture = new TexturedGameEntity(game, position, 0, "Images/vehicle_body", 1);
            parts = new List<PhysicsGameEntity>();



            var chassisBody = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(chassisTexture.Width), ConvertUnits.ToSimUnits(chassisTexture.Height), 10f, ConvertUnits.ToSimUnits(chassisTexture.Position));
            body = new TexturedPhysicsEntity(game, world, chassisTexture, chassisBody, new Vector2(chassisTexture.Width/2f, chassisTexture.Height/2f));
            
            //var bodyVertices = new FileTextureReader(@"VerticesList\vehicle.txt").GetVertices();
            //body = new TexturedPhysicsEntity(game,world,chassisTexture,bodyVertices,BodyType.Dynamic,10f);



            float axisWidth = 5, axisHeight = 50;
            Vector2 axisCentroid = new Vector2(axisWidth/2f, axisHeight/2f);
            PhysicsGameEntity leftAxis = new PhysicsGameEntity(game, world, BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(axisWidth), ConvertUnits.ToSimUnits(axisHeight), 10f, ConvertUnits.ToSimUnits(new Vector2(body.Position.X - 50, body.Position.Y + 15))), axisCentroid),
                rightAxis = new PhysicsGameEntity(game, world, BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(axisWidth), ConvertUnits.ToSimUnits(axisHeight), 10f, ConvertUnits.ToSimUnits(new Vector2(body.Position.X + 68, body.Position.Y + 15))), axisCentroid);
            
            leftAxis.Angle = MathHelper.ToRadians(90);

            leftWheel = new TexturedPhysicsEntity(game,world, new TexturedGameEntity(game, new Vector2(leftAxis.Position.X,leftAxis.Position.Y + 20), 0, "Images/wheel_left", 1), 10f, BodyType.Static);
            rightWheel = new TexturedPhysicsEntity(game,world,new TexturedGameEntity(game, new Vector2(rightAxis.Position.X, rightAxis.Position.Y + 20), 0, "Images/wheel_right", 1), 10f, BodyType.Static);
            ApplyToWheels();
            parts.Add(body);
            parts.Add(leftWheel);
            parts.Add(rightWheel);
            parts.Add(leftAxis);
            parts.Add(rightAxis);

            parts.ForEach(p => SetPartDynamics(p.Body));

            leftAxisJoint = JointFactory.CreateRevoluteJoint(world, leftAxis.Body, leftWheel.Body, Vector2.Zero);
            rightAxisJoint = JointFactory.CreateRevoluteJoint(world, rightAxis.Body, rightWheel.Body, Vector2.Zero);

            //body.Body.BodyType = BodyType.Kinematic;
            //JointFactory.CreateDistanceJoint(world, leftWheel.Body, rightWheel.Body, Vector2.Zero, Vector2.Zero);

            JointFactory.CreateWeldJoint(world, body.Body, leftAxis.Body, ConvertUnits.ToSimUnits(leftAxis.Position));// ConvertUnits.ToSimUnits(new Vector2(axis1.Position.X - 40, axis1.Position.Y)), Vector2.Zero);// ConvertUnits.ToSimUnits(new Vector2(0,body.Position.Y)));
            JointFactory.CreateWeldJoint(world, body.Body, rightAxis.Body, ConvertUnits.ToSimUnits(rightAxis.Position));//ConvertUnits.ToSimUnits(new Vector2(axis2.Position.X + 80, axis2.Position.Y)), Vector2.Zero);
        }

        void ApplyToWheel(TexturedPhysicsEntity wheel)
        {
            wheel.Body.Friction = 5;
            wheel.Body.Restitution = 0.2f;
        }
        void ApplyToWheels()
        {
            ApplyToWheel(leftWheel);
            ApplyToWheel(rightWheel);
        }
        static void SetPartDynamics(Body body)
        {
            body.BodyType = BodyType.Static;
            body.CollisionCategories = Category.Cat2;
            body.CollidesWith = ~Category.Cat2;
            //body.CollisionGroup = 2;
        }
        public void Draw(GameTime gameTime)
        {
            parts.ForEach(p => p.Draw(gameTime));
        }

        public void Update(GameTime gameTime)
        {
            var reading = AccelerometerSensor.GetReading();
            leftAxisJoint.MotorSpeed = (float)(15 * Math.PI * (reading.Position.X > 0 ? 1 : -1));
            leftAxisJoint.MaxMotorTorque = 170;
            rightAxisJoint.MotorSpeed = (float)(15 * Math.PI * (reading.Position.X > 0 ? 1 : -1));// (float)(150 * Math.PI * (reading.Position.X > 0 ? 1 : -1));
            rightAxisJoint.MaxMotorTorque = 170;

            //leftWheel.Body.ApplyTorque(200 * reading.Position.X);
            //rightWheel.Body.ApplyTorque(200 * reading.Position.X);
            body.Body.ApplyTorque(200 * reading.Position.X);
            //leftAxisJoint.MaxMotorTorque = (float) (reading.Position.X > 0 || reading.Position.X < 0 ? 500 : 0.5); 
            //leftWheel.Body.ApplyForce(new Vector2(reading.Position.X * speed, 0));
            //rightWheel.Body.ApplyForce(new Vector2(reading.Position.X * speed, 0));
            //leftWheel.Body.t;
        }

        public Vector2 Position
        {
            get { return body.Position; }
        }
    }
}
