using System;
using UnityEngine;

namespace SpaceScrapper.MathLib.VoronoiLib.Structures
{
    public class VPoint: IEquatable<VPoint>
    {
        public double X { get; }
        public double Y { get; }

        internal VPoint(double x, double y)
        {
            X = x;
            Y = y;
        }

        public Vector2 ToVector2()
        {
            return new Vector2((float)X, (float)Y);
        }

        public Vector3 ToVector3()
        {
            return new Vector3((float)X, (float)Y, 0f);
        }

        //public override bool Equals(object obj)
        //{
        //    if (obj != null)
        //    {
        //        return false;
        //    }

        //    VPoint other = (VPoint)obj;

        //    return other.X.ApproxEqual(X) && other.Y.ApproxEqual(Y);
        //}

        public override int GetHashCode()
        {
            var hashCode = 1861411795;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            return hashCode;
        }

        public bool Equals(VPoint other)
        {
            return other.X.ApproxEqual(X) && other.Y.ApproxEqual(Y);
        }
    }
}
