using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceScrapper.Global
{
    /// <summary>
    /// Backbone of the Game
    /// </summary>
    public static class Game
    {
        public static SceneContext SceneContext { get; private set; }
        public static GameEvents Events { get; private set; }


        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void Reset()
        {
            SceneContext = new SceneContext();
            Events = new GameEvents();
        }
    }
}
