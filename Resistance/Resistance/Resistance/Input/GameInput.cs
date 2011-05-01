using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Mitgard.Resistance.Sprite;

namespace Mitgard.Resistance.Input
{
   public  class GameInput : AbstractInput, IDrawable
    {

        RectButton[] buttons;

        public GameInput()
            : base(new Rectangle(50, 400, 130, 100), new Rectangle(400, 400, 130, 100),
                    new Rectangle(800 - 80, 460 - 250, 100, 130), new Rectangle(800 - 80, 460 - 100, 100, 130),
                    new Rectangle(200, 400, 180, 100), new Rectangle(800 - 80, 460 - 500, 100, 130))
        {

            buttons = new RectButton[base.recs.Length];

            for (int i = 0; i < recs.Length; i++)
            {
                buttons[i] = new RectButton(recs[i]);
            }
        }

        public Type Left { get { return this[0]; } }
        public Type Right { get { return this[1]; } }
        public Type Up { get { return this[2]; } }
        public Type Down { get { return this[3]; } }
        public Type Fire { get { return this[4]; } }
        public Type Bomb { get { return this[5]; } }


        public override void Initilize()
        {

            foreach (var r in buttons)
            {
                r.Initilize();
            }
        }


        public override void Update(GameTime gametime)
        {
            base.Update(gametime);
            foreach (var item in buttons)
            {
                item.Update(gametime);
            }
        }

        public void Draw(GameTime gameTime)
        {
            for (int i = 0; i < recs.Length; i++)
            {
                buttons[i].Draw(gameTime);
            };
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
