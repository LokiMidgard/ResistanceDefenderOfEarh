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

        public const float SHOTSPEED_NORMAL = 48f;
        public const float SHOTSPEED_FAST = 64f;
        public const float SHOTSPEED_SLOW = 24f;


        DesertBackground background;

        Hud hud;

        public Vector2 ViewPort;

        public GameInput input;

        public Player player;

        public List<AbstractEnemy> enemys = new List<AbstractEnemy>();
        public List<Human> humans = new List<Human>();
        public int score;
        public Dificulty difficulty;
        public bool enemyTargetting = false;
        public float EnemyShotSpeed = 45;
        public int level;
        public int noHumans;
        public int noPredetor;
        public int noCollector;

        public GameScene(Dificulty dificulty)
        {
            this.difficulty = dificulty;
            player = new Player(this);
            input = new GameInput();

            background = new DesertBackground(this);
            hud = new Hud(this);

            switch (difficulty)
            {
                case Dificulty.Easy:
                    this.noHumans = 20;
                    this.noPredetor = 0;
                    this.noCollector = 5;
                    //this.noMine = 0;
                    enemyTargetting = false;
                    EnemyShotSpeed = SHOTSPEED_SLOW;
                    break;
                case Dificulty.Medium:
                    this.noHumans = 15;
                    this.noPredetor = 10;
                    this.noCollector = 10;
                    //this.noMine = 0;
                    enemyTargetting = false;
                    EnemyShotSpeed = SHOTSPEED_SLOW;
                    break;
                case Dificulty.Hard:
                    this.noHumans = 10;
                    this.noPredetor = 15;
                    this.noCollector = 10;
                    //this.noMine = 0;
                    enemyTargetting = true;
                    EnemyShotSpeed = SHOTSPEED_FAST;
                    break;
            }



            CreateNewEnemys();


        }

        private void CreateNewEnemys()
        {
            for (int i = 0; i < noPredetor; i++)
            {

                EnemyPredator e = new EnemyPredator(this);
                enemys.Add(e);
            }
            for (int i = 0; i < noCollector; i++)
            {
                EnemyCollector e = new EnemyCollector(this);
                enemys.Add(e);
            }
            humans.Clear();
            for (int i = 0; i < noHumans; i++)
            {
                Human e = new Human(this);
                humans.Add(e);
            }
            //for (int i = 0; i < noMine; i++)
            //{
            //    EnemyMine e = new EnemyMine();
            //    enemys.addElement(e);
            //    enemysManager.append(e);
            //}
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
            hud.Initilize();
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
            hud.Update(gameTime);
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

        public enum Dificulty
        {
            Easy = 0,
            Medium = 1,
            Hard = 2
        }

        private void NextLevel()
        {

            ++level;
            // EnemyDestroyer.clearDestroyer();
            switch (difficulty)
            {
                case Dificulty.Easy:
                    this.noHumans = humans.Count + Game1.random.Next(3) + 3;
                    this.noPredetor += (Game1.random.Next(3) + 2);
                    this.noCollector += (Game1.random.Next(3) + 2);
                    if (level > 9)
                    {
                        enemyTargetting = true;
                    }
                    if (level > 3)
                    {
                        this.EnemyShotSpeed = SHOTSPEED_NORMAL;
                    }
                    if (level > 14)
                    {
                        this.EnemyShotSpeed = SHOTSPEED_FAST;
                    }
                    if (level > 2)
                    {
                        //  this.noMine += (Game1.random.Next(3) + 1);
                    }
                    ;
                    break;
                case Dificulty.Medium:
                    this.noHumans = humans.Count + Game1.random.Next(4) + 2;
                    this.noPredetor += (Game1.random.Next(2) + 3);
                    this.noCollector += (Game1.random.Next(2) + 3);
                    if (level > 5)
                    {
                        enemyTargetting = true;
                    }
                    if (level > 2)
                    {
                        this.EnemyShotSpeed = SHOTSPEED_NORMAL;
                    }
                    if (level > 9)
                    {
                        this.EnemyShotSpeed = SHOTSPEED_FAST;
                    }

                    if (level > 1)
                    {
                        // this.noMine += (Game1.random.Next(4) + 2);
                    }
                    break;
                case Dificulty.Hard:
                    this.noHumans = humans.Count + Game1.random.Next(5) + 1;
                    this.noPredetor += (Game1.random.Next(1) + 4);
                    this.noCollector += (Game1.random.Next(1) + 4);
                    if (level > 3)
                    {
                        this.EnemyShotSpeed = SHOTSPEED_FAST;
                    }
                    if (level > 1)
                    {
                        //  this.noMine += (Game1.random.Next(2) + 4);
                    }
                    break;
            }
            CreateNewEnemys();


        }

        public void GameOver()
        {
            Game1.instance.SwitchToScene(new GameOverScene(score));
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

            hud.Draw(gameTime);


            batch.End();
        }

        public void DoneLoading()
        {

        }





    }
}
