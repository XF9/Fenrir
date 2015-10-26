using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Fenrir.Src.InGame.Components;
using Fenrir.Src.InGame.Entities;

using Microsoft.Xna.Framework;

namespace Fenrir.Src.InGame.ControlModeHandler
{
    /// <summary>
    /// Handler to remove markers
    /// </summary>
    class ClearTunnelHandler : IModeHandler
    {
        /// <summary>
        /// the delete marker
        /// </summary>
        Marker helper;

        /// <summary>
        /// the scene to operate in
        /// </summary>
        InGame.Components.Scene scene;

        /// <summary>
        /// Create the handler
        /// </summary>
        /// <param name="Scene"></param>
        public ClearTunnelHandler(InGame.Components.Scene Scene)
        {
            this.helper = new Marker(MarkerType.Info, new Microsoft.Xna.Framework.Point());
            this.scene = Scene;
        }

        /// <summary>
        /// Update this handler
        /// </summary>
        /// <param name="hoverPoint">the curreently hovered block</param>
        public void Handle(Microsoft.Xna.Framework.Point hoverPoint)
        {
            this.helper = new Marker(MarkerType.Info, hoverPoint);

            if (FenrirGame.Instance.Properties.Input.LeftClick && this.scene.Markers.ContainsKey(hoverPoint))
            {
                this.scene.Markers.Remove(hoverPoint);

                foreach (Cave cave in this.scene.Caves)
                    if (cave.CaveBlocksToMine.Contains(hoverPoint))
                    {
                        foreach (Point caveMarker in cave.CaveBlocksToMine)
                            this.scene.Markers.Remove(caveMarker);

                        this.scene.Caves.Remove(cave);
                        break;  // break from loop
                    }
            }

            if (FenrirGame.Instance.Properties.Input.RightClick)
                this.scene.DisposeCurrentModeHandler();
        }

        /// <summary>
        /// Activate this handler
        /// </summary>
        /// <param name="entity">not used - leave null</param>
        public void Activate(Entity entity) { }

        /// <summary>
        /// Deactivate the handler
        /// </summary>
        public void Deactivate() { }

        /// <summary>
        /// Draw the helper marker
        /// </summary>
        public void DrawHelper()
        {
            this.helper.Draw();
        }
    }
}
