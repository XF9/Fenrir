using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Fenrir.Src.Helper;
using Fenrir.Src.Helper.UI;

namespace Fenrir.Src.LoadingScreen
{
    /// <summary>
    /// basic loading screen
    /// </summary>
    abstract class LoadingScreen
    {
        public LoadingScreen() { }

        /// <summary>
        /// load all the resources
        /// </summary>
        public virtual void Load() { }

        /// <summary>
        /// Draw the loading screen
        /// </summary>
        public void Draw() { }
    }
}
