using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysicsBaseFramework.GameEntities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FPE3Sandbox.Entities
{
    class LoopSpriteAnimator:Entity
    {
        private Texture2D mainTexture;
        private float interval, width, height;
        private int currentFrame, totalSprites;
        private List<Rectangle> spriteBounds;
        private CountdownTimer timer;

        public override Vector2 Position { get; set; }


        public LoopSpriteAnimator(Game game, Texture2D texture2D, float interval, int width, int height, int totalSprites)
            : base(game)
        {
            timer = new CountdownTimer(game,new TimeSpan(0,0,2));
            game.Components.Add(timer);
            timer.Start();

            mainTexture = texture2D;
            this.interval = interval;
            this.width = width;
            this.height = height;
            this.totalSprites = totalSprites;

            currentFrame = 0;

            int rows = texture2D.Width/width, columns = texture2D.Height/height;
            spriteBounds = new List<Rectangle>();
            for (int i = 0; i < totalSprites; i++)
            {
                
            }
        }


        public override void Draw(GameTime gameTime)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            if (timer.IsCountdownExpired)
            {
                timer.Restart();
                // Time to move to the next frame
                currentFrame = (currentFrame + 1) % totalSprites;
            }
        }
    }
}
