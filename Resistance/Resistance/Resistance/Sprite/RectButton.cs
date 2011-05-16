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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Mitgard.Resistance.Components;
using Midgard.Resistance;


namespace Mitgard.Resistance.Sprite
{



    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class RectButton : IDrawableComponent
    {
        private Rectangle rec;

        private Texture2D tex;

        SpriteBatch batch = Game1.instance.spriteBatch;

        Color c;

        public RectButton(Rectangle rec)
        {
            this.rec = rec;

        }






        public void Draw(GameTime gameTime)
        {


            var color = new Color(255, 255, 255, 80);
            color = c;

            batch.Draw(tex, new Rectangle(rec.Left, rec.Top, 5, 5), new Rectangle(0, 0, 5, 5), color);
            batch.Draw(tex, new Rectangle(rec.Right - 5, rec.Top, 5, 5), new Rectangle(6, 0, 5, 5), color);
            batch.Draw(tex, new Rectangle(rec.Left, rec.Bottom - 5, 5, 5), new Rectangle(0, 6, 5, 5), color);
            batch.Draw(tex, new Rectangle(rec.Right - 5, rec.Bottom - 5, 5, 5), new Rectangle(6, 6, 5, 5), color);

            batch.Draw(tex, new Rectangle(rec.Left + 5, rec.Top, rec.Width - 10, 5), new Rectangle(5, 0, 1, 5), color);
            batch.Draw(tex, new Rectangle(rec.Left + 5, rec.Bottom - 5, rec.Width - 10, 5), new Rectangle(5, 6, 1, 5), color);

            batch.Draw(tex, new Rectangle(rec.Left, rec.Top + 5, 5, rec.Height - 10), new Rectangle(0, 5, 5, 1), color);
            batch.Draw(tex, new Rectangle(rec.Right - 5, rec.Top + 5, 5, rec.Height - 10), new Rectangle(6, 5, 5, 1), color);

            batch.Draw(tex, new Rectangle(rec.Left + 5, rec.Top + 5, rec.Width - 10, rec.Height - 10), new Rectangle(5, 5, 1, 1), color);


        }



        public void Update(GameTime gameTime)
        {
            var touchPoints = Microsoft.Xna.Framework.Input.Touch.TouchPanel.GetState();

                    c = Color.White;

            foreach (var t in touchPoints)
            {
                if (rec.Contains(new Point((int)t.Position.X, (int)t.Position.Y)))
                    c = Color.Beige;
                
            }
        }

        public void Initilize()
        {
            Game1.instance.QueuLoadContent("ButtonOutline", (Texture2D t) => tex = t);
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
