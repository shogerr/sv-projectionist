using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class UnwrapObject : MonoBehaviour {
    public GameObject target;
    public void GenerateUVs()
    {
        var meshes = target.GetComponentsInChildren<MeshFilter>();

        for (int j = 0; j < 10; j++)
        {
            Mesh mesh = meshes[1].mesh;
            Vector3[] vertices = mesh.vertices;
            Vector3[] normals = mesh.normals;
            Vector2[] uvs = new Vector2[vertices.Length];
            int i = 0;
            while (i < uvs.Length)
            {
                /*
                if (Mathf.Abs(normals[i].y) > 0.5f)
                {  // if normal is like vector3.up
                    uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
                }
                else if (Mathf.Abs(normals[i].x) > 0.5f)
                {  // if normal is like vector3.right
                    uvs[i] = new Vector2(vertices[i].z, vertices[i].y);
                }
                */
                if (Mathf.Abs(normals[i].z) > .5f)
                { // last case if it's like vector3.forward
                    uvs[i] = new Vector2(vertices[i].x, vertices[i].y);
                }
                i++;
            }
            mesh.uv = uvs;
            foreach (var uv in uvs)
            {
                Debug.Log(uv);
            }
            //meshes[i].sharedMesh.uv = Unwrapping.GeneratePerTriangleUV(meshes[i].sharedMesh);
        }
    }
} 