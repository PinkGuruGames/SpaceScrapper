using System;

namespace SpaceScrapper.MathLib.VoronoiLib.Structures
{
    interface FortuneEvent : IComparable<FortuneEvent>
    {
        double X { get; }
        double Y { get; }
    }
}
