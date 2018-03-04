using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.XR;
using Vuforia;

public class WorldState : NetworkBehaviour {

	// Use this for initialization
	void Start () {
        //VuforiaBehaviour.Instance.enabled = false;
        /*
		if (isServer)
        {
            XRSettings.enabled = false;
        }
        */
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
