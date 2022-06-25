using UnityEngine;

namespace SpaceScrapper
{
    [CreateAssetMenu(menuName = "Custom Data/Database Entry", fileName = "new Database Entry")]
    public class DatabaseEntry : ScriptableObject
    {
        [SerializeField]
        private string title;
        [SerializeField, TextArea(3, 10)]
        private string description;

        public string Title => title;
        public string Description => description;
    }
}
