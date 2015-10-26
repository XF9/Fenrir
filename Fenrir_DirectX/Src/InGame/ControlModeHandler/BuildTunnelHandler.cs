using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Fenrir.Src.InGame.Entities;
using Fenrir.Src.InGame.Components;

namespace Fenrir.Src.InGame.ControlModeHandler
{
    /// <summary>
    /// Handler for building a tunnel
    /// </summary>
    class BuildTunnelHandler : IModeHandler
    {

        /// <summary>
        /// the scene to build in
        /// </summary>
        private Components.Scene scene;

        /// <summary>
        /// temporary marker
        /// </summary>
        private Dictionary<Point, Marker> tmpMarker;

        /// <summary>
        /// the starting point for the building
        /// </summary>
        private Point? dragStart;

        /// <summary>
        /// Crewate it
        /// </summary>
        /// <param name="scene"></param>
        public BuildTunnelHandler(Components.Scene scene)
        {
            this.scene = scene;
            this.tmpMarker = new Dictionary<Point, Marker>();
        }

        //TODO: can be moved to scene?
        /// <summary>
        /// checks if a block can be mined
        /// </summary>
        /// <param name="pos">the block position</param>
        /// <returns>true or false</returns>
        private bool canBeMined(Point pos){
            if (pos.Y % 2 == 0 || pos.X % 2 == 0)
                if (this.scene.GetBlockState(pos) != TileState.None)
                {
                    if (this.scene.GetBlock(pos).IsDestructable)
                        return true;
                }
                else
                    return true;
            return false;
        }

        /// <summary>
        /// add temporary marker
        /// </summary>
        /// <param name="pos"></param>
        private void addTmpMarker(Point pos)
        {
            if (!this.tmpMarker.ContainsKey(pos))
                if (this.canBeMined(pos) && this.scene.GetBlockDepth(pos) <= 0)
                    this.tmpMarker.Add(pos, new Marker(MarkerType.Tunnel, pos));
                else
                    this.tmpMarker.Add(pos, new Marker(MarkerType.Error, pos));
        }

        /// <summary>
        /// mark all blocks for mining
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        private void mark(Point from, Point to)
        {
            Boolean buildHorizontal = true;
            Boolean buildVertical = true;

            if (from.Y % 2 != 0)
                buildHorizontal = false;

            if (from.X % 2 != 0)
                buildVertical = false;

            if (buildHorizontal && buildVertical)
                buildHorizontal = Math.Abs(from.Y - to.Y) < Math.Abs(from.X - to.X);

            Point pos = new Point();
            int small;
            int large;

            if (buildHorizontal)
            {
                pos.Y = from.Y;
                small = (to.X < from.X) ? (int)to.X : from.X;
                large = (to.X > from.X) ? (int)to.X : from.X;
            }
            else
            {
                pos.X = from.X;
                small = (to.Y < from.Y) ? (int)to.Y : from.Y;
                large = (to.Y > from.Y) ? (int)to.Y : from.Y;
            }

            for (; small <= large; small++)
            {
                if (buildHorizontal)
                    pos.X = small;
                else
                    pos.Y = small;

                this.addTmpMarker(pos);
            }
        }

        /// <summary>
        /// how to activate this one
        /// </summary>
        /// <param name="entity">unused - leave null</param>
        public void Activate(Entity entity) { }

        /// <summary>
        /// Update this handler
        /// </summary>
        /// <param name="hoverPoint">the currently hovered block</param>
        public void Handle(Microsoft.Xna.Framework.Point hoverPoint)
        {
            this.tmpMarker.Clear();

            if (FenrirGame.Instance.Properties.Input.RightClick)
            {
                this.scene.DisposeCurrentModeHandler();
                return;
            }

            if (this.scene.IsAboveBuildLevel(hoverPoint))
                hoverPoint = new Microsoft.Xna.Framework.Point(hoverPoint.X, this.scene.Properties.StartingAreaBlocksBottom);

            if (FenrirGame.Instance.Properties.Input.LeftClick && this.canBeMined(hoverPoint))
            {
                // add a marker für the currently hovered position
                this.addTmpMarker(hoverPoint);

                if (!this.scene.Markers.ContainsKey(hoverPoint))
                    this.scene.Markers.Add(hoverPoint, new Marker(MarkerType.Tunnel, hoverPoint));
            }else if (FenrirGame.Instance.Properties.Input.LeftDrag)
            {
                FenrirGame.Instance.InGame.Camera.blockCameraMovement = true;
                if (!this.dragStart.HasValue)
                {
                    this.dragStart = hoverPoint;
                }
                else
                {
                    this.mark(this.dragStart.Value, hoverPoint);
                }
            }
            else if (this.dragStart.HasValue)
            {
                this.mark(this.dragStart.Value, hoverPoint);
                this.dragStart = null;

                foreach (KeyValuePair<Point, Marker> marker in this.tmpMarker)
                    if (!this.scene.Markers.ContainsKey(marker.Key) && marker.Value.Type == MarkerType.Tunnel)
                        this.scene.Markers.Add(marker.Key, marker.Value);
            }
            else
            {
                // add a marker für the currently hovered position
                this.addTmpMarker(hoverPoint);
            }
        }

        /// <summary>
        /// draw the helper marker
        /// </summary>
        public void DrawHelper()
        {
            foreach (Marker maker in this.tmpMarker.Values)
                maker.Draw();
        }

        /// <summary>
        /// deactivate this handler
        /// </summary>
        public void Deactivate() { }
    }
}
