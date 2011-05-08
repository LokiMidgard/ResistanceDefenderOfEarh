using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mitgard.Resistance.Input;
using Mitgard.Resistance.Sprite;
using Midgard.Resistance;
using Microsoft.Xna.Framework;
using Mitgard.Resistance.LevelBackground;
using Microsoft.Phone.Shell;
using Mitgard.Resistance.Configuration;

namespace Mitgard.Resistance.Scene
{
    public class GameScene : IScene
    {


        public const int VIEWPORT_WIDTH = 800;
        public const int VIEWPORT_HEIGHT = 480;

        public const float SHOTSPEED_NORMAL = 48f;
        public const float SHOTSPEED_FAST = 64f;
        public const float SHOTSPEED_SLOW = 24f;


        public GameConfiguration configuration = new GameConfiguration();

        DesertBackground background;

        Hud hud;

        public Vector2 ViewPort;

        public GameInput input;

        public Player player;

        public EnemyDestroyer destroyer;
        public List<AbstractEnemy> notDestroyedEnemys = new List<AbstractEnemy>();
        public List<Human> notKilledHumans = new List<Human>();
        public List<AbstractEnemy> allEnemys = new List<AbstractEnemy>();
        public List<Human> allHumans = new List<Human>();
        public List<EnemyPredator.Shot> allEnemyShots = new List<EnemyPredator.Shot>();
        public int score;
        public Dificulty difficulty;
        public bool enemyTargetting = false;

        public int scoreBeginLevel;
        private double bosCountdown;
        private double destroyerTime;

        public GameScene(Tombstone t)
        {


            this.difficulty = t.difficulty;
            player = new Player(this);
            input = new GameInput();

            background = new DesertBackground(this);
            hud = new Hud(this);

            PrepareGame();

            configuration.EnemyShotSpeed = t.EnemyShotSpeed;
            enemyTargetting = t.enemyTargetting;
            configuration.Level = t.level;
            configuration.NoCollector = t.noCollector;
            configuration.NoHumans = t.noHumans;
            configuration.NoPredator = t.noPredetor;
            configuration.NoMine = t.noMine;
            score = t.score;
            scoreBeginLevel = score;
            destroyer = new EnemyDestroyer(this);

            CreateNewEnemys(false);


        }

        public GameScene(Dificulty dificulty)
        {
            this.difficulty = dificulty;
            player = new Player(this);
            input = new GameInput();

            background = new DesertBackground(this);
            hud = new Hud(this);
            PrepareGame();
            destroyer = new EnemyDestroyer(this);
            CreateNewEnemys(false);


        }

        private void PrepareGame()
        {

            switch (this.difficulty)
            {
                case Dificulty.Easy:
                    configuration.NoHumans = 20;
                    configuration.NoPredator = 0;
                    configuration.NoCollector = 5;
                    configuration.NoMine = 0;
                    enemyTargetting = false;
                    configuration.EnemyShotSpeed = SHOTSPEED_SLOW;
                    break;
                case Dificulty.Medium:
                    configuration.NoHumans = 15;
                    configuration.NoPredator = 10;
                    configuration.NoCollector = 10;
                    configuration.NoMine = 0;
                    enemyTargetting = false;
                    configuration.EnemyShotSpeed = SHOTSPEED_SLOW;
                    break;
                case Dificulty.Hard:
                    configuration.NoHumans = 10;
                    configuration.NoPredator = 15;
                    configuration.NoCollector = 10;
                    configuration.NoMine = 0;
                    enemyTargetting = true;
                    configuration.EnemyShotSpeed = SHOTSPEED_FAST;
                    break;
            }



        }

        private void CreateNewEnemys(bool gameRunning)
        {
            var newEnemys = new List<AbstractEnemy>();
            var newHumans = new List<Human>();
            for (int i = allEnemys.Count(x => x is EnemyPredator); i < configuration.NoPredator; i++)
            {

                EnemyPredator e = new EnemyPredator(this);
                newEnemys.Add(e);
            }
            for (int i = allEnemys.Count(x => x is EnemyCollector); i < configuration.NoCollector; i++)
            {
                EnemyCollector e = new EnemyCollector(this);
                newEnemys.Add(e);
            }

            for (int i = allHumans.Count; i < configuration.NoHumans; i++)
            {
                Human e = new Human(this);
                newHumans.Add(e);
            }
            for (int i = newEnemys.Count(x => x is EnemyMine); i < configuration.NoMine; i++)
            {
                EnemyMine e = new EnemyMine(this);
                newEnemys.Add(e);
            }

            allHumans.AddRange(newHumans);
            notKilledHumans.Clear();
            notKilledHumans.AddRange(allHumans);


            allEnemys.AddRange(newEnemys);
            notDestroyedEnemys.Clear();
            notDestroyedEnemys.AddRange(allEnemys);

            foreach (var h in allHumans)
            {
                h.Initilize();
            }
            foreach (var h in allEnemys)
            {
                h.Initilize();
            }
            if (gameRunning)
            {
                Game1.instance.LoadContentImidetly();
            }
        }

        public void Initilize()
        {
            if (((int)difficulty + 1) * 2 >= Game1.random.Next(10) + 1)
                destroyerTime = (3 - (int)difficulty) << 10;
            destroyer.Initilize();
            foreach (var h in allHumans)
            {
                h.Initilize();
            }
            foreach (var h in allEnemys)
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

            if (notKilledHumans.Count == 0)
            {
                GameOver();
                return;
            }
            if (notDestroyedEnemys.Count == 0)
            {
                NextLevel();
                return;
            }

            input.Update(gameTime);


            foreach (var h in notKilledHumans)
            {
                h.Update(gameTime);
            }
            List<AbstractEnemy> eToDelete = new List<AbstractEnemy>();
            foreach (var h in notDestroyedEnemys)
            {
                h.Update(gameTime);
                if (!h.Visible)
                    eToDelete.Add(h);
            }
            foreach (var e in eToDelete)
            {
                notDestroyedEnemys.Remove(e);
            }

            destroyer.Update(gameTime);

            foreach (var s in allEnemyShots)
            {
                s.Update(gameTime);
            }

            bosCountdown += gameTime.ElapsedGameTime.TotalSeconds;
            if ((4 - (int)difficulty * 2) < configuration.Level && destroyerTime > 0 && destroyerTime <= bosCountdown)
            {
                destroyer.ReEnter();
                bosCountdown = 0.0;
                if (((int)difficulty + 1) * 2 >= Game1.random.Next(10) + 1)
                {
                    destroyerTime = (3 - (int)difficulty) << 10;
                }
                else
                {
                    destroyerTime = 0;
                }
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
                player.position.X += configuration.WorldWidth;
            }
            else if (player.position.X > configuration.WorldWidth)
            {
                player.position.X -= configuration.WorldWidth;
            }

            ViewPort = (player.position * new Vector2(1, 1)) - new Vector2(VIEWPORT_WIDTH / 2, VIEWPORT_HEIGHT / 2);
            ViewPort.Y = MathHelper.Clamp(ViewPort.Y, 0, configuration.WorldHeight - VIEWPORT_HEIGHT);

            foreach (var s in notDestroyedEnemys.Union(new AbstractEnemy[] { destroyer }))
            {
                if (s.position.X < ViewPort.X - (configuration.WorldWidth >> 1))
                {
                    s.position.X += configuration.WorldWidth;
                }
                else if (s.position.X > ViewPort.X + (configuration.WorldWidth >> 1))
                {
                    s.position.X -= configuration.WorldWidth;
                }
            }


            foreach (var s in player.allShots)
            {

                if (s.position.X < ViewPort.X - (configuration.WorldWidth >> 1))
                {
                    s.position.X += configuration.WorldWidth;


                }
                else if (s.position.X > ViewPort.X + (configuration.WorldWidth >> 1))
                {
                    s.position.X -= configuration.WorldWidth;


                }
            }

            foreach (var s in allEnemyShots)
            {

                if (s.position.X < ViewPort.X - (configuration.WorldWidth >> 1))
                {
                    s.position.X += configuration.WorldWidth;


                }
                else if (s.position.X > ViewPort.X + (configuration.WorldWidth >> 1))
                {
                    s.position.X -= configuration.WorldWidth;


                }
            }

            foreach (var s in notKilledHumans)
            {

                if (s.position.X < ViewPort.X - (configuration.WorldWidth >> 1))
                {
                    s.position.X += configuration.WorldWidth;


                }
                else if (s.position.X > ViewPort.X + (configuration.WorldWidth >> 1))
                {
                    s.position.X -= configuration.WorldWidth;


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
            scoreBeginLevel = score;
            ++configuration.Level;
            // EnemyDestroyer.clearDestroyer();
            switch (difficulty)
            {
                case Dificulty.Easy:
                    configuration.NoHumans = notKilledHumans.Count + Game1.random.Next(3) + 3;
                    configuration.NoPredator += (Game1.random.Next(3) + 2);
                    configuration.NoCollector += (Game1.random.Next(3) + 2);
                    if (configuration.Level > 9)
                    {
                        enemyTargetting = true;
                    }
                    if (configuration.Level > 3)
                    {
                        this.configuration.EnemyShotSpeed = SHOTSPEED_NORMAL;
                    }
                    if (configuration.Level > 14)
                    {
                        this.configuration.EnemyShotSpeed = SHOTSPEED_FAST;
                    }
                    if (configuration.Level > 2)
                    {
                        configuration.NoMine += (Game1.random.Next(3) + 1);
                    }
                    ;
                    break;
                case Dificulty.Medium:
                    configuration.NoHumans = notKilledHumans.Count + Game1.random.Next(4) + 2;
                    configuration.NoPredator += (Game1.random.Next(2) + 3);
                    configuration.NoCollector += (Game1.random.Next(2) + 3);
                    if (configuration.Level > 5)
                    {
                        enemyTargetting = true;
                    }
                    if (configuration.Level > 2)
                    {
                        this.configuration.EnemyShotSpeed = SHOTSPEED_NORMAL;
                    }
                    if (configuration.Level > 9)
                    {
                        this.configuration.EnemyShotSpeed = SHOTSPEED_FAST;
                    }

                    if (configuration.Level > 1)
                    {
                        configuration.NoMine += (Game1.random.Next(4) + 2);
                    }
                    break;
                case Dificulty.Hard:
                    configuration.NoHumans = notKilledHumans.Count + Game1.random.Next(5) + 1;
                    configuration.NoPredator += (Game1.random.Next(1) + 4);
                    configuration.NoCollector += (Game1.random.Next(1) + 4);
                    if (configuration.Level > 3)
                    {
                        this.configuration.EnemyShotSpeed = SHOTSPEED_FAST;
                    }
                    if (configuration.Level > 1)
                    {
                        configuration.NoMine += (Game1.random.Next(2) + 4);
                    }
                    break;
            }
            CreateNewEnemys(true);


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

            foreach (var s in allEnemyShots)
            {
                s.Draw(gameTime);
            }

            foreach (var h in notKilledHumans)
            {
                h.Draw(gameTime);
            }
            foreach (var h in notDestroyedEnemys)
            {
                h.Draw(gameTime);
            }

            destroyer.Draw(gameTime);

            input.Draw(gameTime);

            hud.Draw(gameTime);


            batch.End();
        }

        public void DoneLoading()
        {

        }



        public Tombstone GetTombstone()
        {
            Tombstone t = new GameScene.Tombstone();
            t.level = configuration.Level;
            t.difficulty = difficulty;
            t.EnemyShotSpeed = configuration.EnemyShotSpeed;
            t.enemyTargetting = enemyTargetting;
            t.noCollector = configuration.NoCollector;
            t.noHumans = configuration.NoHumans;
            t.noMine = configuration.NoMine;
            t.noPredetor = configuration.NoPredator;
            t.score = scoreBeginLevel;

            return t;
        }

        public struct Tombstone
        {
            public int level;
            public Dificulty difficulty;
            public float EnemyShotSpeed;
            public bool enemyTargetting;
            public int noCollector;
            public int noHumans;
            public int noPredetor;
            public int score;
            public int noMine;
        }





    }
}
