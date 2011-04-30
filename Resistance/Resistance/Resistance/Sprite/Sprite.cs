using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mitgard.Resistance.Components;
using Microsoft.Xna.Framework;

namespace Mitgard.Resistance.Sprite
{
    public abstract class Sprite : IDrawableComponent
    {


        public void Draw(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public int DrawOrder
        {
            get { throw new NotImplementedException(); }
        }

        public event EventHandler<EventArgs> DrawOrderChanged;

        public bool Visible
        {
            get { throw new NotImplementedException(); }
        }

        public event EventHandler<EventArgs> VisibleChanged;

        public void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public void Initilize()
        {
            throw new NotImplementedException();
        }
    }
}
