using UnityEngine;

namespace SpaceScrapper
{
    /// <summary>
    /// This class houses references to globally accesible objects like a camera or player
    /// </summary>
    public class SceneContext
    {


        /// <summary>
        /// This class hold a reference to a Unity objects and ensures it will be safely set and reset
        /// </summary>
        public struct ObjectRef<T> where T : Object
        {
            public T Value { get; private set; }

            public void Set(T newReference)
            {
                if (Value != null)
                {
                    Debug.LogWarning($"Overriding the object {GetName(Value)} with {GetName(newReference)} make sure it's expected behaviour!");
                }

                Value = newReference;
            }

            public void Reset(T oldReference)
            {
                if (Value != oldReference)
                {
                    Debug.LogError($"Cannot reset value using {GetName(oldReference)} because the reference set is {GetName(Value)}!");
                    return;
                }

                Value = null;
            }

            public static implicit operator T(ObjectRef<T> or)
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
