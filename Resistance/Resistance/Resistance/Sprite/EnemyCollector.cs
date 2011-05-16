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

namespace Mitgard.Resistance.Sprite
{
    public class EnemyCollector : AbstractEnemy
    {

        private static Microsoft.Xna.Framework.Graphics.Texture2D image;

        public override Microsoft.Xna.Framework.Graphics.Texture2D Image
        {
            get { return image; }
            set { image = value; }
        }

        public static readonly Animation FLY = new Animation(Point.Zero, 2, 2, 32, 32);

        public EnemyCollector(GameScene scene)
            : base(@"Animation\Enemy2", scene)
        {
            CurrentAnimation = FLY;
            origion = new Vector2(16, 32);
            collisonRec = new Rectangle(-16, -32, 32, 32);
        }


        public Human target;
        public override void Initilize()
        {
            base.Initilize();
            target = null;
            CurrentAnimation = FLY;


        }

        double frameTime;
        const double animationSpeed = 0.05f;

        protected override void AnimationChanged()
        {
            origion = new Vector2(CurrentAnimation.frameWidth / 2, CurrentAnimation.frameHeight);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);

            Vector2 movement = new Vector2();

            if (Dead)
            {
                return;
            }

            if (target == null)
            {
                if (Game1.random.Next(5) == 0)
                    searchTarget();
            }
            else if (target.isCapturedBy == this)
            {
                movement += new Vector2(0, -1);

                if (position.Y < -50)
                {
                    target.Die();
                    target = null;
                }
            }
            else if (target.isCapturedBy != null)
            {
                if (Game1.random.Next(5) == 0)
                    searchTarget();
            }
            else
            {
                var sqrDistance = (position - target.position).LengthSquared();
                if (sqrDistance <= 16)
                {

                    target.isCapturedBy = this;
                }
                else
                {

                    var direction = (target.position - position);

                    direction.Normalize();
                    movement += direction;
                }
            }

            position += movement * scene.configuration.Collector.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;


            frameTime += gameTime.ElapsedGameTime.TotalSeconds;

            while (frameTime > animationSpeed)
            {
                ++currentAnimationFrame;
                if (currentAnimationFrame >= CurrentAnimation.Length)
                {
                    currentAnimationFrame = 0;
                }

                frameTime -= animationSpeed;
            }

        }

        private void searchTarget()
        {
            var v = scene.notKilledHumans;

            var newV = (from x in v where !x.IsCaptured orderby (position - x.position).LengthSquared() select x);
            target = newV.FirstOrDefault();

        }


        public override void Destroy()
        {
            if (target != null && target.isCapturedBy == this)
            {
                target.isCapturedBy = null;
            }
            scene.score += 75;
            base.Destroy();
        }

    }
}
