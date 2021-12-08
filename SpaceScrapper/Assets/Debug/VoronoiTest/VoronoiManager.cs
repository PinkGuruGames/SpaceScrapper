using System.Collections.Generic;
using System.Linq;
using SpaceScrapper.MathLib;
using SpaceScrapper.MathLib.VoronoiLib;
using SpaceScrapper.MathLib.VoronoiLib.Structures;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceScrapper.Debug.VoronoiTest
{
    public class VoronoiManager : MonoBehaviour
    {
        [SerializeField]
        private int seed;

        [Tooltip("Region within which points will be distributed.")]
        [SerializeField]
        private Bounds region;

        [Tooltip("Maximum iteration before stopping sampling with current point")]
        [Range(10, 500)]
        [SerializeField]
        private int maxSamplingTries = 30;

        [Range(0.1f, 100f)]
        [SerializeField]
        private float samplingRadius = 1;

        [SerializeField]
        private bool drawPointDistribution = false;

        [SerializeField]
        private bool removeOpenTiles = false;

        [SerializeField]
        [Tooltip("Radius for vertices when displaying Voronoi")]
        private float vertexRadius = 0.1f;

        private List<Vector2> points;
        private List<FortuneSite> sites;

        public void GenerateVoronoi()
        {
            UnityEngine.Debug.Log($"Generating Voronoi diagram ...");
            Random.InitState(seed);

            points = PoissonDiscSampling.GeneratePoints(samplingRadius, region.size, maxSamplingTries);
            UnityEngine.Debug.Log($"Generated {points.Count} points");

            sites = points.Select(p => new FortuneSite(p.x, p.y)).ToList();
            FortunesAlgorithm.Run(sites, 0, 0, region.size.x, region.size.y);

            UnityEngine.Debug.Log($"Generated {sites.Count} voronoi \"tiles\"");

            UnityEngine.Debug.Log($"... Voronoi generation complete.");
            SceneView.RepaintAll();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(region.center, region.size);

            DrawPointDistribution();
            DrawVoronoi();
        }

        private void DrawPointDistribution()
        {
            if (!drawPointDistribution) return;

            if (points == null) return;

            Gizmos.color = Color.red;
            foreach (var v in points)
            {
                Gizmos.DrawSphere((Vector3)v - region.extents, vertexRadius);
            }
        }

        private void DrawVoronoi()
        {
            if (sites == null) return;
            var deltaBounds = region.center - region.extents;

            foreach (FortuneSite s in sites)
            {
                if (removeOpenTiles && s.ToClose) continue;

                Handles.color = Color.green;
                Handles.DrawSolidDisc(new Vector3((float)s.X, (float)s.Y, 0f) + deltaBounds, Vector3.back,
                    vertexRadius);

                Handles.color = Color.red;
                foreach (var c in s.Corners)
                {
                    Handles.DrawSolidDisc(new Vector3((float)c.X, (float)c.Y, 0f) + deltaBounds, Vector3.back,
                        vertexRadius);
                }

                //draw site edges
                Handles.color = Color.white;
                foreach (VEdge e in s.Cell)
                {
                    if (e.Start != null && e.End != null)
                    {
                        Vector2 start, end;
                        start.x = (float)e.Start.X;
                        start.y = (float)e.Start.Y;
                        end.x = (float)e.End.X;
                        end.y = (float)e.End.Y;
                        Handles.DrawLine((Vector3)start + deltaBounds, (Vector3)end + deltaBounds, 3f);
                    }
                    else
                    {
                        UnityEngine.Debug.Log($"{e.ID} - Edge missing vertex");
                    }
                }
            }
        }
    }
}