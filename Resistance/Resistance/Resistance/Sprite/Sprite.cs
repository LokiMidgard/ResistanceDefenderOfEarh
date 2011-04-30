using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mitgard.Resistance.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Midgard.Resistance;

namespace Mitgard.Resistance.Sprite
{
    public abstract class Sprite : IDrawableComponent
    {

        public Texture2D image;

        public Vector2 position;

        public Vector2 origion;


        private String imageName;

        public Animation currentAnnimation;

        public int currentAnimationFrame;


        public Sprite(String imageName)
        {
            this.imageName = imageName;
        }

        public virtual void Draw(GameTime gameTime)
        {
            Game1.instance.spriteBatch.Draw(image, position, currentAnnimation[currentAnimationFrame], Color.White, 0f, origion, 1f, currentAnnimation.spriteEfekt, 0f);
        }

        public int DrawOrder
        {
            get;
            set;
        }

        public event EventHandler<EventArgs> DrawOrderChanged;

        public bool Visible
        {
            get;
            set;
        }

        public event EventHandler<EventArgs> VisibleChanged;

        public abstract void Update(GameTime gameTime);

        public virtual void Initilize()
        {
            Game1.instance.LoadContent(imageName, (Texture2D t) => image = t);
        }

        public struct Animation
        {
            public Point leftTop;
            public int width;
            public int height;
            public int frameWidth;
            public int frameHeight;

            public SpriteEffects spriteEfekt;

            public Animation(Point leftTop, int width, int heigth, int frameWidth, int frameHeighr)
            {
                this.leftTop = leftTop;
                this.width = width;
                this.height = heigth;
                this.frameWidth = frameWidth;
                this.frameHeight = frameHeighr;
                spriteEfekt = SpriteEffects.None;
            }
            int Length { get { return width * height; } }

            public Rectangle this[int index]
            {
                get { return new Rectangle((index % width) * frameWidth, (index / width) * frameHeight, frameWidth, frameHeight); }
            }

        }
    }
}
