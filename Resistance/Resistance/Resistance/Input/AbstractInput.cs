using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Collections;

namespace Mitgard.Resistance.Input
{
    class AbstractInput       :  Components.IComponent
    {



        Rectangle[] recs;

        BitArray pressed;
        BitArray hold;
        BitArray released;


        public AbstractInput(params Rectangle[] recs)
        {
            this.recs = recs;
            pressed = new BitArray(recs.Length);
            hold = new BitArray(recs.Length);
            released = new BitArray(recs.Length);
        }



        public void Initilize()
        {
           
        }



        public void Update(GameTime gametime)
        {
            BitArray newDirection = GetActualInput();

            pressed = pressed.Xor(newDirection).And(newDirection);
            released = hold.Xor(newDirection).And(newDirection.Not());
            hold = newDirection;
        }

        private BitArray GetActualInput()
        {
            var touchPoints = Microsoft.Xna.Framework.Input.Touch.TouchPanel.GetState();

            BitArray bit = new BitArray(recs.Length);

            for (int i = 0; i < recs.Length; i++)
            {
                foreach (var t in touchPoints)
                {
                    if (recs[i].Contains(new Point((int)t.Position.X, (int)t.Position.Y)))
                        bit[i] = true;
                }
            }

            return bit;
        }





    




    }
}
