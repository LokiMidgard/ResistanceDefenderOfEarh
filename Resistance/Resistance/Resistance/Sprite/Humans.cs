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
        const double animationSpeed = 0.05f;

        public static readonly Animation WALK = new Animation(Point.Zero, 2, 2, 24, 24);
        public static readonly Animation STAND = new Animation(Point.Zero, 1, 1, 24, 24);


        public Human(GameScene scene)
            : base(@"Animation\NewManTiles", scene)
        {
            origion = new Vector2(12, 0);
            collisonRec = new Rectangle(-12, -12, 24, 24);
            position = new Vector2(Game1.random.Next(GameScene.WORLD_WIDTH), GameScene.WORLD_HEIGHT - 24);
        }

        public override void Initilize()
        {
            if(!Visible)
            position = new Vector2(Game1.random.Next(GameScene.WORLD_WIDTH), GameScene.WORLD_HEIGHT - 24);

            base.Initilize();
            currentAnimation = STAND;
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
            }
            scene.score -= 25;
            screem.Play();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            Vector2 movment = new Vector2();
            if (IsCaptured)
            {
                currentAnimation = STAND;
                direction = Direction.None;
                currentAnimationFrame = 0;
                position = new Vector2(isCapturedBy.position.X, Math.Min(GameScene.WORLD_WIDTH - 24, isCapturedBy.position.Y));
            }
            else if (position.Y < GameScene.WORLD_HEIGHT - 24)
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
                            currentAnimation = STAND;
                            currentAnimationFrame = 0;
                            break;
                        case Direction.Right:
                            currentAnimation = WALK;
                            currentAnimationFrame = 0;
                            spriteEfekt = Microsoft.Xna.Framework.Graphics.SpriteEffects.None;
                            break;
                        case Direction.Left:
                            currentAnimation = WALK;
                            spriteEfekt = Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipHorizontally;
                            currentAnimationFrame = 0;
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
                    if (currentAnimationFrame >= currentAnimation.Length)
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
