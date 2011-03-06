using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Common.PhysicsLogic;
using FarseerPhysics.Dynamics;
using FarseerPhysicsBaseFramework.GameEntities;
using FarseerPhysicsBaseFramework.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace FPE3Sandbox.Entities
{
    class ExplosionTrigger:ITouchRespondant, IPlayable
    {
        /*
         * Still unfunctioning
         * From: http://farseerphysics.codeplex.com/SourceControl/changeset/view/85707#1545380
         */

        private Explosion explosion;

        public ExplosionTrigger(Game game, World world)
        {
            explosion = new Explosion(world);
        }

        public void RespondToTouch(GestureSample gesture)
        {
            explosion.Activate(ConvertUnits.ToSimUnits(gesture.Position), 15, 30);
        }

        public void Draw(GameTime gameTime)
        {
            
        }

        public void Update(GameTime gameTime)
        {
        }
    }
}
