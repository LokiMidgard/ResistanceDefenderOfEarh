using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mitgard.Resistance.Scene;

namespace Mitgard.Resistance.Sprite
{
 public    class Humans       :Sprite
    {

     public EnemyCollector isCapturedBy;
  
    Direction direction = 0;


        public Humans(GameScene scene):base("",scene)
        {

        }
        internal void Die()
        {
            throw new NotImplementedException();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            throw new NotImplementedException();
        }








     

    
  
    public Human() {
        defineReferencePixel(getWidth() >> 1, 0);
        setRefPixelPosition(StaticFields.getRandom().nextInt(StaticFields.WORLD_WIDTH), StaticFields.WORLD_HEIGHT - getHeight());
        setVisible(true);
    }

    public boolean isCaptured() {
        return (isCapturedBy != null);
    }

    public void die() {
        StaticFields.currentLevel.removeHuman(this);
        if(isCaptured())
        {
            isCapturedBy.target=null;
        }
        StaticFields.score -= 25;
        StaticFields.getSound().playSFX("scream", 50);
    }

    public void tick(int keyState) {
        if (isCapturedBy != null) {
            setFrameSequence(StaticFields.getSpriteDesinger().NewManStand);
            setRefPixelPosition(isCapturedBy.getRefPixelX(), Math.min(
                    StaticFields.WORLD_HEIGHT - getHeight(),
                    isCapturedBy.getY() + isCapturedBy.getHeight()));
        } else if (getRefPixelY() < StaticFields.WORLD_HEIGHT - getHeight()) {
            move(0, 1);
        } else {
            int newDirection = StaticFields.getRandom().nextInt(40);
            if (newDirection < 3 && direction != newDirection) {
                direction = newDirection;
                switch (direction) {
                    case STAND_STILL:
                        setFrameSequence(StaticFields.getSpriteDesinger().NewManStand);
                        break;
                    case WALK_RIGHT:
                        setFrameSequence(StaticFields.getSpriteDesinger().NewManWalk);
                        setTransform(TRANS_NONE);
                        break;
                    case WALK_LEFT:
                        setFrameSequence(StaticFields.getSpriteDesinger().NewManWalk);
                        setTransform(TRANS_MIRROR);
                        break;
                }
            }
            switch (direction) {
                case STAND_STILL:
                    setFrame(0);
                    break;
                case WALK_RIGHT:
                    move(1, 0);
                    nextFrame();
                    break;
                case WALK_LEFT:
                    move(-1, 0);
                    nextFrame();
                    break;
            }

        }
    }

    }
}
