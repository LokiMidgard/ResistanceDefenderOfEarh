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
    class EnemyMine : AbstractEnemy
    {

        private static Microsoft.Xna.Framework.Graphics.Texture2D image;

        public override Microsoft.Xna.Framework.Graphics.Texture2D Image
        {
            get { return image; }
            set { image = value; }
        }

        public static readonly Animation FLY = new Animation(Point.Zero, 3, 3, 32, 32);


        double frameTime;
        const double animationSpeed = 0.05f;


        private int direction;
        private const int DIRECTION_UP = 0;
        private const int DIRECTION_DOWN = 1;
        private const int DIRECTION_LEFT = 2;
        private const int DIRECTION_RIGHT = 3;
        private const int DIRECTION_UP_LEFT = 4;
        private const int DIRECTION_UP_RIGHT = 5;
        private const int DIRECTION_DOWN_LEFT = 6;
        private const int DIRECTION_DOWN_RIGHT = 7;

        public EnemyMine(GameScene scene)
            : base(@"Animation\Enemy3", scene)
        {
            collisonRec = new Rectangle(-16, -16, 32, 32);
        }

        public override void Initilize()
        {
            base.Initilize();
            CurrentAnimation = FLY;
            origion = new Vector2(16, 16);

        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
            if (Dead)
            {
                return;
            }

            frameTime += gameTime.ElapsedGameTime.TotalSeconds;

            while (frameTime > animationSpeed)
            {
                ++currentAnimationFrame;
                if (currentAnimationFrame >= 8)
                {
                    currentAnimationFrame = 0;
                }

                frameTime -= animationSpeed;
            }


            Player player = scene.player;

            Vector2 movment = new Vector2();

            if ((player.position - position).LengthSquared() < 350 * 350)
            {
                // Spieler rammen
                Vector2 target = player.position - position;
                target.Normalize();
                movment += target;

            }
            else
            {
                // Zufaellige bewegung
                int newDirection = Game1.random.Next(512);
                if (newDirection < 8)
                {
                    direction = newDirection;
                }
                if (position.Y < 0)
                {
                    direction = DIRECTION_DOWN;
                }
                else if (position.Y > scene.configuration.WorldHeight - 32 - 5)
                {
                    direction = DIRECTION_UP;
                }
                switch (direction)
                {
                    case DIRECTION_UP:
                        movment += new Vector2(0, -4);
                        break;
                    case DIRECTION_DOWN:
                        movment += new Vector2(0, 4);
                        break;
                    case DIRECTION_LEFT:
                        movment += new Vector2(-4, 0);
                        break;
                    case DIRECTION_RIGHT:
                        movment += new Vector2(4, 0);
                        break;
                    case DIRECTION_UP_LEFT:
                        movment += new Vector2(-3, -1);
                        break;
                    case DIRECTION_UP_RIGHT:
                        movment += new Vector2(3, -1);
                        break;
                    case DIRECTION_DOWN_LEFT:
                        movment += new Vector2(-3, 1);
                        break;
                    case DIRECTION_DOWN_RIGHT:
                        movment += new Vector2(3, 1);
                        break;
                }
                movment.Normalize();
            }

            position += movment * scene.configuration.Mine.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (ColideWith(player))
            {
                player.Hit();
                base.Destroy(); // sterben ohne punkte
            }


        }


        public override void Destroy()
        {
            scene.score += 75;
            base.Destroy();
        }


    }
}
