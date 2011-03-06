using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace FPE3Sandbox.Entities
{
    public class CountdownTimer:GameComponent
    {
        public CountdownTimer(Game game, TimeSpan interval) : base(game)
        {
            Countdown = interval;
        }

        public bool IsCountdownExpired { get; private set; }
        public bool IsTimerRunning { get; private set; }
        public TimeSpan Countdown { get; set; }

        private TimeSpan currentTimer;

        public void Start ()
        {
            IsTimerRunning = true;
            IsCountdownExpired = false;
        }

        public void Stop()
        {
            IsTimerRunning = false;
            IsCountdownExpired = true;
            currentTimer = new TimeSpan();
        }

        public void Restart()
        {
            Stop();
            Start();
        }

        public override void Update(GameTime gameTime)
        {
            if (IsTimerRunning)
            {
                currentTimer += gameTime.ElapsedGameTime;
                if (currentTimer > Countdown)
                {
                    Stop();
                }
            }

            base.Update(gameTime);
        }

    }
}
