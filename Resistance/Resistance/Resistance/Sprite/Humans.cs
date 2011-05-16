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
using Mitgard.Resistance.Scene;
using Microsoft.Xna.Framework;
using Midgard.Resistance;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace Mitgard.Resistance.Sprite
{
    public class Human : Sprite
    {



        private static Texture2D image;

        public override Texture2D Image
        {
            get { return image; }
            set { image = value; }
        }

        public EnemyCollector isCapturedBy;

        private static bool soundLoaded;

        public bool IsCaptured { get { return isCapturedBy != null; } }

        Direction direction;

        public static SoundEffect screem;

        double frameTime;
        const double animationSpeed = 0.2f;

        public static readonly Animation WALK = new Animation(Point.Zero, 2, 2, 24, 24);
        public static readonly Animation STAND = new Animation(Point.Zero, 1, 1, 24, 24);


        public Human(GameScene scene)
            : base(@"Animation\NewManTiles", scene)
        {
            origion = new Vector2(24, 0);
            collisonRec = new Rectangle(-12, 0, 24, 24);
            position = new Vector2(Game1.random.Next(scene.configuration.WorldWidth), scene.configuration.WorldHeight - 24);
        }

        protected override void AnimationChanged()
        {
            base.AnimationChanged();
            origion = new Vector2(CurrentAnimation.frameWidth / 2, 0);
        }

        public override void Initilize()
        {
            if (!Visible)
                position = new Vector2(Game1.random.Next(scene.configuration.WorldWidth), scene.configuration.WorldHeight - 24);

            base.Initilize();
            CurrentAnimation = STAND;
            direction = Direction.None;
            if (!soundLoaded)
            {
                Game1.instance.QueuLoadContent(@"Sound\scream", (SoundEffect s) => screem = s);
                soundLoaded = true;
            }
        }

        internal void Die()
        {
            Visible = false;

            scene.notKilledHumans.Remove(this);
            if (IsCaptured)
            {
                isCapturedBy.target = null;
                isCapturedBy = null;
            }
            scene.score -= 25;
            screem.Play();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            Vector2 movment = new Vector2();
            if (IsCaptured)
            {
                CurrentAnimation = STAND;
                direction = Direction.None;
                currentAnimationFrame = 0;
                position = new Vector2(isCapturedBy.position.X, Math.Min(scene.configuration.WorldWidth - 24, isCapturedBy.position.Y));
            }
            else if (position.Y < scene.configuration.WorldHeight - 24)
            {
                movment += new Vector2(0, 1);
            }
            else
            {
                int newDirection = Game1.random.Next(40);
                if (newDirection < 3 && direction != (Direction)newDirection)
                {
                    direction = (Direction)newDirection;
                    switch (direction)
                    {
                        case Direction.None:
                            CurrentAnimation = STAND;
                            currentAnimationFrame = 0;
                            break;
                        case Direction.Right:
                            CurrentAnimation = WALK;
                            currentAnimationFrame = 1;
                            spriteEfekt = Microsoft.Xna.Framework.Graphics.SpriteEffects.None;
                            break;
                        case Direction.Left:
                            CurrentAnimation = WALK;
                            spriteEfekt = Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipHorizontally;
                            currentAnimationFrame = 1;
                            break;
                    }
                }

                switch (direction)
                {

                    case Direction.Right:
                        movment += new Vector2(1, 0);
                        break;
                    case Direction.Left:
                        movment -= new Vector2(1, 0);
                        break;
                }


                frameTime += gameTime.ElapsedGameTime.TotalSeconds;

                while (frameTime > animationSpeed)
                {
                    ++currentAnimationFrame;
                    if (currentAnimationFrame >= CurrentAnimation.Length)
                    {
                        currentAnimationFrame = 1;
                    }

                    frameTime -= animationSpeed;
                }



            }
            position += (movment * 16 * (float)gameTime.ElapsedGameTime.TotalSeconds);

        }








    }
}
