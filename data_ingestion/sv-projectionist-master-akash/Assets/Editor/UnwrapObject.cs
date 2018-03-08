using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class UnwrapObject : MonoBehaviour {
    public Material setMaterial;

    public void GenerateAllChildrenUVs()
    {
        var meshes = GetComponentsInChildren<MeshFilter>();
        for (int j = 0; j < meshes.Length; j++)
        {
            Unwrapping.GenerateSecondaryUVSet(meshes[j].sharedMesh);
            meshes[j].sharedMesh.uv = meshes[j].sharedMesh.uv2;
        }
    }

    public void GenerateUV()
    {
        var m = GetComponent<MeshFilter>().mesh;
        var sm = GetComponent<MeshFilter>().sharedMesh;
        Unwrapping.GenerateSecondaryUVSet(sm);
        m.uv = sm.uv2;
    }

    public void GenerateRawUV(Mesh mesh)
    {
        Vector3[] vertices = mesh.vertices;
        Vector3[] normals = mesh.normals;
        Vector2[] uvs = new Vector2[vertices.Length];
        int i = 0;
        while (i < uvs.Length)
        {
            if (Mathf.Abs(normals[i].y) > 0.5f)
            {  // if normal is like vector3.up
                uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
            }
            else if (Mathf.Abs(normals[i].x) > 0.5f)
            {  // if normal is like vector3.right
                uvs[i] = new Vector2(vertices[i].z, vertices[i].y);
            }
            else
            { // last case if it's like vector3.forward
                uvs[i] = new Vector2(vertices[i].x, vertices[i].y);
            }
            i++;
        }
        mesh.uv = uvs;
    }

    public void SetsAllChildrenMaterial()
    {
        var c = GetComponentsInChildren<Renderer>(); 
        for (int i = 0; i < c.Length; i++)
        {
            c[i].material = setMaterial;
        }

    }
} 