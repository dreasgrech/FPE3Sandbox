//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Text;
//using FarseerPhysics.Dynamics;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Input.Touch;
//using WP7Physics.Entities.AbstractEntities;
//using WP7Physics.Helpers;

//namespace WP7Physics.Entities
//{
//    class Helicopter:BreakablePhysicsGameEntity, ITouchRespondant
//    {
//        public Helicopter(Game game, Vector2 position):base(new TexturedGameEntity(game,position,0,"Images/heli",1),2f,BodyType.Dynamic )
//        {
//            Body.IgnoreGravity = true;
//            //Body.IsBullet = true;
//        }

//        public void RespondToTouch(GestureSample gesture)
//        {
//            //Body.ApplyForce(new Vector2(0,-50));
//        }

//        public override void Update(GameTime gametime)
//        {
//            var reading = AccelerometerSensor.GetReading();
//            if (Math.Abs(reading.Position.X) > 0.04 || Math.Abs(reading.Position.Y) > 0.04)
//            {
//                Body.ApplyForce(new Vector2(reading.Position.X * 20, -reading.Position.Y * 20));//,new Vector2(Center.X + ConvertUnits.ToSimUnits(30), Center.Y));
//                Debug.WriteLine("Flying");
//            } else
//            {
//                Debug.WriteLine("Static");
//                //Body.ApplyForce(new Vector2(0, -1));
//                //Body.ApplyTorque(100f);
//            }
//            Angle = 0;
//            base.Update(gametime);
//        }
//    }
//}
