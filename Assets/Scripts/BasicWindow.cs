using UnityEngine;
using UnityEditor;
using System.Collections;

public class BasicWindow : EditorWindow {

    string basicString = "Basic";
    bool groupEnabled;
    bool basicBool = false;
    float basicFloat = 1.5f;

    // Adds a menu item called "Basic Window" to the Window menu
    [MenuItem("Window/Basic Window")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(BasicWindow));
    }

    void OnGUI()
    {
        GUILayout.Label("Basic Settings", EditorStyles.boldLabel);
        basicString = EditorGUILayout.TextField("Basic Text", basicString);

        groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
        basicBool = EditorGUILayout.Toggle("Toggle", basicBool);
        basicFloat = EditorGUILayout.Slider("Slider", basicFloat, -3, 3);
        EditorGUILayout.EndToggleGroup();
    }
}
