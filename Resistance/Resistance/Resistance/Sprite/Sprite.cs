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


        public Sprite(String imageName)
        {
            this.imageName = imageName;
        }

        public void Draw(GameTime gameTime)
        {
            Game1.instance.spriteBatch.Draw(image, null, Color.White);
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

        public void Update(GameTime gameTime)
        {

        }

        public void Initilize()
        {
            Game1.instance.LoadContent(imageName, (Texture2D t) => image = t);
        }

        public struct Animation
        {
            Point leftTop;
            int width;
            int height;
            int frameWidth;
            int frameHeight;
            public Animation(Point leftTop, int width, int heigth, int frameWidth, int frameHeighr)
            {
                this.leftTop = leftTop;
                this.width = width;
                this.height = heigth;
                this.frameWidth = frameWidth;
                this.frameHeight = frameHeighr;
            }
            int Length { get { return width * height; } }

            public Rectangle this[int index]
            {
                get { return new Rectangle((index % width) * frameWidth, (index / width) * frameHeight, frameWidth, frameHeight); }
            }

        }
    }
}
