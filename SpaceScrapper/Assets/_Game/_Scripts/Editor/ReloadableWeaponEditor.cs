using UnityEditor;
using UnityEngine;
using SpaceScrapper.Weapons;

namespace SpaceScrapper.EditorTools
{
    [CustomEditor(typeof(ReloadableWeapon), editorForChildClasses: true)]
    public class ReloadableWeaponEditor : Editor
    {
        SerializedProperty currentAmmoProperty;
        SerializedProperty magazineSizeProperty;
        SerializedProperty reloadTimeProperty;
        SerializedProperty infiniteAmmoProperty;

        private void OnEnable()
        {
            currentAmmoProperty = serializedObject.FindProperty("currentReserveAmmo");
            magazineSizeProperty = serializedObject.FindProperty("magazineSize");
            reloadTimeProperty = serializedObject.FindProperty("reloadTime");
            infiniteAmmoProperty = serializedObject.FindProperty("hasInfiniteAmmo");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if(infiniteAmmoProperty.boolValue is false)
            {
                EditorGUILayout.PropertyField(currentAmmoProperty);
                EditorGUILayout.PropertyField(magazineSizeProperty);
                EditorGUILayout.PropertyField(reloadTimeProperty);
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
