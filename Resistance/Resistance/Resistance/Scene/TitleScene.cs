using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Midgard.Resistance;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Mitgard.Resistance.Musicplayer;
using Microsoft.Xna.Framework.Input.Touch;

namespace Mitgard.Resistance.Scene
{
    class TitleScene : IScene
    {
        Texture2D background;

        SpriteBatch batch;

        Song techno;

        public void Initilize()
        {
            batch = Game1.instance.spriteBatch;
            Game1.instance.LoadContent(@"Menue\ResTitelWP7", (Texture2D t) => background = t);
            Game1.instance.LoadContent(@"Music\techno", (Song t) => techno = t);
           
        }

        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (TouchPanel.IsGestureAvailable)
            {
                var gesture =TouchPanel.ReadGesture();


                // If the user tapped the screen, we check all buttons to see if they were tapped.
                if (gesture.GestureType == GestureType.Tap)
                {
                    Game1.instance.SwitchToScene(new GameScene());
                }
            }

        }

        public void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            batch.Begin();

            batch.Draw(background, Vector2.Zero, Color.White);
         
            batch.End();
        }

        public void DoneLoading()
        {
            MusicManager.PlaySong(techno);
        }
    }
}
