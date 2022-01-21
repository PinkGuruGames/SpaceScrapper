using System.Collections.Generic;
using SpaceScrapper.MathLib;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SpaceScrapper.Debug.PoissonSamplingTest
{
    [ExecuteInEditMode]
    public class PointsManager : MonoBehaviour
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

        private List<Vector2> points; 

        public void GeneratePoints()
        {
            Random.InitState(seed);
            points = PoissonDiscSampling.GeneratePoints(samplingRadius, region.size, maxSamplingTries);
            UnityEngine.Debug.Log($"Generated {points.Count} points");
            SceneView.RepaintAll();
        }

        private void OnDrawGizmos()
        {
            // Draw Bounding box
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(region.center, region.size);
            
            if (points == null) return;
            
            Gizmos.color = Color.red;
            foreach (var v in points)
            {
                Gizmos.DrawSphere((Vector3)v - region.extents, samplingRadius/10f);
            }
        }
    }
}