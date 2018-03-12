using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.XR;
using Vuforia;

public class WorldState : NetworkBehaviour {

    void PlaceMainCameraAtSensor()
    {
        Renderer m = GameObject.Find("Cube").transform.GetComponent<Renderer>();

        Vector3 offset = Vector3.Scale(new Vector3(.95f, 1, 1), (m.bounds.center - Camera.main.transform.position));
        Camera.main.transform.position += offset;

        Camera.main.transform.LookAt(m.bounds.center);
    }

    void PlaceMainCamera()
    {
        Transform s = GameObject.Find("Structures").transform;
        var m = s.GetComponentInChildren<Renderer>();

        Vector3 offset = Vector3.Scale(new Vector3(.95f, .95f, 1.5f), (m.bounds.center - Camera.main.transform.position));
        Camera.main.transform.position = offset;

        Camera.main.transform.LookAt(m.bounds.center);
    }

	// Use this for initialization
	void Start () {
        PlaceMainCameraAtSensor();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
