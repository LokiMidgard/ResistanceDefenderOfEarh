using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mitgard.Resistance.Components;
using Microsoft.Xna.Framework.Graphics;
using Midgard.Resistance;
using Microsoft.Xna.Framework;
using Mitgard.Resistance.Scene;
using Mitgard.Resistance.Sprite;

namespace Mitgard.Resistance.LevelBackground
{
    class DesertBackground : IDrawableComponent
    {


        Texture2D[] cloud = new Texture2D[5];
        Vector2 cloudCoordinats;


        Texture2D gradient;

        Texture2D[] hill = new Texture2D[4];
        Vector2[] hillCoordinats = new Vector2[12];

        Texture2D[] mountain = new Texture2D[2];
        Vector2[] mountainCoordinats = new Vector2[6];

        Texture2D star;
        Vector2[] starCoordinats = new Vector2[4];


        GameScene scene;

        City[] cytiys = new City[5];

        public DesertBackground(GameScene scene)
        {
            this.scene = scene;

            for (int i = 0; i < cytiys.Length; ++i)
            {
                cytiys[i] = City.create(scene);
            }
        }

        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {



            ////        for (int i = 0; i < citys.size(); i++)
            ////        {
            ////            City s = (City) citys.elementAt(i);
            ////            if (s.getRefPixelX() < viewPortX - StaticFileds.WORLD_WIDTH / 2)
            ////            {
            ////                s.setLocalPosition(s.getX_location() + StaticFileds.WORLD_WIDTH, s.getY_location());
            ////            }
            ////            else if (s.getRefPixelX() > viewPortX + StaticFileds.WORLD_WIDTH / 2)
            ////            {
            ////                s.setLocalPosition(s.getX_location() - StaticFileds.WORLD_WIDTH, s.getY_location());
            ////            }
            ////        }

            //        int viewportWidth = StaticFields.getCanvas().getGameWidth();


            //        int viewportHeight = StaticFields.getCanvas().getGameHeight();

            //        Level1Desinger desinger = StaticFields.getLevel1Desingenr();

            //        //  viewPortX = Math.min(Math.max(viewPortX, 0 + viewportWidth / 2), StaticFileds.WORLD_WIDTH - viewportWidth / 2);
            //float  viewPortY = Math.min(Math.max(viewPortY, 0 + (viewportHeight >>> 1)), StaticFields.WORLD_HEIGHT - (viewportHeight >>> 1));


            //        try {

            float tmp1 = scene.ViewPort.X * 0.9f; // viewport *0,9;


            float pre = (scene.ViewPort.Y + (GameScene.VIEWPORT_HEIGHT >> 1) - 240);


            //            int tmp2 = (((pre << 4) + (pre << 3) + (pre << 2) + pre) >> 5) + 350;

            starCoordinats[0] = new Vector2(tmp1 + -400, (scene.ViewPort.Y + (GameScene.VIEWPORT_HEIGHT >> 1) - 480) * 9 / 10 + 180);
            starCoordinats[1] = new Vector2(tmp1 + 0, (scene.ViewPort.Y + (GameScene.VIEWPORT_HEIGHT >> 1) - 480) * 9 / 10 + 180);
            starCoordinats[2] = new Vector2(tmp1 + 400, (scene.ViewPort.Y + (GameScene.VIEWPORT_HEIGHT >> 1) - 480) * 9 / 10 + 180);
            starCoordinats[3] = new Vector2(tmp1 + 800, (scene.ViewPort.Y + (GameScene.VIEWPORT_HEIGHT >> 1) - 480) * 9 / 10 + 180);



            //            sky.setPosition((viewPortX - (viewportWidth >>> 1) + 0), 0);
            ////            desinger.getMountain_1().setPosition((viewPortX - viewportWidth / 2 + 0) * 6 / 10, (viewPortY + viewportHeight / 2 - 480) * 6 / 10 + 350);
            ////            desinger.getMountain_2().setPosition((viewPortX - viewportWidth / 2) * 6 / 10 + 1000, (viewPortY + viewportHeight / 2 - 480) * 6 / 10 + 350);
            ////            desinger.getMountain_3().setPosition((viewPortX - viewportWidth / 2) * 6 / 10 + 2000, (viewPortY + viewportHeight / 2 - 480) * 6 / 10 + 350);
            ////            desinger.getMountain_4().setPosition((viewPortX - viewportWidth / 2) * 6 / 10 + 3000, (viewPortY + viewportHeight / 2 - 480) * 6 / 10 + 350);

            tmp1 = scene.ViewPort.X * 0.6f; // viewport*6
            float tmp2 = (((pre * 16) + (pre * 4) + pre) / 32) + 350;

            mountainCoordinats[0] = new Vector2(tmp1 + -1600, tmp2);
            mountainCoordinats[1] = new Vector2(tmp1 + -800, tmp2);
            mountainCoordinats[2] = new Vector2(tmp1 + 0, tmp2);
            mountainCoordinats[3] = new Vector2(tmp1 + 800, tmp2);
            mountainCoordinats[4] = new Vector2(tmp1 + 1600, tmp2);
            mountainCoordinats[5] = new Vector2(tmp1 + 2400, tmp2);


            tmp1 = scene.ViewPort.X * 0.4f;

            for (int i = 0; i < cytiys.Length; ++i)
            {

                var c = cytiys[i];
                c.Position = new Vector2((scene.ViewPort.X * c.paralxSpeed) + c.OriginalPosition.X, ((pre) * c.paralxSpeed) + c.OriginalPosition.Y);


            }

            tmp1 = scene.ViewPort.X * 0.2f;
            tmp2 = (((pre + pre) * 51) / 1024) + 416;
            //            // tmp2= ((pre<<1)/10+416);
            //            // System.err.println(tmp2);

            hillCoordinats[0] = new Vector2((tmp1 + -3200), tmp2);
            hillCoordinats[1] = new Vector2((tmp1 + -2400), tmp2);
            hillCoordinats[2] = new Vector2((tmp1 + -1600), tmp2);
            hillCoordinats[3] = new Vector2((tmp1 + -800), tmp2);
            hillCoordinats[4] = new Vector2((tmp1 + 0), tmp2);
            hillCoordinats[5] = new Vector2((tmp1 + 800), tmp2);
            hillCoordinats[6] = new Vector2((tmp1 + 1600), tmp2);
            hillCoordinats[7] = new Vector2((tmp1 + 2400), tmp2);
            hillCoordinats[8] = new Vector2((tmp1 + 3200), tmp2);
            hillCoordinats[9] = new Vector2((tmp1 + 4000), tmp2);
            hillCoordinats[10] = new Vector2((tmp1 + 4800), tmp2);
            hillCoordinats[11] = new Vector2((tmp1 + 5200), tmp2);




            //        } catch (IOException ex) {
            //            ex.printStackTrace();


            //        }
            //        this.background.setViewWindow(viewPortX - (viewportWidth >>> 1), viewPortY - (viewportHeight >>> 1), viewportWidth, viewportHeight);


            //        this.enemysManager.setViewWindow(viewPortX - (viewportWidth >>> 1), viewPortY - (viewportHeight >>> 1), viewportWidth, viewportHeight);


            //        this.humanManager.setViewWindow(viewPortX - (viewportWidth >>> 1), viewPortY - (viewportHeight >>> 1), viewportWidth, viewportHeight);


            //        this.playerManager.setViewWindow(viewPortX - (viewportWidth >>> 1), viewPortY - (viewportHeight >>> 1), viewportWidth, viewportHeight);

            //             * 
            //             */

        }

        public void Initilize()
        {
            for (int i = 0; i < cytiys.Length; i++)
            {
                int j = i;
                cytiys[i].Initilize();
            }

            for (int i = 0; i < cloud.Length; i++)
            {
                int j = i;
                Game1.instance.LoadContent("cloud" + (i + 1), (Texture2D t) => cloud[j] = t);
            }

            for (int i = 0; i < hill.Length; i++)
            {
                int j = i;
                Game1.instance.LoadContent("hills" + (j + 1), (Texture2D t) => hill[j] = t);
            }

            for (int i = 0; i < mountain.Length; i++)
            {
                int j = i;
                Game1.instance.LoadContent("mountains" + (i + 1), (Texture2D t) => mountain[j] = t);
            }
            Game1.instance.LoadContent("gradient", (Texture2D t) => gradient = t);
            Game1.instance.LoadContent("stars", (Texture2D t) => star = t);

        }

        public void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            var batch = Game1.instance.spriteBatch;

            for (int i = 0; i < starCoordinats.Length; i++)
            {
                batch.Draw(star, starCoordinats[i] - scene.ViewPort, Color.White);
            }
            batch.Draw(gradient, new Rectangle(0, 0, 800, 480), Color.White);

            for (int i = 0; i < mountainCoordinats.Length; i += 2)
            {
                batch.Draw(mountain[0], mountainCoordinats[i] - scene.ViewPort, Color.White);
                batch.Draw(mountain[1], mountainCoordinats[i + 1] - scene.ViewPort, Color.White);
            }


            for (int i = 0; i < cytiys.Length; i += 2)
            {
                cytiys[i].Draw(gameTime);
            }


            for (int i = 0; i < 3; ++i)
            {
                for (int j = 0; j < 4; j++)
                {

                    batch.Draw(hill[j], hillCoordinats[i * 4 + j] - scene.ViewPort, Color.White);

                }
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
    }
}
