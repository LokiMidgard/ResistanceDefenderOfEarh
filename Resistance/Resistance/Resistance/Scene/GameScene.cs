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

        const int WORLD_WIDTH = 4000;
        const int WORLD_Height = 480;




        public Vector2 ViewPort;

        public GameInput input;

        public Player player;

        public AbstractEnemy[] enemys = new AbstractEnemy[0];
        public Humans[] humans = new Humans[0];

        public GameScene()
        {
            player = new Player(this);
            input = new GameInput();

        }

        public void Initilize()
        {
            input.Initilize();

            player.Initilize();
        }

        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            input.Update(gameTime);

            player.Update(gameTime);

            ViewPort = (player.position * new Vector2(1, 0)) - new Vector2(400, 0);
        }

        public void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            var batch = Game1.instance.spriteBatch;
            batch.Begin();
            player.Draw(gameTime);



            input.Draw(gameTime);

            batch.End();
        }

        public void DoneLoading()
        {

        }
    }
}
