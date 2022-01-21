using UnityEditor;
using UnityEngine;

namespace SpaceScrapper.Debug.PoissonSamplingTest
{

    [CustomEditor(typeof(PointsManager), false)]
    public class PointsManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUI.BeginDisabledGroup(serializedObject.isEditingMultipleObjects);
            if (GUILayout.Button("Generate"))
            {
                ((PointsManager)target).GeneratePoints();
            }
            EditorGUI.EndDisabledGroup();
        }
    }
}
