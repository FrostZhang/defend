using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Asher : EditorWindow {
    [MenuItem("Tools/Asher")]
    public static void Open()
    {
        var asher = GetWindow<Asher>();
        asher.Show();
        asher.position = new Rect(100, 100, 200, 100);
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Clear Player prefabs"))
        {
            PlayerPrefs.DeleteAll(); 
        }
    }
}
