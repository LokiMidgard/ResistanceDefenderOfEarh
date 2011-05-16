// Microsoft Reciprocal License (Ms-RL)
// 
// This license governs use of the accompanying software. If you use the software, you accept this license. If you do not accept the license, do not use the software.
// 
// 1. Definitions
// 
// The terms "reproduce," "reproduction," "derivative works," and "distribution" have the same meaning here as under U.S. copyright law.
// A "contribution" is the original software, or any additions or changes to the software.
// A "contributor" is any person that distributes its contribution under this license.
// "Licensed patents" are a contributor's patent claims that read directly on its contribution.
// 
// 2. Grant of Rights
// 
// (A) Copyright Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, each contributor grants you a non-exclusive, worldwide, royalty-free copyright license to reproduce its contribution, prepare derivative works of its contribution, and distribute its contribution or any derivative works that you create.
// (B) Patent Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, each contributor grants you a non-exclusive, worldwide, royalty-free license under its licensed patents to make, have made, use, sell, offer for sale, import, and/or otherwise dispose of its contribution in the software or derivative works of the contribution in the software.
// 
// 3. Conditions and Limitations
// 
// (A) Reciprocal Grants- For any file you distribute that contains code from the software (in source code or binary format), you must provide recipients the source code to that file along with a copy of this license, which license will govern that file. You may license other files that are entirely your own work and do not contain code from the software under any terms you choose.
// (B) No Trademark License- This license does not grant you rights to use any contributors' name, logo, or trademarks.
// (C) If you bring a patent claim against any contributor over patents that you claim are infringed by the software, your patent license from such contributor to the software ends automatically.
// (D) If you distribute any portion of the software, you must retain all copyright, patent, trademark, and attribution notices that are present in the software.
// (E) If you distribute any portion of the software in source code form, you may do so only under this license by including a complete copy of this license with your distribution. If you distribute any portion of the software in compiled or object code form, you may only do so under a license that complies with this license.
// (F) The software is licensed "as-is." You bear the risk of using it. The contributors give no express warranties, guarantees or conditions. You may have additional consumer rights under your local laws which this license cannot change. To the extent permitted under your local laws, the contributors exclude the implied warranties of merchantability, fitness for a particular purpose and non-infringement. 

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
