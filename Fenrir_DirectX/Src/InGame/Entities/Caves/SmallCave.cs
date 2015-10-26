using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fenrir.Src.InGame.Entities.Caves
{
    class SmallCave : CaveBlueprint
    {
        public SmallCave() : base(2, 2, 2, Fenrir.Src.Helper.DataIdentifier.modelSmallCave) { }
    }
}
