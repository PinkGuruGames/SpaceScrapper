using System;
using System.Collections.Generic;

namespace SpaceScrapper.MathLib.VoronoiLib.Structures
{
    public class FortuneSite: IEquatable<FortuneSite>
    {
        public static int IDCounter = 0;

        public int ID { get; }
        public double X { get; }
        public double Y { get; }
        public List<VPoint> Corners { get; }

        public bool ToClose { get; set; } = false;

        public HashSet<VEdge> Cell { get; private set; }

        public HashSet<FortuneSite> Neighbors { get; private set; }

        public FortuneSite(double x, double y)
        {
            ID = IDCounter++;
            X = x;
            Y = y;
            Cell = new HashSet<VEdge>();
            Neighbors = new HashSet<FortuneSite>();
            Corners = new List<VPoint>();
        }

        public bool Equals(FortuneSite other)
        {
            return ID.Equals(other.ID);
        }

        public HashSet<VPoint> GetUniquePoints()
        {
            HashSet<VPoint> uniquePoints = new HashSet<VPoint>();
            foreach (var v in Corners)
            {
                uniquePoints.Add(v);
            }
            foreach (var e in Cell)
            {
                uniquePoints.Add(e.Start);
                uniquePoints.Add(e.End);
            }

            return uniquePoints;
        }
    }
}
