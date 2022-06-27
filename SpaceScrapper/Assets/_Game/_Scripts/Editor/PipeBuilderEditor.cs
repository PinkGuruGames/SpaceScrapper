using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PipeBuilder))]
public class PipeBuilderEditor : Editor
{
    PipeBuilder pipe;
    Transform transform;

    SerializedProperty controlPointsProperty;

    private void OnEnable()
    {
        pipe = target as PipeBuilder;   
        transform = pipe.GetComponent<Transform>(); 
        controlPointsProperty = serializedObject.FindProperty("controlPoints");
        Debug.Log("Enable Editor Pipe");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }

    public void OnSceneGUI() 
    {
        bool changed = false;
        for(int i = 0; i < pipe.controlPoints.Length; i++)
        {
            //The position handle for the controlpoint.
            var original = transform.TransformPoint(pipe.controlPoints[i]);
            var newPoint = Handles.DoPositionHandle(original, Quaternion.identity);
            pipe.controlPoints[i] = transform.InverseTransformPoint(newPoint);
            changed = changed || Vector3.SqrMagnitude(original - newPoint) > 0.0001f;
        }

        Handles.BeginGUI();
        for(int i = 0; i < pipe.controlPoints.Length; i++)
        {
            Vector3 cPoint = pipe.controlPoints[i];
            Vector3 offset;
            if(i < pipe.controlPoints.Length-1)
                offset = Vector3.Normalize(pipe.controlPoints[i+1] - cPoint);
            else
                offset = Vector3.Normalize(cPoint - pipe.controlPoints[i-1]);
            
            Vector3 localPos = cPoint + offset*0.4f;
            Vector3 worldPos = transform.TransformPoint(localPos);

            //Insert controlpoint 
            Vector2 guiPos = HandleUtility.WorldToGUIPoint(worldPos);
            Rect buttonRect = new Rect(guiPos.x - 7.5f, guiPos.y - 7.5f, 15, 15);
            if(GUI.Button(buttonRect, "+"))
            {
                controlPointsProperty.InsertArrayElementAtIndex(i+1);
                controlPointsProperty.GetArrayElementAtIndex(i+1).vector3Value = localPos;
                serializedObject.ApplyModifiedProperties();
            }
            
            //Remove controlpoint.
            worldPos = transform.TransformPoint(cPoint - offset*0.4f);
            guiPos = HandleUtility.WorldToGUIPoint(worldPos);
            buttonRect.x = guiPos.x - 7.5f;
            buttonRect.y = guiPos.y - 7.5f;
            if(GUI.Button(buttonRect, "-"))
            {
                controlPointsProperty.DeleteArrayElementAtIndex(i);
                serializedObject.ApplyModifiedProperties();
                return;
            }
        }
        Handles.EndGUI();

        if(changed)
        {
            //Debug.Log("Auto rebuild");
            pipe.Build();
        }
    }
}