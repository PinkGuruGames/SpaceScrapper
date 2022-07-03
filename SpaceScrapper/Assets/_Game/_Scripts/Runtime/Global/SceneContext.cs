using UnityEngine;
using SpaceScrapper.Levels;

namespace SpaceScrapper.Global
{
    /// <summary>
    /// This class houses references to globally accesible objects like a camera or player
    /// </summary>
    public class SceneContext
    {
        //the scene context for the current level that is being played.
        //will be cleared AFTER returning back to the level selection, and placing the player at the entrance.
        public readonly SharedHandle<LevelInfo> CurrentLevel = new SharedHandle<LevelInfo>();

        public readonly SharedHandle<DialogueManager> DialogueHandler = new SharedHandle<DialogueManager>();

        public readonly SharedHandle<GameObject> PlayerObject = new SharedHandle<GameObject>();

        /// <summary>
        /// !!Use SharedHandle for readonly fields and properties!!
        /// This struct holds a reference to a Unity objects and ensures it will be safely bound and unbound
        /// </summary>
        public struct Handle<T> where T : Object
        {
            public T Value { get; private set; }

            public void Bind(T sender)
            {
                if (Value != null)
                {
                    Debug.LogWarning($"Overriding the object {GetName(Value)} with {GetName(sender)} make sure it's expected behaviour!");
                }

                Value = sender;
            }

            public void Unbind(T sender)
            {
                // We make sure that only the object whose reference we hold can remove itself
                if (Value != sender)
                {
                    Debug.LogError($"Cannot reset value using {GetName(sender)} because the reference set is {GetName(Value)}!");
                    return;
                }

                Value = null;
            }

            public static implicit operator T(Handle<T> or)
            {
                return or.Value;
            }

            private string GetName(T o)
            {
                if (o == null)
                    return "[null]";
                return o.name;
            }
        }
    }
}
