using Fenrir.Src.Helper.UI;
using Fenrir.Src.Helper.UI.Alignment;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fenrir.Src.InGame.Components.HUD
{
    /// <summary>
    /// A basic mockup of an infopanel, aligned to the top-right corner
    /// </summary>
    abstract class InfoPanel : Group
    {
        public InfoPanel()
            : base(Horizontal.Right, Vertical.Top)
        { }
    }
}
