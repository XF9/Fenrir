using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fenrir.Src.InGame.ControlModeHandler
{
    /// <summary>
    /// Interface for a basic handler
    /// </summary>
    interface IModeHandler
    {
        /// <summary>
        /// Handles the Update of the state
        /// </summary>
        /// <param name="hoverPoint">the tile the current curser hovers over</param>
        void Handle(Microsoft.Xna.Framework.Point hoverPoint);

        /// <summary>
        /// Activates the mode für the given Entity if needed
        /// </summary>
        /// <param name="activator"></param>
        void Activate(Entities.Entity activator);

        void Deactivate();

        /// <summary>
        /// the draw call if needed
        /// </summary>
        void DrawHelper();

    }
}
