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


namespace Mitgard.Resistance.Sprite
{
    public class EnemyPredator : AbstractEnemy
    {

        private static Microsoft.Xna.Framework.Graphics.Texture2D image;

        public override Microsoft.Xna.Framework.Graphics.Texture2D Image
        {
            get { return image; }
            set { image = value; }
        }

        SoundEffect zap;

        private int direction;
        private const int HOCH = 0;
        private const int RUNTER = 1;
        private const int LINKS = 2;
        private const int RECHTS = 3;
        private const int LINKSOBEN = 4;
        private const int RECHTSOBEN = 5;
        private const int LINKSUNTEN = 6;
        private const int RECHTSUNTEN = 7;


        public Shot[] shots;
        System.Collections.Generic.Dictionary<int, bool> indicis = new Dictionary<int, bool>();




        double frameTime;
        const double animationSpeed = 0.05f;
        const float SPEED = 16;



        private static readonly Animation FLY = new Animation(Point.Zero, 3, 3, 32, 32);

        public EnemyPredator(GameScene scene)
            : base(@"Animation\Enemy1", scene)
        {
            origion = new Microsoft.Xna.Framework.Vector2(16, 16);
            collisonRec = new Rectangle(-16, -16, 32, 32);
            CurrentAnimation = FLY;
            shots = new Shot[scene.configuration.Predator.NumberOfShots];
            for (int i = 0; i < scene.configuration.Predator.NumberOfShots; i++)
            {
                shots[i] = new Shot(scene);
                scene.allEnemyShots.Add(shots[i]);
            }
        }


        public override void Initilize()
        {
            base.Initilize();

            Game1.instance.QueuLoadContent(@"Sound\shot", (SoundEffect t) => zap = t);

            CurrentAnimation = FLY;
            origion = new Vector2(16, 16);



            for (int i = 0; i < scene.configuration.Predator.NumberOfShots; i++)
            {
                shots[i].Initilize();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);


        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            for (int i = 0; i < shots.Length; i++)
            {
                if (!shots[i].Visible)
                    indicis[i] = true;
            }

            if (Dead)
            {
                return;
            }


            frameTime += gameTime.ElapsedGameTime.TotalSeconds;

            while (frameTime > animationSpeed)
            {
                ++currentAnimationFrame;
                if (currentAnimationFrame >= 7)
                {
                    currentAnimationFrame = 0;
                }

                frameTime -= animationSpeed;
            }
            int newDirection = Game1.random.Next(512);
            if (newDirection < 8)
            {
                direction = newDirection;
            }
            if (position.Y < 0)
            {
                direction = RUNTER;
            }
            else if (position.Y > scene.configuration.WorldHeight - CurrentAnimation.frameHeight - 5)
            {
                direction = HOCH;
            }
            Vector2 movment = new Vector2();
            switch (direction)
            {
                case 0:
                    movment += new Vector2(0, -2);
                    break;
                case 1:
                    movment += new Vector2(0, 2);
                    break;
                case 2:
                    movment += new Vector2(-3, 0);
                    break;
                case 3:
                    movment += new Vector2(3, 0);
                    break;
                case 4:
                    movment += new Vector2(-2, -1);
                    break;
                case 5:
                    movment += new Vector2(2, -1);
                    break;
                case 6:
                    movment += new Vector2(-2, 1);
                    break;
                case 7:
                    movment += new Vector2(2, 1);
                    break;

            }

            position += movment * (float)gameTime.ElapsedGameTime.TotalSeconds * SPEED;

            if (Game1.random.Next((3 - (int)scene.difficulty) << 5) < 1)
            {
                fire();
            }
        }

        private void fire()
        {
            if (indicis.Count == 0)
                return;



            Vector2 target;

            Player player = scene.player;

            if (scene.configuration.EnemyTargetting)
            {
                for (float i = 0; i < 6f; i += 0.3f)
                {


                    var mov = player.movment;

                    var newPlayerPosition = player.position + (i * mov);

                    target = newPlayerPosition - position;
                    if (target.LengthSquared() <= scene.configuration.EnemyShotSpeed * i * scene.configuration.EnemyShotSpeed * i)
                        goto targetin;

                }
                return;

            }
            else
                target = player.position - position;
            if (target.LengthSquared() > 400 * 400)
            {
                return;


            }
        targetin:

            float distance = target.Length();




            if (distance < 48)
            {
                return;
            }

            int index = indicis.First().Key;
            indicis.Remove(index);
            Shot s = shots[index];

            target.Normalize();
            target *= scene.configuration.EnemyShotSpeed;
            s.init(position, target, distance + 150);
            zap.Play();
            //TODO: Shoot
            //((Shot) shots.pop()).init(getRefPixelX(), getRefPixelY(), dx, dy, distance + 150);
            //StaticFields.getSound().playSFX("shot", 20);

        }

        public class Shot : Sprite
        {

            private static Microsoft.Xna.Framework.Graphics.Texture2D image;

            public override Microsoft.Xna.Framework.Graphics.Texture2D Image
            {
                get { return image; }
                set { image = value; }
            }

            public static readonly Animation FLY = new Animation(Point.Zero, 2, 2, 8, 8);

            double frameTime;
            const double animationSpeed = 0.05f;

            double livetime = 0;
            /**
             * Maximale Lebenszeit
             */
            double timeToLive;
            Vector2 movment;

            public override void Initilize()
            {
                base.Initilize();
                Visible = false;
            }

            public Shot(GameScene scene)
                : base(@"Animation\newAlienFireBlast", scene)
            {
                CurrentAnimation = FLY;
            }

            public void init(Vector2 position, Vector2 movement, double lifetime)
            {
                timeToLive = lifetime;
                this.livetime = 0;

                this.position = position;
                movement.Normalize();
                movement *= scene.configuration.EnemyShotSpeed;
                this.movment = movement;

                Visible = true;
            }

            public override void Update(GameTime gameTime)
            {
                if (!Visible)
                    return;
                if ((position.Y < 0 && movment.Y <= 0) || (position.Y > scene.configuration.WorldHeight && movment.Y >= 0))
                {
                    die();
                }
                livetime += gameTime.ElapsedGameTime.TotalSeconds;
                if (livetime > timeToLive)
                {
                    die();
                }
                else if (ColideWith(scene.player))
                {
                    scene.player.Hit();
                    die();
                }
                else
                {
                    while (frameTime > animationSpeed)
                    {
                        ++currentAnimationFrame;
                        if (currentAnimationFrame > CurrentAnimation.Length)
                        {
                            currentAnimationFrame = 0;
                        }

                        frameTime -= animationSpeed;
                    }
                    position += movment * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
            }

            private void die()
            {
                Visible = false;

            }
        }

        public override void Destroy()
        {
            scene.score += 75;
            base.Destroy();
        }



    }
}
