using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mitgard.Resistance.Scene;

namespace Mitgard.Resistance.Sprite
{
    public class AbstractEnemy : Sprite
    {
        public AbstractEnemy(GameScene scene)
            : base("", scene)
        {
        }
        public bool Dead { get; set; }

        internal void Destroy()
        {
            throw new NotImplementedException();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
