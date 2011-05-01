using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mitgard.Resistance.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Midgard.Resistance;
using Mitgard.Resistance.Scene;

namespace Mitgard.Resistance.Sprite
{
    public abstract class Sprite : IDrawableComponent
    {

        public Texture2D image;

        public Vector2 position;

        public Vector2 origion;

        public Rectangle collisonRec;

        public GameScene scene;

        private String imageName;

        public Animation currentAnnimation;

        public int currentAnimationFrame;

        public SpriteEffects spriteEfekt = SpriteEffects.None;




        public Sprite(String imageName, GameScene scene)
        {
            this.imageName = imageName;
            this.scene = scene;
        }

        public virtual void Draw(GameTime gameTime)
        {
            Game1.instance.spriteBatch.Draw(image, position - scene.ViewPort, currentAnnimation[currentAnimationFrame], Color.White, 0f, origion, 1f, spriteEfekt, 0f);
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

        public bool ColideWith(Sprite other)
        {
            var t = collisonRec;
            t.Offset((int)position.X, (int)position.Y);
            var o = other.collisonRec;
            o.Offset((int)other.position.X, (int)other.position.Y);
            return t.Intersects(o);
        }

        public struct Animation
        {
            public Point leftTop;
            public int width;
            public int height;
            public int frameWidth;
            public int frameHeight;




            public Animation(Point leftTop, int width, int heigth, int frameWidth, int frameHeighr)
            {
                this.leftTop = leftTop;
                this.width = width;
                this.height = heigth;
                this.frameWidth = frameWidth;
                this.frameHeight = frameHeighr;

            }
            public int Length { get { return width * height; } }

            public Rectangle this[int index]
            {
                get { return new Rectangle(leftTop.X + (index % width) * frameWidth, leftTop.Y + (index / width) * frameHeight, frameWidth, frameHeight); }
            }

            public static bool operator ==(Animation a1, Animation a2)
            {
                return a1.Equals(a2);
            }

            public static bool operator !=(Animation a1, Animation a2)
            {
                return !a1.Equals(a2);
            }

            // override object.Equals
            public override bool Equals(object obj)
            {
                //       
                // See the full list of guidelines at
                //   http://go.microsoft.com/fwlink/?LinkID=85237  
                // and also the guidance for operator== at
                //   http://go.microsoft.com/fwlink/?LinkId=85238
                //

                if (obj == null || GetType() != obj.GetType())
                {
                    return false;
                }

                if (Object.ReferenceEquals(this, obj))
                    return true;
                if (obj == null)
                    return false;
                Animation other = (Animation)obj;
                if (frameHeight != other.frameHeight)
                    return false;
                if (frameWidth != other.frameWidth)
                    return false;
                if (height != other.height)
                    return false;
                if (leftTop == null)
                {
                    if (other.leftTop != null)
                        return false;
                }
                else if (!leftTop.Equals(other.leftTop))
                    return false;
                if (width != other.width)
                    return false;
                return true;
            }

            // override object.GetHashCode
            public override int GetHashCode()
            {
                int prime = 31;
                int result = 1;
                result = prime * result + frameHeight;
                result = prime * result + frameWidth;
                result = prime * result + height;
                result = prime * result + leftTop.GetHashCode();
                result = prime * result + width;
                return result;
            }

        }
    }

    public enum Direction
    {
        None,
        Left,
        Right   

    }
}
