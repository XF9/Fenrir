using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Fenrir.Src.Helper;
using Fenrir.Src.Helper.UI;
using Fenrir.Src.Helper.UI.Alignment;

namespace Fenrir.Src.Menu
{
    class Header : Group
    {
        public Header()
            : base(Horizontal.Center, Vertical.Top)
        {
            Image header = new Image(DataIdentifier.textureMainMenuTitle, Horizontal.Center, Vertical.Top, new Microsoft.Xna.Framework.Vector2(0, 0));

            this.AddUiElement(new Microsoft.Xna.Framework.Vector2(0,0), header);

            this.ResetPosition(new Microsoft.Xna.Framework.Vector2(0, 50));
        }
    }
}
