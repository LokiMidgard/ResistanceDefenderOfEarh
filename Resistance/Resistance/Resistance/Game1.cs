using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using Mitgard.Resistance.Scene;
using Mitgard.Resistance.Loading;
using Mitgard.Resistance.Musicplayer;
using System.Threading;
using Microsoft.Phone.Shell;

namespace Midgard.Resistance
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {

        public static readonly Random random = new Random();

        public static Game1 instance;

        public SpriteFont font;

        public Texture2D clearStdBackground;


        public Queue<Action> actionList = new Queue<Action>();

        float deltaFPSTime = 0;
        float fps = 0;

        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;

        IScene actualScene;

        public Game1()
        {
            instance = this;

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.IsFullScreen = true;

            PhoneApplicationService.Current.Activated += new EventHandler<ActivatedEventArgs>(Current_Activated);
            PhoneApplicationService.Current.Deactivated += new EventHandler<DeactivatedEventArgs>(Current_Deactivated);


            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);
        }

        void Current_Deactivated(object sender, DeactivatedEventArgs e)
        {

        }

        void Current_Activated(object sender, ActivatedEventArgs e)
        {

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            spriteBatch = new SpriteBatch(GraphicsDevice);
            SwitchToScene(new TitleScene());
            TouchPanel.EnabledGestures = GestureType.Tap;

            this.IsFixedTimeStep = false;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.

            font = Content.Load<SpriteFont>("font");
            clearStdBackground = Content.Load<Texture2D>(@"Menue\ResTitelWP7_ohneTitle");
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // The time since Update was called last
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            float fps = 1 / elapsed;
            deltaFPSTime += elapsed;
            if (deltaFPSTime > 1)
            {
                this.fps = fps;
                deltaFPSTime -= 1;
            }



            MusicManager.Update(gameTime);

            actualScene.Update(gameTime);


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);


            actualScene.Draw(gameTime);

            spriteBatch.Begin();
            spriteBatch.DrawString(font, fps.ToString(), Vector2.Zero, Color.Magenta);
            spriteBatch.End();

            base.Draw(gameTime);
        }



        public void SwitchToScene(IScene scene)
        {
            scene.Initilize();
            Action a = () =>
            {
                actualScene = scene;
                scene.DoneLoading();
            };
            if (actionList.Count > 0)
            {
                LoadingScene l = new LoadingScene(actionList, a);
                actualScene = l;
            }
            else
                a();
        }


        Dictionary<String, object> loaded = new Dictionary<string, object>();

        public void LoadContent<T>(String name, LoadedDelegate<T> del)
        {

            System.Action a = () =>
              {
                  if (!loaded.ContainsKey(name))
                  {
                      var x = Content.Load<T>(name);
                      loaded.Add(name, x);
                  }
                  //Thread.Sleep(500);
                  del((T)loaded[name]);
              };

            actionList.Enqueue(a);


        }


    }
}
