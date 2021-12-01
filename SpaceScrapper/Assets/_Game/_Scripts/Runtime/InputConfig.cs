using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu]
public class InputConfig : ScriptableObject
{
    [Header("Physics")]
    public float MaxSpeed = 10f;
    public float Acceleration = 2f;
    public float ConstantDrag = 0.05f;
    public bool ApplyConstantDragAlways = true;
    public bool ApplyDragIndpendently = false;
    public float BrakeDrag = 0.2f;

    [Header("Input")]
    public DirectionalMultipliers WASDMultipliers;
    public bool LocalSpaceDirections;
    public BrakeKey Brake;

    [System.Serializable]
    public struct DirectionalMultipliers
    {
        [Range(0, 1)] public float Forward;
        [Range(0, 1)] public float Back;
        [Range(0, 1)] public float Left;
        [Range(0, 1)] public float Right;

        public Vector2 ApplyTo(Vector2 v)
        {
            if (v.x > 0)
                v.x *= Right;
            else
                v.x *= Left;

            if (v.y > 0)
                v.y *= Forward;
            else
                v.y *= Back;

            return v;
        }
    }

    public enum BrakeKey { None, S, Space }
}
