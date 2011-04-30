using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Mitgard.Resistance.Components
{
    interface IComponent   
    {
        void Update(GameTime gameTime);

        void Initilize();


    }
}
