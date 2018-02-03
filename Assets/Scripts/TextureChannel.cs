using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TextureChannel : NetworkBehaviour {
    // Sending texture
    public Texture2D texture_;

    // Recieving texture
    private Texture2D _texture;

	// Update is called once per frame
	void Update () {
        if (Network.isServer)
        {
            if(Input.GetKeyDown(KeyCode.X))
            {
                RpcSend(texture_.EncodeToPNG());
            }
        }
	}
    [ClientRpc]
    private void RpcSend(byte[] r)
    {
        _texture = new Texture2D(1, 1);
        _texture.LoadImage(r);
    }
}
