using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mitgard.Resistance.Scene;
using Microsoft.Xna.Framework.Graphics;
using Midgard.Resistance;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Mitgard.Resistance.Sprite
{
    public class AbstractEnemy : Sprite
    {
        public static Texture2D explosion;
        public static SoundEffect bam;

        private bool explosionLoaded = false;

        public static readonly Animation EXPLOAD = new Animation(Point.Zero, 5, 4, 64, 64);

        public AbstractEnemy(String name, GameScene scene)
            : base(name, scene)
        {
            position = new Vector2(Game1.random.Next(GameScene.WORLD_WIDTH), -30-Game1.random.Next(100));
        }
        public bool Dead { get; set; }

        public override void Initilize()
        {
            base.Initilize();
            if (!explosionLoaded)
            {
                Game1.instance.LoadContent(@"Animation\ExplosionTiledsSmall", (Texture2D t) => explosion = t);
                Game1.instance.LoadContent(@"Sound\blast", (SoundEffect t) => bam = t);
                
                explosionLoaded = true;
            }
        }


        public  virtual void Destroy()
        {
            Dead = true;
            image = explosion;
            currentAnimation = EXPLOAD;
            currentAnimationFrame = 0;
            origion = new Vector2(23, 23);
            position += new Vector2(-64 >> 2, -64 >> 2);
            bam.Play();
        }



        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (currentAnimation == EXPLOAD)
            {
                ++currentAnimationFrame;
                if (currentAnimationFrame > currentAnimation.Length)
                {
                    Visible = false;
                }
            }
        }
    }
}
