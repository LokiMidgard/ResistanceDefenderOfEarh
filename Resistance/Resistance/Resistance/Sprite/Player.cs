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

        private static Microsoft.Xna.Framework.Graphics.Texture2D image;

        public override Microsoft.Xna.Framework.Graphics.Texture2D Image
        {
            get { return image; }
            set { image = value; }
        }

        const int SHOT_COUNT = 10;
        public Shot[] allShots;

        System.Collections.Generic.Dictionary<int, bool> indicis = new Dictionary<int, bool>();

        SoundEffect shoot;

        Bomb bomb;

        double frameTime;

        const double animationSpeed = 0.05f;

        const float SPEED = 64f;

        public int lifePoints = 5;

        public Vector2 movment;

        public Player(GameScene scene)
            : base(@"Animation\SmallShipTiles", scene)
        {
            bomb = new Bomb(scene);
            position = new Vector2(scene.configuration.WorldWidth / 2, scene.configuration.WorldHeight / 2);
            origion = new Vector2(24, 12);
            collisonRec = new Rectangle(-24, -12, 48, 24);
            allShots = new Shot[SHOT_COUNT];

            for (int i = 0; i < allShots.Length; i++)
            {
                allShots[i] = new Shot(this);
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
            movment *= SPEED;
            if (input.Fire == AbstractInput.Type.Press && CurrentAnimation != TURN_LEFT && CurrentAnimation != TURN_RIGHT)
            {
                Fire(movment.X);
            }
            if (input.Bomb == AbstractInput.Type.Press && !bomb.Visible && CurrentAnimation != TURN_LEFT && CurrentAnimation != TURN_RIGHT)
            {
                bomb.boom();
            }
            position += movment * (float)gameTime.ElapsedGameTime.TotalSeconds;

            for (int i = 0; i < allShots.Length; i++)
            {
                allShots[i].Update(gameTime);
                if (!allShots[i].Visible)
                    indicis[i] = true;
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

        public override void Initilize()
        {
            base.Initilize();
            CurrentAnimation = FLY_RIGHT;
            bomb.Initilize();
            Game1.instance.QueuLoadContent(@"Sound\shot2", (SoundEffect s) => shoot = s);

            foreach (var shot in allShots)
            {
                shot.Initilize();
            }
        }

        public static readonly Animation FLY_RIGHT = new Animation(Point.Zero, 1, 1, 48, 24);
        public static readonly Animation FLY_Left = new Animation(new Point(0, 3 * 24), 1, 1, 48, 24);
        public static readonly Animation TURN_RIGHT = new Animation(new Point(0, 3 * 24), 6, 3, 48, 24);
        public static readonly Animation TURN_LEFT = new Animation(Point.Zero, 6, 3, 48, 24);

        public class Shot : Sprite
        {

            private static Microsoft.Xna.Framework.Graphics.Texture2D image;

            public override Microsoft.Xna.Framework.Graphics.Texture2D Image
            {
                get { return image; }
                set { image = value; }
            }

            public const float SPEED = 720;
            public const double SHOT_LIFETIME = 2;
            public Direction direction;
            private double lifetime;
            public float speed;

            double frameTime;

            const double animationSpeed = 0.05f;


            Player player;

            private readonly Animation CREATE = new Animation(Point.Zero, 4, 2, 160, 8);
            private readonly Animation FLY = new Animation(new Point(0, 8), 1, 1, 160, 8);
            private readonly Animation DIE = new Animation(new Point(0, 8), 4, 2, 160, 8);


            public Shot(Player player)
                : base(@"Animation\FireBlastTiles", player.scene)
            {
                this.player = player;
                CurrentAnimation = FLY;
            }

            public override void Initilize()
            {
                base.Initilize();
                Visible = false;
            }

            protected override void AnimationChanged()
            {
                //Auskommentiert, da die Collision rectangls zu sehr mit der Grafik verknüpft sind
                //base.AnimationChanged();
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




            private void Die()
            {
                CurrentAnimation = DIE;
                currentAnimationFrame = 0;
                //int frame = getFrame();
                //int seqLength = getFrameSequenceLength();
                //setFrameSequence(StaticFields.getSpriteDesinger().MainFireBlastDie);
                //setFrame(Math.max(0, getFrameSequenceLength() - (seqLength - frame)));
                //lifetime = Integer.MAX_VALUE;
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


            public override void Draw(GameTime gameTime)
            {
                if (Visible)
                    base.Draw(gameTime);
            }

        }



        public void Hit()
        {
            --lifePoints;
            if (lifePoints <= 0)
                scene.GameOver();
        }

        public class Bomb : Sprite
        {

            private float destructivXRangeWithScaleOne;
            private float destructivYRangeWithScaleOne;

            static Texture2D tex;


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

            protected override void AnimationChanged()
            {
                base.AnimationChanged();
                destructivXRangeWithScaleOne = CurrentAnimation.frameWidth / 2;
                destructivYRangeWithScaleOne = CurrentAnimation.frameHeight / 2;
            }


            public override void Update(GameTime gameTime)
            {
                if (!Visible)
                    return;
                if (scale >= 2.0f)
                {
                    Visible = false;
                    return;
                }

                foreach (var enemy in scene.notDestroyedEnemys)
                {
                    if (PointWithinExplosion(enemy.position))
                        enemy.Destroy();
                }

                scale = (float)sequence;

                sequence += gameTime.ElapsedGameTime.TotalSeconds;
            }

            public bool PointWithinExplosion(Vector2 pointposition)
            {
                Vector2 realativPosition = pointposition - position;
                if (destructivXRangeWithScaleOne != destructivYRangeWithScaleOne)
                    realativPosition *= new Vector2(1, destructivXRangeWithScaleOne / destructivYRangeWithScaleOne);
                return realativPosition.LengthSquared() <= destructivXRangeWithScaleOne * scale;
            }


            public override void Draw(GameTime gameTime)
            {
                base.Draw(gameTime);
            }



            double sequence = 0;

            public Bomb(GameScene scene)
                : base(@"Animation\BombExplosion", scene)
            {
                CurrentAnimation = new Animation(Point.Zero, 1, 1, 475, 474);
            }

            public void boom()
            {
                sequence = 0;
                position = scene.player.position;
                Visible = true;
            }



        }
    }
}
