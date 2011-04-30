using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Mitgard.Resistance.Scene
{
    interface IScene
    {

        void Initilize();

        void Update(GameTime gameTime);

        void Draw(GameTime gameTime);

        void DoneLoading();
    }
}