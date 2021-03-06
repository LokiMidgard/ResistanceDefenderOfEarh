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
using Microsoft.Xna.Framework;
using Mitgard.Resistance.Input;
using Mitgard.Resistance.Scene;
using Mitgard.Resistance.Components;
using Microsoft.Xna.Framework.Graphics;
using Midgard.Resistance;
using Microsoft.Xna.Framework.Audio;

namespace Mitgard.Resistance.Sprite
{

    public class Player : Sprite
    {

        const double animationSpeed = 0.05f;
        public static readonly Animation FLY_Left = new Animation(new Point(0, 3 * 24), 1, 1, 48, 24);
        public static readonly Animation FLY_RIGHT = new Animation(Point.Zero, 1, 1, 48, 24);
        public static readonly Animation TURN_LEFT = new Animation(Point.Zero, 6, 3, 48, 24);
        public static readonly Animation TURN_RIGHT = new Animation(new Point(0, 3 * 24), 6, 3, 48, 24);
        SoundEffect shoot;

        System.Collections.Generic.Dictionary<int, bool> indicis = new Dictionary<int, bool>();
        public Shot[] allShots;
       public Bomb bomb;
        double frameTime;
        public Vector2 movment;


        private static Microsoft.Xna.Framework.Graphics.Texture2D image;
        public int lifePoints;
        public override Microsoft.Xna.Framework.Graphics.Texture2D Image
        {
            get { return image; }
            set { image = value; }
        }


        public Player(GameScene scene)
            : base(@"Animation\SmallShipTiles", scene)
        {
            bomb = new Bomb(scene);
            position = new Vector2(scene.configuration.WorldWidth / 2, scene.configuration.WorldHeight / 2);
            origion = new Vector2(24, 12);
            collisonRec = new Rectangle(-24, -12, 48, 24);
            allShots = new Shot[scene.configuration.Player.ShotCount];

            for (int i = 0; i < allShots.Length; i++)
            {
                allShots[i] = new Shot(this);
            }

        }

        public override void Draw(GameTime gameTime)
        {
            foreach (var shot in allShots)
            {
                shot.Draw(gameTime);
            }
            bomb.Draw(gameTime);
            base.Draw(gameTime);
        }

        public void Fire(float speed)
        {
            if (indicis.Count == 0)
                return;
            int i = indicis.First().Key;
            indicis.Remove(i);
            Shot s = allShots[i];
            s.Fire(speed, CurrentAnimation == FLY_Left ? Direction.Left : Direction.Right, position);
            shoot.Play();
        }

        public void Hit()
        {
            --lifePoints;
            if (lifePoints <= 0)
                scene.GameOver();
        }

        public override void Initilize()
        {
            base.Initilize();
            CurrentAnimation = FLY_RIGHT;
            bomb.Initilize();
            lifePoints = scene.configuration.Player.Lifepoints;

            Game1.instance.QueuLoadContent(@"Sound\shot2", (SoundEffect s) => shoot = s);

            foreach (var shot in allShots)
            {
                shot.Initilize();
            }
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {

            var input = scene.input;
            bomb.Update(gameTime);
            frameTime += gameTime.ElapsedGameTime.TotalSeconds;

            while (frameTime > animationSpeed)
            {

                if (CurrentAnimation == TURN_LEFT)
                {
                    ++currentAnimationFrame;
                    if (currentAnimationFrame >= CurrentAnimation.Length)
                    {
                        CurrentAnimation = FLY_Left;
                        currentAnimationFrame = 0;
                    }

                }
                else if (CurrentAnimation == TURN_RIGHT)
                {
                    ++currentAnimationFrame;
                    if (currentAnimationFrame >= CurrentAnimation.Length)
                    {
                        CurrentAnimation = FLY_RIGHT;
                        currentAnimationFrame = 0;
                    }
                }

                frameTime -= animationSpeed;
            }

            movment = new Vector2();

            if (input.Down == AbstractInput.Type.Hold)
                movment += new Vector2(0, 2);
            if (input.Up == AbstractInput.Type.Hold)
                movment += new Vector2(0, -2);
            if (CurrentAnimation == FLY_RIGHT)
            {
                movment += new Vector2(1, 0);
                if (input.Right == AbstractInput.Type.Hold)
                    movment += new Vector2(2, 0);
                else if (input.Left == AbstractInput.Type.Press)
                    CurrentAnimation = TURN_LEFT;
            }
            else if (CurrentAnimation == FLY_Left)
            {
                movment += new Vector2(-1, 0);
                if (input.Left == AbstractInput.Type.Hold)
                    movment += new Vector2(-2, 0);
                else if (input.Right == AbstractInput.Type.Press)
                    CurrentAnimation = TURN_RIGHT;
            }
            movment *= scene.configuration.Player.Speed;
            if (input.Fire == AbstractInput.Type.Press && CurrentAnimation != TURN_LEFT && CurrentAnimation != TURN_RIGHT)
            {
                Fire(movment.X);
            }
            if (input.Bomb == AbstractInput.Type.Press && !bomb.Visible && CurrentAnimation != TURN_LEFT && CurrentAnimation != TURN_RIGHT)
            {
                bomb.Boom();
            }
            position += movment * (float)gameTime.ElapsedGameTime.TotalSeconds;

            for (int i = 0; i < allShots.Length; i++)
            {
                allShots[i].Update(gameTime);
                if (!allShots[i].Visible)
                    indicis[i] = true;
            }

        }

        public class Bomb : Sprite
        {

            private Vector2 destructivRangeWithScaleOne;


            private Vector2 scalePerSeccond;

            public override Texture2D Image
            {
                get
                {
                    return tex;
                }
                set
                {
                    tex = value;
                }
            }

            static Texture2D tex;
            private Vector2 scaleWithMaxRadius;

            public Bomb(GameScene scene)
                : base(@"Animation\BombExplosion", scene)
            {
                CurrentAnimation = new Animation(Point.Zero, 1, 1, 475, 474);
            }

            protected override void AnimationChanged()
            {
                base.AnimationChanged();
                destructivRangeWithScaleOne = new Vector2(CurrentAnimation.frameWidth / 2, CurrentAnimation.frameHeight / 2);
                scaleWithMaxRadius = new Vector2(scene.configuration.Player.MaxBombSize.X / destructivRangeWithScaleOne.X, scene.configuration.Player.MaxBombSize.Y / destructivRangeWithScaleOne.Y);
                scalePerSeccond = scaleWithMaxRadius / scene.configuration.Player.TimeTillMaxBombSize;
            }

            public void Boom()
            {
                position = scene.player.position;
                Visible = true;
                scale = Vector2.Zero;
            }

            public override void Initilize()
            {
                base.Initilize();
                Visible = false;
            }

            public bool PointWithinExplosion(Vector2 pointposition)
            {
                Vector2 realativPosition = pointposition - position;

                Vector2 radians = destructivRangeWithScaleOne * scale;

                if (radians.X != radians.Y)
                    realativPosition *= new Vector2(1, radians.X / radians.Y);
                return realativPosition.LengthSquared() <= radians.X * radians.X;
            }

            public override void Update(GameTime gameTime)
            {
                if (!Visible)
                    return;
                if (scale.X >= scaleWithMaxRadius.X && scale.X >= scaleWithMaxRadius.X)
                {
                    Visible = false;
                    return;
                }

                float transparentPercent = 1f - scale.X / scaleWithMaxRadius.X;

                color = new Color(transparentPercent, transparentPercent, transparentPercent, transparentPercent);

                foreach (var enemy in scene.notDestroyedEnemys)
                {
                    if (PointWithinExplosion(enemy.position))
                        enemy.Destroy();
                }


                scale += scalePerSeccond * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        public class Shot : Sprite
        {

            const double animationSpeed = 0.05f;
            public const double SHOT_LIFETIME = 2;
            public const float SPEED = 720;
            private readonly Animation CREATE = new Animation(Point.Zero, 4, 2, 160, 8);
            private readonly Animation DIE = new Animation(new Point(0, 8), 4, 2, 160, 8);
            public Direction direction;
            private readonly Animation FLY = new Animation(new Point(0, 8), 1, 1, 160, 8);
            double frameTime;

            private static Microsoft.Xna.Framework.Graphics.Texture2D image;
            public override Microsoft.Xna.Framework.Graphics.Texture2D Image
            {
                get { return image; }
                set { image = value; }
            }

            private double lifetime;
            Player player;
            public float speed;

            public Shot(Player player)
                : base(@"Animation\FireBlastTiles", player.scene)
            {
                this.player = player;
                CurrentAnimation = FLY;
            }

            protected override void AnimationChanged()
            {
                //Auskommentiert, da die Collision rectangls zu sehr mit der Grafik verkn�pft sind
                //base.AnimationChanged();
            }

            private void Die()
            {
                CurrentAnimation = DIE;
                currentAnimationFrame = 0;
            }

            public override void Draw(GameTime gameTime)
            {
                if (Visible)
                    base.Draw(gameTime);
            }

            public void Fire(float playerSpeed, Direction playerDirection, Vector2 position)
            {
                this.position = position;
                this.direction = playerDirection;
                switch (playerDirection)
                {
                    case Direction.Left:
                        speed = playerSpeed - SPEED;
                        origion = new Vector2(0, 4);
                        spriteEfekt = SpriteEffects.FlipHorizontally;
                        collisonRec = new Rectangle(-2, -2, 8, 4);
                        break;
                    case Direction.Right:
                        speed = playerSpeed + SPEED;
                        origion = new Vector2(160, 4);
                        collisonRec = new Rectangle(-6, -2, 8, 4);
                        spriteEfekt = SpriteEffects.None;
                        break;
                }
                Visible = true;
                currentAnimationFrame = 0;
                lifetime = 0;
                CurrentAnimation = CREATE;
            }

            public override void Initilize()
            {
                base.Initilize();
                Visible = false;
            }

            public override void Update(GameTime gameTime)
            {
                if (!Visible)
                    return;

                Vector2 tmp = player.position;

                position += new Vector2((float)(speed * gameTime.ElapsedGameTime.TotalSeconds), 0);

                lifetime += gameTime.ElapsedGameTime.TotalSeconds;

                if (lifetime > SHOT_LIFETIME && CurrentAnimation != DIE)
                {
                    Die();
                }

                frameTime += gameTime.ElapsedGameTime.TotalSeconds;

                while (frameTime > animationSpeed)
                {
                    if (CurrentAnimation == CREATE)
                    {
                        ++currentAnimationFrame;
                        if (currentAnimationFrame > 5)
                        {
                            currentAnimationFrame = 0;
                            CurrentAnimation = FLY;
                        }
                    }
                    else if (CurrentAnimation == DIE)
                    {
                        ++currentAnimationFrame;
                        if (currentAnimationFrame > 6)
                        {
                            Visible = false;
                        }

                    }


                    frameTime -= animationSpeed;
                }


                if (CurrentAnimation != DIE)
                {

                    var enemys = player.scene.notDestroyedEnemys.Union(new AbstractEnemy[] { scene.destroyer });
                    foreach (var e in enemys)
                    {
                        if (ColideWith(e) && !e.Dead
                            && ((direction == Direction.Right && e.position.X >= tmp.X)
                                || (direction == Direction.Left && e.position.X <= tmp.X)))
                        {
                            e.Destroy();
                            Die();
                            break;
                        }
                    }
                    var humans = player.scene.notKilledHumans;
                    foreach (var h in humans)
                    {
                        if (ColideWith(h)
                            && ((direction == Direction.Right && h.position.X >= tmp.X)
                                || (direction == Direction.Left && h.position.X <= tmp.X)))
                        {
                            h.Die();
                            Die();
                            break;
                        }
                    }
                }
            }
        }
    }
}
