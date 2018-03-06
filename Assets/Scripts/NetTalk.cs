using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class NetTalk : NetworkBehaviour {

    [Command]
    void CmdSayHello()
    {
        GameObject.Find("Cube").GetComponent<Renderer>().material = (Material)Resources.Load("Materials/projection", typeof(Material));
        //Debug.Log("Hello from " + SceneManager.GetActiveScene().name);
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (isLocalPlayer)
        {
            CmdSayHello();
        }
	}
}
