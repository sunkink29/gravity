using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(PowerObject),true)]
[CanEditMultipleObjects]
class PowerObjectEditor : Editor {
    
    private SerializedProperty prop;
    private SerializedProperty showPowerProvider;

    void OnEnable() {
        showPowerProvider = serializedObject.FindProperty("showPowerProvider");
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();
        prop = serializedObject.GetIterator();
        while (prop.NextVisible(true)) {
            if (prop.displayName == "Power Provider" && (serializedObject.targetObject as MonoBehaviour).GetComponent<PowerProviderPowerObject>() != null) {
               if (showPowerProvider != null){
                    EditorGUILayout.PropertyField(prop,true);
                }
            } else if (!(prop.displayName == "Script" || prop.displayName == "Power Type" || prop.displayName == "Size"
            || prop.displayName.Contains("Element") || prop.displayName == "X" || prop.displayName == "Y" || prop.displayName == "Z")) {
                EditorGUILayout.PropertyField(prop,true);  
            }
        }
        serializedObject.ApplyModifiedProperties();
    }
}