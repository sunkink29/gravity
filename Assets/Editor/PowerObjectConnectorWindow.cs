using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

class PowerObjectConnectorWindow : EditorWindow {

    List<PowerObject> selectedPowerObjects = new List<PowerObject>();
    List<SerializedObject> selectedSerializedObjects = new List<SerializedObject>();

    PowerObject currentSelectedPowerObject = null;

    [MenuItem ("Window/PowerConnector")]
    public static void  ShowWindow () {
        EditorWindow.GetWindow(typeof(PowerObjectConnectorWindow));
    }
    
    void OnGUI () {
        GUILayout.Label ("Current selected power chain", EditorStyles.boldLabel);
        for (int i = 0; i < selectedPowerObjects.Count; i++) {
            EditorGUILayout.LabelField(selectedPowerObjects[i].name);
        }
        GUILayout.BeginHorizontal();
        if (selectedPowerObjects.Count == 0) {
            if (GUILayout.Button("start new chain") && currentSelectedPowerObject != null) {
                selectedPowerObjects.Add(currentSelectedPowerObject);
                selectedSerializedObjects.Add(new SerializedObject(currentSelectedPowerObject));
            }
        } else {
            if (GUILayout.Button("add powerObject") && currentSelectedPowerObject != null) {
                if (!selectedPowerObjects.Contains(currentSelectedPowerObject)) {
                    selectedPowerObjects.Add(currentSelectedPowerObject);
                    selectedSerializedObjects.Add(new SerializedObject(currentSelectedPowerObject));
                }
            }
            if (GUILayout.Button("delete last powerObject from chain") && currentSelectedPowerObject != null) {
                selectedPowerObjects.RemoveAt(selectedPowerObjects.Count - 1);
                selectedSerializedObjects.RemoveAt(selectedSerializedObjects.Count - 1);
            }
            if (GUILayout.Button("end chain")) {
                for (int i = 1; i < selectedPowerObjects.Count; i++) {
                    SerializedProperty powerProviderProperty = selectedSerializedObjects[i].FindProperty("powerProvider");
                    powerProviderProperty.objectReferenceValue = selectedPowerObjects[i-1];
                    selectedSerializedObjects[i].ApplyModifiedProperties();
                }
                selectedPowerObjects.Clear();
                selectedSerializedObjects.Clear();
            }
        }
        GUILayout.EndHorizontal();
        if (currentSelectedPowerObject == null) {
            EditorGUILayout.LabelField("selected object does not contain a powerObject script");
        }
    }

    void OnSelectionChange() {
        if (Selection.activeGameObject != null) {
            currentSelectedPowerObject = Selection.activeGameObject.GetComponent<PowerObject>();
        }
        Repaint();
    }
}