using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fenrir.Src.InGame.Entities.Items
{
    interface Item
    {
        /// <summary>
        /// Deploys the item inworld
        /// </summary>
        void Deploy();

        void Update();

        void Draw();
    }
}
