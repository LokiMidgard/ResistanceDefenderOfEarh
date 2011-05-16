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
using Mitgard.Resistance.Components;
using Microsoft.Xna.Framework.Graphics;
using Midgard.Resistance;
using Mitgard.Resistance.Scene;
using Microsoft.Xna.Framework;

namespace Mitgard.Resistance.Sprite
{
    class Hud : IDrawableComponent
    {
        Texture2D point;
        Texture2D bombIcon;
        Texture2D bombEmptyIcon;

        int rardarHeight = 96;
        int radarWidth = 800;

        private int live;
        private int maxLive;

        private String score = "";

        GameScene scene;

        Dictionary<Vector2, Color> radarDots = new Dictionary<Vector2, Color>();

        public Hud(GameScene scene)
        {
            this.scene = scene;
        }


        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {

            maxLive = 300;
            live = scene.player.lifePoints * 300 / scene.configuration.Player.Lifepoints;
            score = scene.score.ToString();
            Vector2 scalirungsvector = new Vector2((float)radarWidth / (float)scene.configuration.WorldWidth, (float)rardarHeight / (float)scene.configuration.WorldHeight);
            radarDots.Clear();

            Vector2 playerVector = scene.player.position * scalirungsvector;

            Vector2 mittelVector = new Vector2(this.radarWidth / 2, rardarHeight / 2);

            Vector2 deltaVector = mittelVector - playerVector;

            deltaVector *= new Vector2(1, 0);

            foreach (var item in scene.notDestroyedEnemys)
            {

                Vector2 v = item.position * scalirungsvector;
                radarDots[v + deltaVector] = Color.Violet;
            }

            foreach (var shot in from s in scene.allEnemyShots where s.Visible select s)
            {
                radarDots[shot.position * scalirungsvector + deltaVector] = Color.WhiteSmoke;
            }

            foreach (var item in scene.notKilledHumans)
            {

                Vector2 v = item.position * scalirungsvector;
                radarDots[v + deltaVector] = item.IsCaptured ? Color.Red : Color.Green;

            }

            Vector2 destroyserVector = scene.destroyer.position * scalirungsvector;
            radarDots[destroyserVector + deltaVector] = Color.Magenta;


            radarDots[playerVector + deltaVector] = Color.Yellow;




        }

        public void Initilize()
        {
            Game1.instance.QueuLoadContent("Point", (Texture2D t) => point = t);
            Game1.instance.QueuLoadContent("bombIcon", (Texture2D t) => bombIcon = t);
            Game1.instance.QueuLoadContent("bombEmptyIcon", (Texture2D t) => bombEmptyIcon = t);
        }

        public void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {

            Color c = new Color(15, 15, 15, 100);

            var batch = Game1.instance.spriteBatch;

            batch.Draw(point, new Rectangle(0, 0, radarWidth, rardarHeight), c);

            foreach (var item in radarDots)
            {
                batch.Draw(point, item.Key, item.Value);
            }


            batch.Draw(point, new Rectangle(4, 480 - maxLive - 6, 22, maxLive + 2), c);
            batch.Draw(point, new Rectangle(5, 480 - maxLive + (maxLive - live) - 5, 20, live), Color.Green);
            batch.DrawString(Game1.instance.font, score, new Vector2(5, 5), Color.Green);

        }

        public int DrawOrder
        {
            get;
            set;
        }

        public event EventHandler<EventArgs> DrawOrderChanged;

        public bool Visible
        {
            get;
            set;
        }

        public event EventHandler<EventArgs> VisibleChanged;
    }
}
