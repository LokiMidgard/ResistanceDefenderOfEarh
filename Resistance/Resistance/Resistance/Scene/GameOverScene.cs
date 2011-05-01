using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Midgard.Resistance;
using Microsoft.Xna.Framework;

namespace Mitgard.Resistance.Scene
{
    class GameOverScene : IScene
    {
        Texture2D texture;
        public void Initilize()
        {
            Game1.instance.LoadContent(@"Menue\ResTitelWP7_ohneGuy", (Texture2D t) => texture = t);
        }

        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
        }

        public void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {

            Game1.instance.spriteBatch.Begin();
            Game1.instance.spriteBatch.Draw(texture, Vector2.Zero, Color.White);
            Game1.instance.spriteBatch.End();
        }

        public void DoneLoading()
        {
        }
    }
}
