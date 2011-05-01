using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mitgard.Resistance.Input;
using Mitgard.Resistance.Sprite;
using Midgard.Resistance;
using Microsoft.Xna.Framework;
using Mitgard.Resistance.LevelBackground;

namespace Mitgard.Resistance.Scene
{
    public class GameScene : IScene
    {

        public const int WORLD_WIDTH = 4000;
        public const int WORLD_HEIGHT = 480;

        public const int VIEWPORT_WIDTH = 800;
        public const int VIEWPORT_HEIGHT = 480;



        DesertBackground background;


        public Vector2 ViewPort;

        public GameInput input;

        public Player player;

        public List<AbstractEnemy> enemys = new List<AbstractEnemy>();
        public List<Human> humans = new List<Human>();
        public int score;

        public GameScene()
        {
            player = new Player(this);
            input = new GameInput();

            for (int i = 0; i < 100; i++)
            {
                humans.Add(new Human(this));
            }
            for (int i = 0; i < 100; i++)
            {
                enemys.Add(new EnemyCollector(this));
            }
            background = new DesertBackground(this);

        }

        public void Initilize()
        {
            foreach (var h in humans)
            {
                h.Initilize();
            }
            foreach (var h in enemys)
            {
                h.Initilize();
            }

            input.Initilize();

            player.Initilize();
            background.Initilize();
        }

        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (humans.Count == 0)
            {
                GameOver();
                return;
            }
            if (enemys.Count == 0)
            {
                NextLevel();
                return;
            }

            input.Update(gameTime);


            foreach (var h in humans)
            {
                h.Update(gameTime);
            }
            List<AbstractEnemy> eToDelete = new List<AbstractEnemy>();
            foreach (var h in enemys)
            {
                h.Update(gameTime);
                if (!h.Visible)
                    eToDelete.Add(h);
            }
            foreach (var e in eToDelete)
            {
                enemys.Remove(e);
            }



            player.Update(gameTime);

            MoveEveryThing();


            background.Update(gameTime);
        }

        private void MoveEveryThing()
        {
            if (player.position.X < 0)
            {
                player.position.X += WORLD_WIDTH;
            }
            else if (player.position.X > WORLD_WIDTH)
            {
                player.position.X -= WORLD_WIDTH;
            }

            ViewPort = (player.position * new Vector2(1, 1)) - new Vector2(VIEWPORT_WIDTH / 2, VIEWPORT_HEIGHT / 2);
            ViewPort.Y = MathHelper.Clamp(ViewPort.Y, 0, WORLD_HEIGHT - VIEWPORT_HEIGHT);

            foreach (var s in enemys)
            {
                if (s.position.X < ViewPort.X - (WORLD_WIDTH >> 1))
                {
                    s.position.X += WORLD_WIDTH;
                }
                else if (s.position.X > ViewPort.X + (WORLD_WIDTH >> 1))
                {
                    s.position.X -= WORLD_WIDTH;
                }
            }


            foreach (var s in player.allShots)
            {

                if (s.position.X < ViewPort.X - (WORLD_WIDTH >> 1))
                {
                    s.position.X += WORLD_WIDTH;


                }
                else if (s.position.X > ViewPort.X + (WORLD_WIDTH >> 1))
                {
                    s.position.X -= WORLD_WIDTH;


                }
            }
            foreach (var s in humans)
            {

                if (s.position.X < ViewPort.X - (WORLD_WIDTH >> 1))
                {
                    s.position.X += WORLD_WIDTH;


                }
                else if (s.position.X > ViewPort.X + (WORLD_WIDTH >> 1))
                {
                    s.position.X -= WORLD_WIDTH;


                }
            }





        }

        private void NextLevel()
        {
            Game1.instance.SwitchToScene(new GameOverScene());
        }

        private static void GameOver()
        {
            Game1.instance.SwitchToScene(new GameOverScene());
        }

        public void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            var batch = Game1.instance.spriteBatch;
            batch.Begin();

            background.Draw(gameTime);

            player.Draw(gameTime);


            foreach (var h in humans)
            {
                h.Draw(gameTime);
            }
            foreach (var h in enemys)
            {
                h.Draw(gameTime);
            }

            input.Draw(gameTime);

            batch.DrawString(Game1.instance.font, score.ToString(), Vector2.Zero, Color.Green);

            batch.End();
        }

        public void DoneLoading()
        {

        }



        public class Serelizer
        {




        }
    }
}
