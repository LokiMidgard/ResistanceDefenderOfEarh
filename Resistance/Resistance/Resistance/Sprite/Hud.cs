using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mitgard.Resistance.Components;
using Microsoft.Xna.Framework.Graphics;
using Midgard.Resistance;

namespace Mitgard.Resistance.Sprite
{
    class Hud : IDrawableComponent
    {
        Texture2D point;

        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public void Initilize()
        {
            Game1.instance.LoadContent("Point", (Texture2D t) => point = t);
        }

        public void Draw(Microsoft.Xna.Framework.GameTime gameTime)
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
    }
}
