using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetOject : MonoBehaviour {

    public GameObject targetGroup;
    private Transform[] t;

	// Use this for initialization
	void Start () {
        t = targetGroup.transform.GetComponentsInChildren<Transform>();
        Debug.Log(t[1].gameObject.GetComponent<MeshFilter>().mesh.normals[0]);
	}
	
	// Update is called once per frame
	void Update () {
        var target = t[1].gameObject.GetComponent<MeshFilter>().mesh;
        // Get a center point in world coords
        var p = t[1].localToWorldMatrix.MultiplyPoint3x4(target.vertices[0]);
        transform.position =  p + new Vector3(10, 0, 0);
        //transform.LookAt(t[1].position);
	}
}
