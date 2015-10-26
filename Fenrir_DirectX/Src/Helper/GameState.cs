using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fenrir.Src.Helper
{
    /// <summary>
    /// possible game modes
    /// </summary>
    public enum GameState
    {
        /// <summary>
        /// load the main meun
        /// </summary>
        LoadMenu,
        /// <summary>
        /// the main menu
        /// </summary>
        MainMenu,
        /// <summary>
        /// the option menu
        /// </summary>
        OptionMenu,
        /// <summary>
        /// load the game
        /// </summary>
        LoadGame,
        /// <summary>
        /// in game
        /// </summary>
        InGame,
        /// <summary>
        /// game paused
        /// </summary>
        Paused
    }
}
