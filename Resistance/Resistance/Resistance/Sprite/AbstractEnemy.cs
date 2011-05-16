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
using Microsoft.Xna.Framework.Graphics;
using Midgard.Resistance;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Mitgard.Resistance.Sprite
{
    public abstract class AbstractEnemy : Sprite
    {
        public static Texture2D explosion;
        public static SoundEffect bam;

        private bool explosionLoaded = false;

        public static readonly Animation EXPLOAD = new Animation(Point.Zero, 5, 4, 64, 64);

        public AbstractEnemy(String name, GameScene scene)
            : base(name, scene)
        {
        }
        public bool Dead { get; set; }

        public override void Initilize()
        {
            base.Initilize();
            Dead = false;
            position = new Vector2(Game1.random.Next(scene.configuration.WorldWidth), -30 - Game1.random.Next(100));
            if (!explosionLoaded)
            {
                Game1.instance.QueuLoadContent(@"Animation\ExplosionTiledsSmall", (Texture2D t) => explosion = t);
                Game1.instance.QueuLoadContent(@"Sound\blast", (SoundEffect t) => bam = t);

                explosionLoaded = true;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if (Dead)
            {
                var oldImage = Image;
                Image = explosion;

                base.Draw(gameTime);

                Image = oldImage;
            }
            else
                base.Draw(gameTime);


        }

        public virtual void Destroy()
        {
            if (Dead)
                return;
            Dead = true;

            CurrentAnimation = EXPLOAD;
            currentAnimationFrame = 0;
            origion = new Vector2(23, 23);
            position += new Vector2(-64 >> 2, -64 >> 2);
            bam.Play();
        }



        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (Visible && CurrentAnimation == EXPLOAD)
            {
                ++currentAnimationFrame;
                if (currentAnimationFrame > CurrentAnimation.Length)
                {
                    Visible = false;
                }
            }
        }
    }
}
