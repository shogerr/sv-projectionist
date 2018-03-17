using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Structures : MonoBehaviour {
    public Material defaultMaterial;

    // Generate UV coordinates for children.
    public void GenerateAllChildrenUVs()
    {
        MeshFilter[] m = GetComponentsInChildren<MeshFilter>();
        for (int j = 0; j < m.Length; j++)
        {
            GenerateRawUV(m[j].mesh);
        }
    }

    public void GenerateUV()
    {
        Mesh m = GetComponent<MeshFilter>().mesh;
        GenerateRawUV(m);
    }

    public void GenerateRawUV(Mesh mesh)
    {
        Vector3[] vertices = mesh.vertices;
        Vector3[] normals = mesh.normals;
        Vector2[] uvs = new Vector2[vertices.Length];

        for (int i = 0; i < uvs.Length; i++)
        {
            // Normal is up.
            if (Mathf.Abs(normals[i].y) > 0.5f)
            {
                uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
            }
            // Normal is in X plane.
            else if (Mathf.Abs(normals[i].x) > 0.5f)
            { 
                uvs[i] = new Vector2(vertices[i].z, vertices[i].y);
            }
            // Normal is in Z.
            else
            {
                uvs[i] = new Vector2(vertices[i].x, vertices[i].y);
            }
        }

        // Set UV coordinates for object
        mesh.uv = uvs;
    }

    public void SetsAllChildrenMaterial()
    {
        var c = GetComponentsInChildren<Renderer>(); 
        for (int i = 0; i < c.Length; i++)
        {
            c[i].material = defaultMaterial;
        }

    }
    public void DeactivateNonMesh()
    {
        Transform[] c = GetComponentsInChildren<Transform>();
        foreach (Transform t in c)
        {
            // If object is not the root parent or itself
            if (t.gameObject.GetComponent<MeshFilter>() == null && t.parent != c[0] && t != c[0])
                t.gameObject.SetActive(false);
        }
    }

    public void CullNonMeshFromStructures()
    {
        Transform[] c = GetComponentsInChildren<Transform>();
        foreach (Transform t in c)
        {
            if (t.gameObject.GetComponent<MeshFilter>() == null && t.parent != c[0] && t != c[0])
                DestroyImmediate(t.gameObject);
        }
    }

    public void AddColliderToComponent(GameObject o)
    {
        o.AddComponent<MeshCollider>();
    }

    public void SetAllChildrenColliders()
    {
        Transform[] c = GetComponentsInChildren<Transform>();
        foreach (Transform t in c)
        {
            if (t.gameObject.GetComponent<MeshFilter>() != null)
                AddColliderToComponent(t.gameObject);
        }
    }
} 