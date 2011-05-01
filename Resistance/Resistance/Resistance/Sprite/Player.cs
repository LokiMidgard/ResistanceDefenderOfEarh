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

        const int SHOT_COUNT = 10;
        public Shot[] allShots;

        System.Collections.Generic.Dictionary<int, bool> indicis = new Dictionary<int, bool>();

        SoundEffect shoot;

        double frameTime;

        const double animationSpeed = 0.05f;

        const float SPEED = 64f;

        public int lifePoints = 5;

        public Vector2 movment;

        public Player(GameScene scene)
            : base(@"Animation\SmallShipTiles", scene)
        {
            position = new Vector2(GameScene.WORLD_WIDTH / 2, GameScene.WORLD_HEIGHT / 2);
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

            frameTime += gameTime.ElapsedGameTime.TotalSeconds;

            while (frameTime > animationSpeed)
            {

                if (currentAnimation == TURN_LEFT)
                {
                    ++currentAnimationFrame;
                    if (currentAnimationFrame >= currentAnimation.Length)
                    {
                        currentAnimation = FLY_Left;
                        currentAnimationFrame = 0;
                    }

                }
                else if (currentAnimation == TURN_RIGHT)
                {
                    ++currentAnimationFrame;
                    if (currentAnimationFrame >= currentAnimation.Length)
                    {
                        currentAnimation = FLY_RIGHT;
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
            if (currentAnimation == FLY_RIGHT)
            {
                movment += new Vector2(1, 0);
                if (input.Right == AbstractInput.Type.Hold)
                    movment += new Vector2(2, 0);
                else if (input.Left == AbstractInput.Type.Press)
                    currentAnimation = TURN_LEFT;
            }
            else if (currentAnimation == FLY_Left)
            {
                movment += new Vector2(-1, 0);
                if (input.Left == AbstractInput.Type.Hold)
                    movment += new Vector2(-2, 0);
                else if (input.Right == AbstractInput.Type.Press)
                    currentAnimation = TURN_RIGHT;
            }
            movment *= SPEED;
            if (input.Fire == AbstractInput.Type.Press && currentAnimation != TURN_LEFT && currentAnimation != TURN_RIGHT)
            {
                Fire(movment.X);
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
            base.Draw(gameTime);
        }

        public void Fire(float speed)
        {
            if (indicis.Count == 0)
                return;
            int i = indicis.First().Key;
            indicis.Remove(i);
            Shot s = allShots[i];
            s.Fire(speed, currentAnimation == FLY_Left ? Direction.Left : Direction.Right, position);
            shoot.Play();
        }

        public override void Initilize()
        {
            base.Initilize();
            currentAnimation = FLY_RIGHT;

            Game1.instance.LoadContent(@"Sound\shot2", (SoundEffect s) => shoot = s);

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
                currentAnimation = FLY;
            }

            public override void Initilize()
            {
                base.Initilize();
                Visible = false;
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
                currentAnimation = CREATE;
            }




            private void Die()
            {
                currentAnimation = DIE;
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

                if (lifetime > SHOT_LIFETIME && currentAnimation != DIE)
                {
                    Die();
                }

                frameTime += gameTime.ElapsedGameTime.TotalSeconds;

                while (frameTime > animationSpeed)
                {
                    if (currentAnimation == CREATE)
                    {
                        ++currentAnimationFrame;
                        if (currentAnimationFrame > 5)
                        {
                            currentAnimationFrame = 0;
                            currentAnimation = FLY;
                        }
                    }
                    else if (currentAnimation == DIE)
                    {
                        ++currentAnimationFrame;
                        if (currentAnimationFrame > 6)
                        {
                            Visible = false;
                        }

                    }


                    frameTime -= animationSpeed;
                }


                if (currentAnimation != DIE)
                {

                    var enemys = player.scene.enemys;
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
                    var humans = player.scene.humans;
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
    }
}
