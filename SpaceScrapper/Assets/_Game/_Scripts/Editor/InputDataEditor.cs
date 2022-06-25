using UnityEngine;
using UnityEditor;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace SpaceScrapper.EditorTools
{
    [CustomEditor(typeof(InputData))]
    public class InputDataEditor : Editor
    {
        private SerializedProperty inputAsset;
        private SerializedProperty gameplayActionMapName;
        private SerializedProperty pauseMenuActionMapName;
        private SerializedProperty dialogueScreenActionMapName;

        private void OnEnable()
        {
            //nothing special, just get all the properties we need.
            inputAsset = serializedObject.FindProperty("inputAsset");
            gameplayActionMapName = serializedObject.FindProperty("gameplayActionMapName");
            pauseMenuActionMapName = serializedObject.FindProperty("pauseMenuActionMapName");
            dialogueScreenActionMapName = serializedObject.FindProperty("dialogueScreenActionMapName");
        }

        public override void OnInspectorGUI()
        {
            //most stuff handled by the default editor GUI.
            base.OnInspectorGUI();
            //check if an input action asset is referenced.
            var inputAssetReference = (InputActionAsset) inputAsset.objectReferenceValue;
            if(inputAssetReference != null)
            {
                //get the possible action maps, then reduce them to their names.
                var actionMaps = inputAssetReference.actionMaps;
                string[] maps = new string[actionMaps.Count];
                for(int i = 0; i < maps.Length; i++)
                    maps[i] = actionMaps[i].name;

                //assign values to the action map name fields.
                gameplayActionMapName.stringValue       = GetInputMapName(gameplayActionMapName, maps);
                pauseMenuActionMapName.stringValue      = GetInputMapName(pauseMenuActionMapName, maps);
                dialogueScreenActionMapName.stringValue = GetInputMapName(dialogueScreenActionMapName, maps);

                //apply modifications
                serializedObject.ApplyModifiedProperties();
            }
        }

        private string GetInputMapName(SerializedProperty stringProperty, string[] options)
        {
            string currentValue = stringProperty.stringValue;
            //assign first map if none applicable.
            if (string.IsNullOrEmpty(currentValue))
                return options[0];
            int index = 0;
            //find currentValue in options.
            for (int i = 0; i < options.Length; i++)
                if (currentValue.Equals(options[i]))
                {
                    index = i;
                    break;
                }
            //reassign the index.
            index = EditorGUILayout.Popup(stringProperty.displayName, index, options);
            //return the new value
            return options[index];
        }
    }
}
