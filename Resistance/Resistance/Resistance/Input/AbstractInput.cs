using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Collections;

namespace Mitgard.Resistance.Input
{
    public abstract class AbstractInput : Components.IComponent
    {



        protected Rectangle[] recs;

        protected BitArray pressed;
        protected BitArray hold;
        protected BitArray released;

        public Type this[int index]
        {
            get
            {
                return (Type)((hold[index] ? 2 : 1) | (released[index] ? 4 : 0) | (pressed[index] ? 8 : 0));
            }
        }


        public AbstractInput(params Rectangle[] recs)
        {
            this.recs = recs;
            pressed = new BitArray(recs.Length);
            hold = new BitArray(recs.Length);
            released = new BitArray(recs.Length);
        }



        public abstract void Initilize();


        public virtual  void Update(GameTime gametime)
        {
            BitArray newDirection = GetActualInput();

            pressed = new BitArray(hold).Xor(newDirection).And(newDirection);
            released = new BitArray(hold.Xor(newDirection).And(new BitArray(newDirection).Not()));
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





        [Flags]
        public enum Type
        {
            None = 1,
            Hold = 2,
            Release = 4 | 1,
            Press = 8 | 2
        }




    }
}
