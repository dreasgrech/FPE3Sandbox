using System;
using System.Collections.Generic;
using FarseerPhysics;
using FarseerPhysics.DebugViews;
using FarseerPhysics.Dynamics;
using FarseerPhysicsBaseFramework.GameEntities;
using FarseerPhysicsBaseFramework.Helpers;
using FarseerPhysicsBaseFramework.Helpers.Camera;
using FPE3Sandbox.Entities;
using FPE3Sandbox.Helpers.Parallax;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using XNAScreenManager;

namespace FPE3Sandbox.GameScreens
{
    class MainGameScreen:GameScreen
    {
        private World world;
        private List<IPlayable> entities;
        private Vector2 gravity = new Vector2(0, 50f);
        private Parallax parallax;

        private Camera2D camera;
        private DebugViewXNA debugView;
        private Crosshair crosshair;

        public bool isDebugOn;

        private float viewScale = 1f, newScale = 1f;


        public MainGameScreen(Game game, SpriteBatch spriteBatch, GraphicsDeviceManager graphicsDeviceManager, ScreenManager screenManager) : base(game, spriteBatch, graphicsDeviceManager, screenManager)
        {
            TouchPanel.EnabledGestures = GestureType.Tap | GestureType.FreeDrag | GestureType.Flick | GestureType.Pinch;

            world = new World(gravity);
            game.Services.AddService(typeof(World), world);

            var sphere = new Sphere(game, world, new Vector2(850, 100), 10f);
            var terrain = new Terrain(game, world, graphicsDeviceManager.GraphicsDevice.Viewport.Height);
            crosshair = new Crosshair(game, spriteBatch, graphicsDeviceManager.GraphicsDevice.Viewport.Width, graphicsDeviceManager.GraphicsDevice.Viewport.Height);
            //var vehicle = new Vehicle(Game, world, new Vector2(500,150));
            entities = new List<IPlayable>
                           {
                               new ScreenBounds(world,GraphicsDeviceManager.GraphicsDevice.Viewport.Width,GraphicsDeviceManager.GraphicsDevice.Viewport.Height),
                               terrain,
                               //new TestEntity(game,"Images/block",new Vector2(400,200),1f,BodyType.Dynamic),
                               sphere,
                               //new Helicopter(game,new Vector2(400,400)),
                               //new RotatingPlatform(game,"platform1",new Vector2(600,-100),0,1f),
                               //new StaticPlatform(game,"platform1",new Vector2(600,500),0),
                               //new RotatingPlatform(game,"platform1",new Vector2(600,400),0,10f),
                               //new ElevatorPlatform(game,world,"platform2",new Vector2(400, 300),0,100f,new List<Vector2>{new Vector2(-200, 0), new Vector2(200,0), new Vector2(200, -200), new Vector2(-200,-200)}, 50f),
                               //new StaticPlatform(game,"platform1",new Vector2(200,650),0),
                               //vehicle
                               //new Rope(game,world, spriteBatch, new Vector2(858, terrain.Position.Y - (terrain.Height/2f) + 22),664)
                               new Bridge(game,world,spriteBatch, new Vector2(880,300),new Vector2(1450, 200)),
                               crosshair
                           };
            parallax = new Parallax(game,spriteBatch,ParallaxDirection.Left);
            parallax.AddLayer("Images/Parallax/cloud1", 0.5f, 2);
            parallax.AddLayer("Images/Parallax/cloud2", 0.5f, 5);
            game.Components.Add(parallax);

            camera = new Camera2D(game)
                         {
                             Focus = crosshair,
                             MoveSpeed = 10f,
                             //Clamp = p => new Vector2(
                             //                 MathHelper.Clamp(p.X, Game.GraphicsDevice.Viewport.Width / 2f,
                             //                                  Game.GraphicsDevice.Viewport.Width * 2 -
                             //                                  Game.GraphicsDevice.Viewport.Width / 2),
                             //                 MathHelper.Clamp(p.Y, float.NegativeInfinity,
                             //                                  Game.GraphicsDevice.Viewport.Height / 2f)),
                             Scale = 1.0f
                         };
            game.Components.Add(camera);
            debugView = new DebugViewXNA(world) {DefaultShapeColor = Color.White, SleepingShapeColor = Color.LightGray};
            debugView.LoadContent(graphicsDeviceManager.GraphicsDevice,game.Content);
            debugView.AppendFlags(DebugViewFlags.DebugPanel | DebugViewFlags.Joint | DebugViewFlags.Shape | DebugViewFlags.PerformanceGraph | DebugViewFlags.ContactPoints);
        }

        public override string Name
        {
            get { return "maingame"; }
        }

        public override void Draw(GameTime gameTime)
        {
            //camera.Scale *= viewScale;
            Vector2 size = (((new Vector2(GraphicsDeviceManager.GraphicsDevice.Viewport.Width, GraphicsDeviceManager.GraphicsDevice.Viewport.Height)) / (ConvertUnits.DisplayUnitsToSimUnitsRatio * 2)) / 1f);
            Matrix proj = Matrix.CreateOrthographicOffCenter(-size.X, size.X, size.Y, -size.Y, 0, 1),
                   view = Matrix.CreateTranslation(camera.PositionWithoutClamp.X / -ConvertUnits.DisplayUnitsToSimUnitsRatio, camera.PositionWithoutClamp.Y / -ConvertUnits.DisplayUnitsToSimUnitsRatio, 0);
            view *= Matrix.CreateScale(viewScale);

            SpriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, null, null, null, null, camera.Transform);// Matrix.CreateTranslation(camera.Position.X, camera.Position.Y, 0) * Matrix.CreateScale(viewScale));
            if (isDebugOn)
            {
                debugView.RenderDebugData(ref proj, ref view);
                crosshair.Draw(gameTime);
            } else
            {
                entities.ForEach(e => e.Draw(gameTime));
            }
            
            SpriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                isDebugOn = !isDebugOn;
            }

            world.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f, (1f / 30f)));
            entities.ForEach(e => e.Update(gameTime));

            while (TouchPanel.IsGestureAvailable)
            {
                GestureSample sample = TouchPanel.ReadGesture();
                entities.ForEach(e =>
                {
                    if (e is ITouchRespondant)
                    {
                        ((ITouchRespondant)e).RespondToTouch(sample);
                    }
                });
                if (sample.GestureType == GestureType.Pinch)
                {
                    Vector2 oldPosition = sample.Position - sample.Delta,
                            oldPosition2 = sample.Position2 - sample.Delta2;
                    float newDistance = Vector2.Distance(sample.Position, sample.Position2),
                          oldDistance = Vector2.Distance(oldPosition, oldPosition2),
                          scaleFactor = newDistance/oldDistance;
                    newScale = scaleFactor;
                    viewScale *= newScale;
                }
                if (sample.GestureType == GestureType.FreeDrag)
                {
                    crosshair.Position += -sample.Delta * 2;// new Vector2(-sample.Position.X, sample.Position.Y);
                    camera.Position += -sample.Delta * 2;
                }
            }
        }
    }
}
