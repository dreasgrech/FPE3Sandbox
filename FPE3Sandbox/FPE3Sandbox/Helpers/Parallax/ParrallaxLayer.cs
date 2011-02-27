using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FPE3Sandbox.Helpers.Parallax
{
    class ParrallaxLayer
    {
        public float Speed { get; set; }
        public float ZIndex { get; set; }
        public Texture2D Entity1 { get; set; }
        public Texture2D Entity2 { get; set; }
        public Vector2 Position1 { get; set; }
        public Vector2 Position2 { get; set; }
        public ParallaxDirection Direction { get; set; }

        public ParrallaxLayer(Game game, string entity, float speed, float zIndex, ParallaxDirection direction)
        {
            Direction = direction;
            Entity1 = game.Content.Load<Texture2D>(entity);
            Entity2 = game.Content.Load<Texture2D>(entity);
            Speed = speed;
            ZIndex = zIndex;
            Position1 = new Vector2(0);
            Position2 = new Vector2(Entity1.Width * (int)direction, Position1.Y);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Entity1, Position1, Color.White);
            spriteBatch.Draw(Entity2, Position2, Color.White);
        }

        public void Update(GameTime gameTime)
        {
            Position1 = new Vector2(Position1.X - (Speed * (int)Direction), Position1.Y);
            Position2 = new Vector2(Position2.X - (Speed * (int)Direction), Position2.Y);
            if (Position1.X < -Entity1.Width)
            {
                Position1 = new Vector2(Entity2.Width, Position1.Y);
            }
            if (Position2.X < -Entity2.Width)
            {
                Position2 = new Vector2(Entity2.Width, Position2.Y);
            }
            if (Position1.X > Entity1.Width)
            {
                Position1 = new Vector2(-Entity1.Width, Position1.Y);
            }
            if (Position2.X > Entity2.Width)
            {
                Position2 = new Vector2(-Entity2.Width, Position1.Y);
            }
        }
    }
}
