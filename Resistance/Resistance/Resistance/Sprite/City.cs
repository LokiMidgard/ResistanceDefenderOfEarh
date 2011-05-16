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
using Mitgard.Resistance.Scene;
using Midgard.Resistance;
using Microsoft.Xna.Framework;

namespace Mitgard.Resistance.Sprite
{
    class City : IDrawableComponent
    {
        static Vector2 originCity1;
        static Vector2 originCity2;
        static Vector2 originCity3;

        public float paralxSpeed;
        public float scalewidth;
        City lowerCity;
        City higherCity;


        static Texture2D city1;
        static Texture2D city2;
        static Texture2D city3;

        static bool loaded;




        public void Initilize()
        {
            if (!loaded)
            {
                Game1.instance.QueuLoadContent("city1", (Texture2D t) =>
                {
                    city1 = t;
                    originCity1 = new Vector2(city1.Bounds.Width / 2, city1.Bounds.Height);
                });
                Game1.instance.QueuLoadContent("city2", (Texture2D t) =>
                {
                    city2 = t;
                    originCity2 = new Vector2(city2.Bounds.Width / 2, city2.Bounds.Height);
                });
                Game1.instance.QueuLoadContent("city3", (Texture2D t) =>
                {
                    city3 = t;
                    originCity3 = new Vector2(city3.Bounds.Width / 2, city3.Bounds.Height);
                });
                loaded = true;
                if (lowerCity != null && higherCity != null)
                {
                    lowerCity.Initilize();
                    higherCity.Initilize();
                }
            }
        }

        public void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            switch (image)
            {
                case CityNumber.City1:
                    Game1.instance.spriteBatch.Draw(city1, Position - scene.ViewPort, null, Color.White, 0f, originCity1, 1f, SpriteEffects.None, 0f);
                    break;
                case CityNumber.City2:
                    Game1.instance.spriteBatch.Draw(city2, Position - scene.ViewPort, null, Color.White, 0f, originCity2, 1f, SpriteEffects.None, 0f);
                    break;
                case CityNumber.City3:
                    Game1.instance.spriteBatch.Draw(city3, Position - scene.ViewPort, null, Color.White, 0f, originCity3, 1f, SpriteEffects.None, 0f);
                    break;

            }
            if (lowerCity != null && higherCity != null)
            {
                lowerCity.Draw(gameTime);
                higherCity.Draw(gameTime);
            }
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







        public City(CityNumber image, float paralaxSpeed, GameScene scene)
        {
            // TODO: Complete member initialization
            this.image = CityNumber.None;
            this.paralxSpeed = paralaxSpeed;
            this.scene = scene;
            lowerCity = new City(image, scene);
            higherCity = new City(image, scene);
            scalewidth = ((paralxSpeed * (float)GameScene.VIEWPORT_WIDTH + scene.configuration.WorldWidth * 0.35f));

            OriginalPosition = new Vector2(Game1.random.Next((int)(scene.configuration.WorldWidth - scalewidth * 2)) + scalewidth + GameScene.VIEWPORT_WIDTH, scene.configuration.WorldHeight - 10f - (1 - paralxSpeed) * 10f);
            Visible = true;
            lowerCity.Visible = true;
            higherCity.Visible = true;
        }

        public City(CityNumber image, GameScene scene)
        {
            // TODO: Complete member initialization
            OriginalPosition = new Vector2(Game1.random.Next((int)(scene.configuration.WorldWidth - scalewidth)), scene.configuration.WorldHeight - 10f - (1 - paralxSpeed) * 70f);
            this.image = image;
            this.scene = scene;
        }

        /**
         * Erzeugt eine Stadt mit einem der Zufälligen 3 Bilder
         * @return
         */
        public static City create(GameScene scene)
        {
            CityNumber image = CityNumber.None;

            int r = Game1.random.Next(3);
            switch (r)
            {

                case 0:
                    image = CityNumber.City1;
                    break;
                case 1:
                    image = CityNumber.City2;
                    break;
                case 2:
                    image = CityNumber.City3;
                    break;
            }

            return new City(image, (float)(Game1.random.NextDouble() * 0.3 + 0.2), scene);
        }



        Vector2 position;
        private CityNumber image;
        private int p;
        private GameScene scene;

        public Vector2 OriginalPosition { get; set; }

        public Vector2 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
                if (lowerCity != null && higherCity != null)
                {
                    lowerCity.Position = new Vector2(position.X - scalewidth, position.Y);
                    higherCity.Position = new Vector2(position.X + scalewidth, position.Y);
                }
            }
        }





        public void Update(GameTime gameTime)
        {
        }

        public enum CityNumber
        {
            None,
            City1,
            City2,
            City3
        }
    }
}
