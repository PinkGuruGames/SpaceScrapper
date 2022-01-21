using UnityEditor;
using UnityEngine;

namespace SpaceScrapper.Debug.VoronoiTest
{
    [CustomEditor(typeof(VoronoiManager), false)]
    public class VoronoiManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUI.BeginDisabledGroup(serializedObject.isEditingMultipleObjects);
            if (GUILayout.Button("Generate"))
            {
                ((VoronoiManager)target).GenerateVoronoi();
            }
            EditorGUI.EndDisabledGroup();
        }
    }
}