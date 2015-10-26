using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Fenrir.Src.Helper;

namespace Fenrir.Src.LoadingScreen
{
    /// <summary>
    /// Loadingscreen for ingame
    /// </summary>
    class LoadGame : LoadingScreen
    {
        public override void Load()
        {
            // HUD
            FenrirGame.Instance.Properties.ContentManager.AddTextureToLibrary(DataIdentifier.textureIconTunnel, "Texture/Icons/Tunnel");
            FenrirGame.Instance.Properties.ContentManager.AddTextureToLibrary(DataIdentifier.textureIconTunnelHover, "Texture/Icons/Tunnel_hover");
            FenrirGame.Instance.Properties.ContentManager.AddTextureToLibrary(DataIdentifier.textureIconSmallCave, "Texture/Icons/Cave_small");
            FenrirGame.Instance.Properties.ContentManager.AddTextureToLibrary(DataIdentifier.textureIconSmallCaveHover, "Texture/Icons/Cave_small_hover");
            FenrirGame.Instance.Properties.ContentManager.AddTextureToLibrary(DataIdentifier.textureIconMediumCave, "Texture/Icons/Cave_medium");
            FenrirGame.Instance.Properties.ContentManager.AddTextureToLibrary(DataIdentifier.textureIconMediumCaveHover, "Texture/Icons/Cave_medium_hover");
            FenrirGame.Instance.Properties.ContentManager.AddTextureToLibrary(DataIdentifier.textureIconLargeCave, "Texture/Icons/Cave_large");
            FenrirGame.Instance.Properties.ContentManager.AddTextureToLibrary(DataIdentifier.textureIconLargeCaveHover, "Texture/Icons/Cave_large_hover");
            FenrirGame.Instance.Properties.ContentManager.AddTextureToLibrary(DataIdentifier.texturePauseMenuTitle, "Texture/MainMenu/Title");
            FenrirGame.Instance.Properties.ContentManager.AddTextureToLibrary(DataIdentifier.texturePauseMenuBackground, "Texture/PauseMenu/Background");

            // game
            FenrirGame.Instance.Properties.ContentManager.AddModelToLibrary(DataIdentifier.modelBlock, "Model/Entities/Block");
            FenrirGame.Instance.Properties.ContentManager.AddModelToLibrary(DataIdentifier.modelStartingArea, "Model/World/Startlocation");
            FenrirGame.Instance.Properties.ContentManager.AddModelToLibrary(DataIdentifier.modelPlayerUnit, "Model/Entities/Player");
            FenrirGame.Instance.Properties.ContentManager.AddModelToLibrary(DataIdentifier.modelMarker, "Model/Overlays/Marker");
            FenrirGame.Instance.Properties.ContentManager.AddModelToLibrary(DataIdentifier.modelSmallCave, "Model/Caves/smallCave");
            FenrirGame.Instance.Properties.ContentManager.AddModelToLibrary(DataIdentifier.modelMediumCave, "Model/Caves/mediumCave");
            FenrirGame.Instance.Properties.ContentManager.AddModelToLibrary(DataIdentifier.modelLargeCave, "Model/Caves/largeCave");
            FenrirGame.Instance.Properties.ContentManager.AddModelToLibrary(DataIdentifier.modelBuildingCampfire, "Model/Buildings/Campfire/campfire");

            FenrirGame.Instance.InGame.LoadLevel();

            // done - go to menu
            FenrirGame.Instance.Properties.RequestNewGameState(GameState.InGame);
        }
    }
}
