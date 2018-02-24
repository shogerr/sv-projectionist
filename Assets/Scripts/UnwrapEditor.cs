﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;


[CustomEditor(typeof(UnwrapObject))]
public class UnwrapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        UnwrapObject u = (UnwrapObject)target;
        if (GUILayout.Button("Calculate UVs"))
        {
            u.GenerateUVs();
        }
    }
}
