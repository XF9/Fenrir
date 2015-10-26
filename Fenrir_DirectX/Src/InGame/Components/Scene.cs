using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Fenrir.Src.Helper;
using Fenrir.Src.InGame;
using Fenrir.Src.InGame.Entities;
using Fenrir.Src.InGame.Entities.Caves;

namespace Fenrir.Src.InGame.Components
{
    /// <summary>
    /// the curent mode of the scene
    /// </summary>
    enum ModeHandler
    {
        /// <summary>
        /// control a unit
        /// </summary>
        Unit,
        /// <summary>
        /// build a tunnel
        /// </summary>
        BuildTunnel,
        /// <summary>
        /// remove a tunnel to be mined
        /// </summary>
        ClearTunnel,
        /// <summary>
        /// build a small cave
        /// </summary>
        BuildCaveSmall,
        /// <summary>
        /// build a medium cave
        /// </summary>
        BuildCaveMedium,
        /// <summary>
        /// build a large cae
        /// </summary>
        BuildCaveLarge
    }
    /// <summary>
    /// the state of a tile
    /// </summary>
    enum TileState
    {
        None,

        /// <summary>
        /// theres a solid block
        /// </summary>
        Solid,

        /// <summary>
        /// this block can be walked
        /// </summary>
        Walkable,

        /// <summary>
        /// this block can be walked and have noch bottom
        /// </summary>
        Climbable
    }

    /// <summary>
    /// This is the world ..
    /// it contains everything real - from ponies to unicorns
    /// </summary>
    class Scene
    {
        #region variables
        /// <summary>
        /// All Blocks in the system
        /// Array for the depth of each block
        /// </summary>
        private Dictionary<Point,Block>[] blocks;

        /// <summary>
        /// markers for building
        /// </summary>
        private Dictionary<Point, Marker> markers;

        internal Dictionary<Point, Marker> Markers
        {
            get { return markers; }
            private set { markers = value; }
        }

        /// <summary>
        /// all playerunits
        /// </summary>
        private List<Entities.Unit> units;

        internal List<Entities.Unit> Units
        {
            get { return units; }
            private set { units = value; }
        }

        /// <summary>
        /// all buildings
        /// </summary>
        private List<Building> buildings;

        /// <summary>
        /// all caves build and still to build
        /// </summary>
        private List<Cave> caves;

        internal List<Cave> Caves
        {
            get { return caves; }
            private set { caves = value; }
        }

        /// <summary>
        /// the mouse surface to trac the mouse
        /// </summary>
        private Plane mouseSurface;

        /// <summary>
        /// a small cave
        /// </summary>
        private CaveBlueprint smallCave;

        /// <summary>
        /// a medium cave
        /// </summary>
        private CaveBlueprint mediumCave;

        /// <summary>
        /// a large cave
        /// </summary>
        private CaveBlueprint largeCave;

        private SceneProperties properties;
        /// <summary>
        /// fixed properties
        /// </summary>
        internal SceneProperties Properties
        {
            get { return properties; }
            private set { properties = value; }
        }

        /// <summary>
        /// Unitmaster
        /// </summary>
        private ControlModeHandler.UnitHandler unitHandler;

        /// <summary>
        /// Cavemaster
        /// </summary>
        private ControlModeHandler.BuildCaveHandler buildCaveHandler;
        
        /// <summary>
        /// tunnelmaster
        /// </summary>
        private ControlModeHandler.BuildTunnelHandler buildTunnelHandler;
        
        /// <summary>
        /// clearermaster
        /// </summary>
        private ControlModeHandler.ClearTunnelHandler clearTunnelHandler;

        /// <summary>
        /// the current mode of operation
        /// </summary>
        private ControlModeHandler.IModeHandler currentModeHandler;

        #endregion

        /// <summary>
        /// the world
        /// </summary>
        public Scene()
        {
            // load settings and stuff
            this.properties = new SceneProperties();
        }

        /// <summary>
        /// prepares a new world
        /// </summary>
        public void InitializeWorld()
        {
            // initialize variables
            this.blocks = new Dictionary<Point, Block>[this.Properties.MaxBlockDepth];

            for (int i = 0; i < this.Properties.MaxBlockDepth; i++)
                this.blocks[i] = new Dictionary<Point, Block>();

            this.markers = new Dictionary<Point, Marker>();
            this.units = new List<Entities.Unit>();
            this.buildings = new List<Building>();
            this.caves = new List<Cave>();

            this.units.Add(new Entities.Unit(DataIdentifier.modelPlayerUnit, new Vector3(0.5f, 0.5f, 0.5f), new Vector3(0, 4, -1), "Herbert"));
            this.units.Add(new Entities.Unit(DataIdentifier.modelPlayerUnit, new Vector3(0.5f, 0.5f, 0.5f), new Vector3(-2, 4, -1), "Torben"));

            this.buildings.Add(new Building(DataIdentifier.modelBuildingCampfire, new Vector3(2, 1, 1), new Vector3(-4, 4, -3)));

            this.mouseSurface = new Plane(Vector3.UnitZ, 0);

            // generate the world
            this.GenerateNewWorld();

            // generate cave models
            this.smallCave = new SmallCave();
            this.mediumCave = new MediumCave();
            this.largeCave = new LargeCave();

            this.unitHandler = new ControlModeHandler.UnitHandler(this);
            this.buildCaveHandler = new ControlModeHandler.BuildCaveHandler(this);
            this.buildTunnelHandler = new ControlModeHandler.BuildTunnelHandler(this);
            this.clearTunnelHandler = new ControlModeHandler.ClearTunnelHandler(this);
            this.currentModeHandler = null;
        }

        /// <summary>
        /// generates a new set of blocks to start with
        /// </summary>
        private void GenerateNewWorld()
        {
            for (int x = this.Properties.StartingAreaBlocksLeft; x <= this.Properties.StartingAreaBlocksRight; x++)
                for (int y = this.Properties.StartingAreaBlocksBottom; y <= this.Properties.StartingAreaBlocksTop; y++)
                    this.ConstructBlock(new Microsoft.Xna.Framework.Point(x, y));

            // mine the starting tunnel
            Block tmpBlock;

            tmpBlock = this.ConstructBlock(new Microsoft.Xna.Framework.Point(4, 1), 1, true);
            tmpBlock.Hide();
            
            if (this.blocks[0].TryGetValue(new Microsoft.Xna.Framework.Point(4, 0), out tmpBlock))
                tmpBlock.Destruct(false);
            if (this.blocks[0].TryGetValue(new Microsoft.Xna.Framework.Point(4, -1), out tmpBlock))
                tmpBlock.Destruct(false);
            
            // add starting area to the mined blocks so units can move
            int posy = 2;
            for (int posx = -7; posx < 5; posx++)
            {
                Microsoft.Xna.Framework.Point pos = new Microsoft.Xna.Framework.Point(posx, posy);
                Block newBlock = this.ConstructBlock(pos, 1, true);
                newBlock.Hide();
            }
        }

        /// <summary>
        /// converts pixel position into tileposition
        /// </summary>
        /// <param name="pixelPosition"> Vector3 pixelposition to be converted in tile coordinates</param>
        /// <returns></returns>
        public Point PixelPositionToTilePosition(Vector3 pixelPosition)
        {
            Point target;
            target.X = (int)Math.Floor(pixelPosition.X / this.Properties.TileSize);
            target.Y = (int)Math.Floor(pixelPosition.Y / this.Properties.TileSize);
            return target;
        }

        /// <summary>
        /// checks if the placement of a block at the current position is allowed or not
        /// </summary>
        /// <param name="position">the position of the block</param>
        /// <returns>allowed or not</returns>
        public Boolean IntersectsStartingArea(Point position)
        {
            return (position.Y > this.Properties.StartingAreaBottomBorder && position.X < this.Properties.StartingAreaRightBorder && position.X > this.Properties.StartingAreaLeftBorder) ? true : false;
        }

        /// <summary>
        /// checks if the block is above the build level
        /// </summary>
        /// <param name="position">block position</param>
        /// <returns>above or not</returns>
        public Boolean IsAboveBuildLevel(Point position)
        {
            return position.Y > this.Properties.BuildLevel ? true : false;
        }

        /// <summary>
        /// returns the depth of the block at the given position
        /// </summary>
        /// <param name="position">the block position</param>
        /// <returns>the depth of the block</returns>
        public int GetBlockDepth(Point position)
        {
            for (int i = 0; i < this.Properties.MaxBlockDepth; i++)
            {
                if (this.blocks[i].ContainsKey(position))
                   return i;
            }

            return -1;
        }

        /// <summary>
        /// Generates a block if possible
        /// </summary>
        /// <param name="position">position of the block to be created</param>
        /// <param name="depth">depth layer of the block</param>
        /// <param name="force">force the block to be created what ever comes</param>
        /// <returns>the created Block or null</returns>
        public Block ConstructBlock(Point position, int depth = 0, Boolean force = false)
        {
            // Block already present - skip this
            if (this.blocks[depth].ContainsKey(position))
                return this.blocks[depth][position];

            // block not already mined && block not in starting area
            // maxbe needs a check for a valid depth value
            if (force || (!( this.GetBlockDepth(position) > depth) && !this.IntersectsStartingArea(position)))
            {
                Block newBlock = new Block(position, depth, !this.IntersectsStartingArea(position));
                this.blocks[depth].Add(position, newBlock);
                newBlock.destruction += new EventHandler<BlockDestructEvent>(OnBlockDestructed);
                return newBlock;
            }
            return null;
        }

        /// <summary>
        /// get the state for a specific block
        /// </summary>
        /// <param name="position">the position</param>
        /// <returns>the state Solid / Walkable / Climbable / None</returns>
        public TileState GetBlockState(Point position)
        {
            int blockDepth = this.GetBlockDepth(position);

            switch (blockDepth)
            {
                case -1:
                    return TileState.None;
                case 0:
                    return TileState.Solid;
                default:
                    return TileState.Walkable;
            }
        }

        /// <summary>
        /// check if a block is marked for mining
        /// </summary>
        /// <param name="position"></param>
        /// <param name="depth"></param>
        /// <returns></returns>
        public Boolean IsMarked(Point position)
        {
            return this.markers.ContainsKey(position);
        }

        /// <summary>
        /// returns the first block at the position
        /// </summary>
        /// <param name="position">the position of the block</param>
        /// <returns></returns>
        public Block GetBlock(Microsoft.Xna.Framework.Point position)
        {
            int depth = this.GetBlockDepth(position);

            if (depth == -1)
                return this.ConstructBlock(position);
            else
                return this.blocks[depth][position];
        }

        /// <summary>
        /// handles the event thats get raised if a blocks get destructed
        /// </summary>
        /// <param name="block">the block</param>
        /// <param name="arguments">the arguments</param>
        private void OnBlockDestructed(object block, BlockDestructEvent arguments)
        {
            try
            {
                Block theBlock = block as Block;
                this.OnDestructBlockEvent(theBlock);
            }
            catch (Exception e)
            {
                FenrirGame.Instance.Log(LogLevel.Error, "failed to handle block destruction event! " + ((Block)block).GridPosition + " - " + e.Message);
                FenrirGame.Instance.Log(LogLevel.Error, e.StackTrace);
            }
        }

        /// <summary>
        /// INTERNAL USE ONLY!
        /// this will NOT destruct a block
        /// this is a destruction handler!
        /// </summary>
        private void OnDestructBlockEvent(Block block)
        {
            if (blocks[block.Depth].ContainsKey(block.GridPosition))
            {
                this.blocks[block.Depth].Remove(block.GridPosition);

                // block behind
                if(block.Depth < this.Properties.MaxBlockDepth)
                    this.ConstructBlock(block.GridPosition, block.Depth + 1, true);

                // surrounding blocks
                Point tmp = new Point();
                int depth;

                for (int x = -1; x < 2; x++)
                    for (int y = -1; y < 2; y++)
                    {
                        tmp.X = block.GridPosition.X + x;
                        tmp.Y = block.GridPosition.Y + y;
                        depth = this.GetBlockDepth(tmp);
                        if(depth <= block.Depth)
                            this.ConstructBlock(tmp, block.Depth);
                    }
            }

            // remove marker
            if (this.markers.ContainsKey(block.GridPosition) && block.Depth == this.markers[block.GridPosition].Depth - 1)
            {
                if (this.markers[block.GridPosition].Type == MarkerType.Cave)
                    foreach (Cave cave in this.caves)
                        cave.CaveBlockMined(block.GridPosition);

                this.markers.Remove(block.GridPosition);
            }
        }

        /// <summary>
        /// computes the shortest route to a given point .. if reachable
        /// </summary>
        /// <param name="currentPosition">the start position</param>
        /// <param name="target">the position you want to get to</param>
        /// <returns>the path or null if unreachable</returns>
        public LinkedList<Microsoft.Xna.Framework.Point> getPath(Microsoft.Xna.Framework.Point currentPosition, Microsoft.Xna.Framework.Point target){
            return this.getPath(currentPosition, new List<Microsoft.Xna.Framework.Point>(new Microsoft.Xna.Framework.Point[] { target } ));
        }

        /// <summary>
        /// computes the shortest route to a given set of possible points .. if reachable
        /// </summary>
        /// <param name="currentPosition">the start position</param>
        /// <param name="targets">the positions you want to get to</param>
        /// <returns></returns>
        public LinkedList<Microsoft.Xna.Framework.Point> getPath(Microsoft.Xna.Framework.Point currentPosition, List<Microsoft.Xna.Framework.Point> targets)
        {
            foreach(Microsoft.Xna.Framework.Point target in targets)
                if(currentPosition == target)
                    return new LinkedList<Microsoft.Xna.Framework.Point>();


            // Breadth-First-Search / Distance Table
            // this will give us the perfect route

            Boolean foundTarget = false;
            Dictionary<Microsoft.Xna.Framework.Point, int> distanceMap = new Dictionary<Microsoft.Xna.Framework.Point, int>();
            Microsoft.Xna.Framework.Point? targetTile = null;

            List<Microsoft.Xna.Framework.Point> neighboursCurrent = new List<Microsoft.Xna.Framework.Point>();  // neighbours, checkt in the current iteration
            List<Microsoft.Xna.Framework.Point> neighboursNext = new List<Microsoft.Xna.Framework.Point>();     // neighbours, checkt in the next iteration

            int distance = 0;

            Microsoft.Xna.Framework.Point[] pointer = { new Microsoft.Xna.Framework.Point(), new Microsoft.Xna.Framework.Point(), new Microsoft.Xna.Framework.Point(), new Microsoft.Xna.Framework.Point() };
            neighboursCurrent.Add(currentPosition);


            while (!foundTarget)
            {
                foreach (Microsoft.Xna.Framework.Point neighbour in neighboursCurrent)
                {
                    if (!foundTarget && !distanceMap.ContainsKey(neighbour)) // skip if already found or the rare occasion with two or more ways to the same tile with the same amount of steps
                    {
                        distanceMap.Add(neighbour, distance);

                        pointer[2].Y = neighbour.Y;
                        pointer[2].X = neighbour.X - 1;

                        pointer[3].Y = neighbour.Y;
                        pointer[3].X = neighbour.X + 1;

                        pointer[0].Y = neighbour.Y - 1;
                        pointer[0].X = neighbour.X;

                        pointer[1].Y = neighbour.Y + 1;
                        pointer[1].X = neighbour.X;

                        foreach (Microsoft.Xna.Framework.Point possibleMovement in pointer)
                        {
                            Components.TileState state = FenrirGame.Instance.InGame.Scene.GetBlockState(possibleMovement);
                            if ((state == Components.TileState.Walkable || state == Components.TileState.Climbable) && !distanceMap.ContainsKey(possibleMovement))
                                neighboursNext.Add(possibleMovement);

                            // check if a target
                            foreach (Microsoft.Xna.Framework.Point target in targets)
                                if (possibleMovement.X == target.X && possibleMovement.Y == target.Y)
                                {
                                    foundTarget = true;
                                    targetTile = target;
                                }
                        }
                    }
                }

                distance++;
                neighboursCurrent = new List<Microsoft.Xna.Framework.Point>(neighboursNext);
                neighboursNext.Clear();

                // if we have no next neighbours left to check and still havent found the target there is no way
                if (neighboursCurrent.Count == 0)
                    foundTarget = true;
            }

            // if the current block is the one to be mined error is set too
            // so check if me might found the target
            if (!targetTile.HasValue)
            {
                FenrirGame.Instance.Log(LogLevel.Error, "unreachable target for pathfinding");
                return null;
            }
            else
            {
                // samething backwards - search the path
                foundTarget = false;
                LinkedList<Microsoft.Xna.Framework.Point> path = new LinkedList<Microsoft.Xna.Framework.Point>();
                Microsoft.Xna.Framework.Point moveTo = targetTile.Value;
                Boolean foundSmallerOne;
                path.AddFirst(targetTile.Value);

                while (!foundTarget)
                {
                    pointer[0].Y = targetTile.Value.Y;
                    pointer[0].X = targetTile.Value.X - 1;

                    pointer[1].Y = targetTile.Value.Y;
                    pointer[1].X = targetTile.Value.X + 1;

                    pointer[2].Y = targetTile.Value.Y - 1;
                    pointer[2].X = targetTile.Value.X;

                    pointer[3].Y = targetTile.Value.Y + 1;
                    pointer[3].X = targetTile.Value.X;

                    foundSmallerOne = false;

                    foreach (Microsoft.Xna.Framework.Point possibleMovement in pointer)
                    {
                        int blockDistance;
                        if (distanceMap.TryGetValue(possibleMovement, out blockDistance))
                        {
                            if (blockDistance < distance && blockDistance > 0)
                            {
                                foundSmallerOne = true;
                                distance = blockDistance;
                                moveTo = possibleMovement;
                            }
                        }
                    }

                    if (foundSmallerOne)
                    {
                        path.AddFirst(moveTo);
                        targetTile = moveTo;
                    }
                    else
                        foundTarget = true;
                }

                return path;
            }
        }

        /// <summary>
        /// exits the current mode
        /// </summary>
        public void DisposeCurrentModeHandler() 
        {
            this.currentModeHandler.Deactivate();
            this.currentModeHandler = null;
        }

        /// <summary>
        /// activates a specific mode of operation
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="entity"></param>
        public void ActivateModeHandler(ModeHandler handler, Entity entity = null)
        {
            if(this.currentModeHandler != null)
                this.DisposeCurrentModeHandler();

            switch (handler)
            {
                case ModeHandler.Unit:
                    this.currentModeHandler = this.unitHandler;
                    this.unitHandler.Activate(entity);
                    break;
                case ModeHandler.BuildTunnel:
                    this.currentModeHandler = this.buildTunnelHandler;
                    this.buildTunnelHandler.Activate(null);
                    break;
                case ModeHandler.ClearTunnel:
                    this.currentModeHandler = this.clearTunnelHandler;
                    this.clearTunnelHandler.Activate(null);
                    break;
                case ModeHandler.BuildCaveSmall:
                    this.currentModeHandler = this.buildCaveHandler;
                    this.buildCaveHandler.Activate(this.smallCave);
                    break;
                case ModeHandler.BuildCaveMedium:
                    this.currentModeHandler = this.buildCaveHandler;
                    this.buildCaveHandler.Activate(this.mediumCave);
                    break;
                case ModeHandler.BuildCaveLarge:
                    this.currentModeHandler = this.buildCaveHandler;
                    this.buildCaveHandler.Activate(this.largeCave);
                    break;
                default:
                    this.currentModeHandler = null;
                    break;
            }
        }

        /// <summary>
        /// default update function
        /// </summary>
        public void Update()
        {
            if (FenrirGame.Instance.Properties.CurrentGameState == GameState.InGame)
            {

                Point hoverPoint = new Point();
                float? mouseDistance = FenrirGame.Instance.Properties.Input.MouseRay.Intersects(this.mouseSurface);
                if (mouseDistance.HasValue)
                {
                    Vector3 mousePosition = FenrirGame.Instance.Properties.Input.MouseRay.Position + FenrirGame.Instance.Properties.Input.MouseRay.Direction * mouseDistance.Value;
                    hoverPoint = new Microsoft.Xna.Framework.Point((int)Math.Round(mousePosition.X / this.Properties.TileSize), (int)Math.Round(mousePosition.Y / this.Properties.TileSize));
                }

                if (this.currentModeHandler != null)
                    this.currentModeHandler.Handle(hoverPoint);
                
                // no else so the handler can deactivate itself and a new one can be started in the same update cyclus
                if(this.currentModeHandler == null)
                {
                    foreach (Entities.Unit unit in this.Units)
                        if (FenrirGame.Instance.Properties.Input.MouseRay.Intersects(unit.Boundingbox) != null && FenrirGame.Instance.Properties.Input.LeftClick)
                            this.ActivateModeHandler(ModeHandler.Unit, unit);
                }
            }

            // update all units
            foreach (Entities.Unit unit in this.units)
                unit.Update();

            if (FenrirGame.Instance.Properties.Input.EndCurrentAction)
            {
                if (this.currentModeHandler != null)
                    this.DisposeCurrentModeHandler();
                else
                {
                    if (FenrirGame.Instance.Properties.CurrentGameState == GameState.InGame)
                        FenrirGame.Instance.Properties.RequestNewGameState(GameState.Paused);
                    else if (FenrirGame.Instance.Properties.CurrentGameState == GameState.Paused)
                        FenrirGame.Instance.Properties.RequestNewGameState(GameState.InGame);
                }
            }
        }

        /// <summary>
        /// draw everything in this scene
        /// </summary>
        public void Draw()
        {
            foreach (Entities.Unit unit in this.units)
                unit.Draw();

            foreach (Building building in this.buildings)
                building.Draw();

            for(int i = 0; i < this.Properties.MaxBlockDepth; i++)
                foreach (KeyValuePair<Microsoft.Xna.Framework.Point, Block> block in this.blocks[i])
                    block.Value.Draw();

            foreach (Cave cave in this.caves)
                cave.Draw();

            FenrirGame.Instance.Renderer.Draw(
                DataIdentifier.modelStartingArea,
                Matrix.CreateRotationX(Microsoft.Xna.Framework.MathHelper.ToRadians(-90)) * Matrix.CreateRotationY(MathHelper.ToRadians(-90)) * Matrix.CreateTranslation(new Vector3(-2, 2, 0))
                );

            if (this.currentModeHandler != null)
                this.currentModeHandler.DrawHelper();

            foreach(KeyValuePair<Point, Marker> maker in this.markers)
                maker.Value.Draw();
        }
    }
}
