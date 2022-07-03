using UnityEngine;
using UnityEditor;
using SpaceScrapper;

namespace SpaceScrapper.EditorTools
{
    [CustomEditor(typeof(AIControllerBase), editorForChildClasses: true)]
    public class AIControllerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            //Debug.Log("controller editor");
            AIControllerBase controller = (AIControllerBase)target;
            //check if the controller has a LivingEntity component, which is important.
            if (controller.GetComponent<LivingEntity>() == null)
            {
                EditorGUILayout.HelpBox("AI Controller doesn't yet have a LivingEntity component. This is required.", MessageType.Warning);
            }
        }
    }
}
