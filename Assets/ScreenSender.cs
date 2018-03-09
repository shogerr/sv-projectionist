using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ScreenSender : NetworkBehaviour {
    public Material _material;

    [ClientRpc(channel=1)]
    void RpcScreenTexture(byte[] recieved)
    {
        var recievedTexture = new Texture2D(1, 1);
        recievedTexture.LoadImage(recieved);
        GameObject.Find("Cube").GetComponent<Renderer>().material.mainTexture = recievedTexture;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!isServer)
            return;
        //Texture2D t = (_material.GetTexture("_MainTex") as Texture2D);
        Texture2D t = new Texture2D(1, 1);
        RpcScreenTexture(t.EncodeToPNG());

	}
}
