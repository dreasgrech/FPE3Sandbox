using FarseerPhysicsBaseFramework.GameEntities;
using FarseerPhysicsBaseFramework.Helpers.Camera;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FPE3Sandbox.Entities
{
    class Crosshair:IFocusable, IPlayable
    {
        // TODO: NEEDS MAJOR REFACTORING

        private Texture2D texture;
        private Vector2 screenCenter;
        private SpriteBatch spriteBatch;

        public Crosshair(Game game, SpriteBatch spriteBatch, int screenWidth, int screenHeight)
        {
            this.spriteBatch = spriteBatch;
            texture = game.Content.Load<Texture2D>("Images/crosshair1");
            screenCenter = new Vector2(screenWidth/2f, screenHeight/2f);
            Position = screenCenter;
        }

        public Vector2 Position
        {
            get; set;
        }

        public void Draw(GameTime gameTime)
        {
            //spriteBatch.Draw(texture, Position,Color.White);
        }

        public void Update(GameTime gameTime)
        {
        }
    }
}
