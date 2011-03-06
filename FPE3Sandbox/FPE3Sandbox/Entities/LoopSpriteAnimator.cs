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
        private Texture2D texture;
        private float interval, width, height;
        private int currentFrame, totalSprites;
        private List<Rectangle> spriteBounds;
        private CountdownTimer timer;
        private SpriteBatch spriteBatch;

        public override Vector2 Position { get; set; }

        /// <summary>
        /// Initializes a new Loop SpriteAnimator
        /// </summary>
        /// <param name="game"></param>
        /// <param name="spriteBatch"></param>
        /// <param name="texture2D"></param>
        /// <param name="interval">The duration of each frame, in milliseconds</param>
        /// <param name="columns">The number of columns in the sprite sheet grid</param>
        /// <param name="rows">The number of rows in the sprite sheet grid</param>
        /// <param name="totalSprites">The total number of sprites in the sprite sheet</param>
        public LoopSpriteAnimator(Game game, SpriteBatch spriteBatch, Texture2D texture2D, float interval, int columns, int rows, int totalSprites)
            : base(game)
        {
            this.spriteBatch = spriteBatch;
            timer = new CountdownTimer(game,new TimeSpan(0,0,0,0,(int)interval));
            game.Components.Add(timer);
            timer.Start();

            texture = texture2D;
            this.interval = interval;
            width = texture2D.Width / (float)columns;
            height = texture2D.Height / (float)rows;
            this.totalSprites = totalSprites;

            currentFrame = 0;

            spriteBounds = GetSpriteBounds(rows, columns);

        }

        /// <summary>
        /// Returns each sprite's bounding box
        /// </summary>
        /// <param name="rows">Number of rows</param>
        /// <param name="columns">Number of columns</param>
        /// <returns></returns>
        private List<Rectangle> GetSpriteBounds(int rows, int columns)
        {
            var bounds = new List<Rectangle>();
            int tot = 0;
            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    if (tot++ >= totalSprites)
                    {
                        goto PostProcessing; // http://xkcd.com/292/
                    }

                    var bound = new Rectangle((int) (column*width), (int) (row*height), (int) width, (int) height);
                    bounds.Add(bound);
                }
            }
        PostProcessing:
            return bounds;
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Draw(texture,Position,spriteBounds[currentFrame],Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            if (timer.IsCountdownExpired) // Time to move to the next frame
            {
                currentFrame = (currentFrame + 1) % totalSprites;
                timer.Restart();
            }
        }
    }
}
