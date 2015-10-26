using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Fenrir.Src.Helper;

namespace Fenrir.Src.InGame
{
    /// <summary>
    /// Manages all game components
    /// </summary>
    class ComponentManager
    {
        private Components.Camera camera;
        /// <summary>
        /// the camera
        /// </summary>
        internal Components.Camera Camera
        {
            get { return camera; }
            private set { camera = value; }
        }
        private Components.Scene scene;
        /// <summary>
        /// the scene
        /// </summary>
        internal Components.Scene Scene
        {
            get { return scene; }
            private set { scene = value; }
        }

        private Components.Hud hud;
        /// <summary>
        /// the HUD
        /// </summary>
        internal Components.Hud Hud
        {
            get { return hud; }
            private set { hud = value; }
        }
        
        public ComponentManager() { }

        /// <summary>
        /// Load a basic level
        /// </summary>
        public void LoadLevel()
        {
            FenrirGame.Instance.Properties.RequestNewGameState(GameState.LoadGame);
            this.camera = new Components.Camera();
            this.scene = new Components.Scene();
            this.hud = new Components.Hud();

            this.scene.InitializeWorld();

            FenrirGame.Instance.Properties.RequestNewGameState(GameState.InGame);
        }

        /// <summary>
        /// distribute the update signal to all components
        /// </summary>
        public void Update() 
        {
            if (FenrirGame.Instance.Properties.CurrentGameState == GameState.InGame || FenrirGame.Instance.Properties.CurrentGameState == GameState.Paused)
            {
                this.hud.Update();
                this.scene.Update();
                this.camera.Update();
            }
        }

        /// <summary>
        /// distribute the draw signal to all components
        /// </summary>
        public void Draw()
        {
            if (FenrirGame.Instance.Properties.CurrentGameState == GameState.InGame || FenrirGame.Instance.Properties.CurrentGameState == GameState.Paused)
            {
                this.scene.Draw();
                this.hud.Draw();
            }
        }
    }
}
