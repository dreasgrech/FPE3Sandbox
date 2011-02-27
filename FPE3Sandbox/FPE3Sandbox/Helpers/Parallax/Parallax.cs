using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FPE3Sandbox.Helpers.Parallax
{
    enum ParallaxDirection
    {
        Left = 0x1,
        Right = -0x1
    }

    class Parallax:DrawableGameComponent
    {
        private Game game;
        private SpriteBatch spriteBatch;
        private List<ParrallaxLayer> layers;
        private ParallaxDirection direction;

        public Parallax(Game game, SpriteBatch spriteBatch, ParallaxDirection direction) : base(game)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;
            this.direction = direction;
            layers = new List<ParrallaxLayer>();
        }

        public void AddLayer(string layer, float zIndex, float speed)
        {
            layers.Add(new ParrallaxLayer(game,layer,speed,zIndex,direction));
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
            layers.ForEach(l => l.Draw(gameTime,spriteBatch));
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            layers.ForEach(l => l.Update(gameTime));
            base.Update(gameTime);
        }
    }
}
