using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.XR;
using Vuforia;

public class WorldState : NetworkBehaviour {

	// Use this for initialization
	void Start () {
        Transform s = GameObject.Find("Structures").transform;
        var m = s.GetComponentInChildren<Renderer>();

        Vector3 offset = Vector3.Scale(new Vector3(.25f, .25f, 1.5f), (m.bounds.center - Camera.main.transform.position));
        Camera.main.transform.position = offset;

        Camera.main.transform.LookAt(m.bounds.center);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
