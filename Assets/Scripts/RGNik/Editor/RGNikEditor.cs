using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RGNik))]
public class RGNikEditor : Editor
{
    public override void OnInspectorGUI()
    {
        RGNik myTarget = (RGNik)target;

        GUIStyle style = new GUIStyle();
        style.richText = true;

        GUILayout.Label("<size=30><color=green>~ RGNik ~</color> 3D Room Generator </size>", style);

        this.DrawDefaultInspector();

        //GUILayout.Label("<size=30>This is a <color=red>CUSTOM</color> text</size>", style);
    }
}
