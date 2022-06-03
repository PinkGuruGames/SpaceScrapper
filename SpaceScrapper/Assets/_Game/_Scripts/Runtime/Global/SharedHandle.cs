using UnityEngine;

namespace SpaceScrapper
{
    /// <summary>
    /// This class holds a reference to a Unity objects and ensures it will be safely bound and unbound
    /// </summary>
    public class SharedHandle<T> where T : Object
    {
        public T Value { get; private set; }

        public bool HasValue => Value != null;

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

        public static implicit operator T(SharedHandle<T> or)
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