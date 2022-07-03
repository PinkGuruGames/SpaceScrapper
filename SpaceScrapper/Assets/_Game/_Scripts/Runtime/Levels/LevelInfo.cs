using UnityEngine;

namespace SpaceScrapper.Levels
{
    [CreateAssetMenu(fileName = "New Level Info", menuName = "Custom Data/Level Info")]
    public class LevelInfo : ScriptableObject
    {
        [SerializeField]
        private string levelName;
        [SerializeField]
        private string sceneName;

        private bool hasBeenCompleted = false;

        //needed for in-world quest markers, and placing the player at the correct positions when leaving a level. (wait for it to be bound)
        public readonly SharedHandle<LevelEntrance> Entrance = new SharedHandle<LevelEntrance>();

        public string SceneName => sceneName;
        public string LevelName => levelName;
        public bool HasBeenCompleted => hasBeenCompleted;
    }
}
