using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;


[CustomEditor(typeof(Structures))]
public class StructuresEditor : Editor
{
    bool meshActive = true;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Structures u = (Structures)target;
        if (GUILayout.Button("Set All Children Material"))
        {
            u.SetsAllChildrenMaterial();
        }
        if (GUILayout.Button("Calculate All Children UVs"))
        {
            u.GenerateAllChildrenUVs();
        }
        if (GUILayout.Button("Calculate Object UVs"))
        {
            u.GenerateUV();
        }
        if (GUILayout.Button("Add MeshCollider to all Children"))
        {
            u.SetAllChildrenColliders();
        }
        if (GUILayout.Button("Cull Non-Mesh"))
        {
            u.CullNonMeshFromStructures();
        }

        if (GUILayout.Button("Activate Non-Mesh"))
        {
            Debug.Log(meshActive);
            u.DeactivateNonMesh();
        }
    }
}
