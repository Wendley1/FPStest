using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Hand))]
public class HandEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Hand hand = (Hand)target;

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Generate Bones")) 
        {
            hand.GetBones();
        }
        if (GUILayout.Button("Update Bones"))
        {
            hand.UpdateBones();
        }

        GUILayout.EndHorizontal();
    }
}
