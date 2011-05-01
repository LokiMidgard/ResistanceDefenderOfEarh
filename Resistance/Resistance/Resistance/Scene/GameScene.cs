using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mitgard.Resistance.Input;
using Mitgard.Resistance.Sprite;
using Midgard.Resistance;
using Microsoft.Xna.Framework;

namespace Mitgard.Resistance.Scene
{
    public class GameScene : IScene
    {

        public const int WORLD_WIDTH = 4000;
        public const int WORLD_HEIGHT = 480;




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
        }

        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
           

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

            ViewPort = (player.position * new Vector2(1, 0)) - new Vector2(400, 0);
        }

        public void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            var batch = Game1.instance.spriteBatch;
            batch.Begin();
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

            batch.End();
        }

        public void DoneLoading()
        {

        }
    }
}
