// Microsoft Reciprocal License (Ms-RL)
// 
// This license governs use of the accompanying software. If you use the software, you accept this license. If you do not accept the license, do not use the software.
// 
// 1. Definitions
// 
// The terms "reproduce," "reproduction," "derivative works," and "distribution" have the same meaning here as under U.S. copyright law.
// A "contribution" is the original software, or any additions or changes to the software.
// A "contributor" is any person that distributes its contribution under this license.
// "Licensed patents" are a contributor's patent claims that read directly on its contribution.
// 
// 2. Grant of Rights
// 
// (A) Copyright Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, each contributor grants you a non-exclusive, worldwide, royalty-free copyright license to reproduce its contribution, prepare derivative works of its contribution, and distribute its contribution or any derivative works that you create.
// (B) Patent Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, each contributor grants you a non-exclusive, worldwide, royalty-free license under its licensed patents to make, have made, use, sell, offer for sale, import, and/or otherwise dispose of its contribution in the software or derivative works of the contribution in the software.
// 
// 3. Conditions and Limitations
// 
// (A) Reciprocal Grants- For any file you distribute that contains code from the software (in source code or binary format), you must provide recipients the source code to that file along with a copy of this license, which license will govern that file. You may license other files that are entirely your own work and do not contain code from the software under any terms you choose.
// (B) No Trademark License- This license does not grant you rights to use any contributors' name, logo, or trademarks.
// (C) If you bring a patent claim against any contributor over patents that you claim are infringed by the software, your patent license from such contributor to the software ends automatically.
// (D) If you distribute any portion of the software, you must retain all copyright, patent, trademark, and attribution notices that are present in the software.
// (E) If you distribute any portion of the software in source code form, you may do so only under this license by including a complete copy of this license with your distribution. If you distribute any portion of the software in compiled or object code form, you may only do so under a license that complies with this license.
// (F) The software is licensed "as-is." You bear the risk of using it. The contributors give no express warranties, guarantees or conditions. You may have additional consumer rights under your local laws which this license cannot change. To the extent permitted under your local laws, the contributors exclude the implied warranties of merchantability, fitness for a particular purpose and non-infringement. 

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


        public abstract Texture2D Image
        {
            get;
            set;
        }


        public Vector2 position;

        public Vector2 origion;

        public Rectangle collisonRec;

        public GameScene scene;

        private String imageName;

        private Animation currentAnimation;

        public Animation CurrentAnimation
        {
            get { return currentAnimation; }
            set
            {
                currentAnimation = value;
                AnimationChanged();
            }
        }

        protected virtual void AnimationChanged()
        {
            origion = new Vector2(currentAnimation.frameWidth / 2, currentAnimation.frameHeight / 2);
        }

        public int currentAnimationFrame;

        public SpriteEffects spriteEfekt = SpriteEffects.None;


        public Sprite(String imageName, GameScene scene)
        {
            this.imageName = imageName;
            this.scene = scene;
            color = Color.White;
        }

        public virtual void Draw(GameTime gameTime)
        {
            if (Visible)
                Game1.instance.spriteBatch.Draw(Image, position - scene.ViewPort, CurrentAnimation[currentAnimationFrame], color, 0f, origion, scale, spriteEfekt, 0f);

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
        public Vector2 scale = Vector2.One;
        public Color color;

        public abstract void Update(GameTime gameTime);

        public virtual void Initilize()
        {
            Visible = true;
            if (Image == null)
                Game1.instance.QueuLoadContent(imageName, (Texture2D t) => Image = t);
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
