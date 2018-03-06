using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Vuforia;

public class CustomManager : NetworkManager {

    public override void OnClientSceneChanged(NetworkConnection conn)
    {
        Debug.Log("Found Client.");
        // Delete main camera so only one remains.
        //Destroy(Camera.main.gameObject);
        // Additively load the scene, keeping the network manager.
        SceneManager.LoadScene("Viewfinder", LoadSceneMode.Additive);
        ClientScene.Ready(conn);
        ClientScene.AddPlayer(conn, 0);
        //VuforiaRuntime.Instance.InitVuforia();
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        Debug.Log("Found Server.");
        //Destroy(Camera.main.gameObject);
        SceneManager.LoadScene("Projectionist", LoadSceneMode.Additive);
    }
}
