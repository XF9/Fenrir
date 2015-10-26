using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fenrir.Src.Helper.UI
{
    interface Element
    {
        /// <summary>
        /// Draws the ui element
        /// </summary>
        void Draw();

        /// <summary>
        /// Updates the ui element
        /// </summary>
        void Update();

        Microsoft.Xna.Framework.Vector2 Position { get; set; }
    }
}
