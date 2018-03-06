using System.Collections;
using System.Collections.Generic;using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CustomNetworkHUD : MonoBehaviour
{
    public NetworkManager manager;
    public bool showGUI = true;
    public int offsetX;
    public int offsetY;
    public int fontSize = 24;

    void Awake()
    {
        manager = GetComponent<NetworkManager>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (showGUI == true)
                showGUI = false;
            else
                showGUI = true;
        }

        if (!showGUI)
            return;

        if (!NetworkClient.active && !NetworkServer.active && manager.matchMaker == null)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                manager.StartServer();
            }
            if (Input.GetKeyDown(KeyCode.H))
            {
                manager.StartHost();
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                manager.StartClient();
            }
        }
        if (NetworkServer.active && NetworkClient.active)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                manager.StopHost();
            }
        }
    }

    void OnGUI()
    {
        if (!showGUI)
            return;

        int xpos = 10 + offsetX;
        int ypos = 10 + offsetY;
        int buttonWidth = Screen.width - 20;
        int buttonHeight = Screen.height / 5;
        int squareButtonDim = Screen.width / 12;
        int spacing = buttonHeight;

        // Setup Button style
        GUIStyle aButton = new GUIStyle(GUI.skin.button);
        aButton.fontSize = fontSize;
        // Setup TextField style
        GUIStyle someText = new GUIStyle(GUI.skin.textField);
        someText.fontSize = fontSize;
        someText.alignment = TextAnchor.MiddleCenter;
        // Set Text font size
        GUIStyle someLabel = new GUIStyle(GUI.skin.label);
        someLabel.fontSize = fontSize;

        if (NetworkServer.active || NetworkClient.active)
        {
            if (GUI.Button(new Rect(xpos, ypos, squareButtonDim, squareButtonDim), "X"))
            {
                manager.StopHost();
            }
            ypos += spacing;
        }

        // Initial connection menu
        if (!NetworkClient.active && !NetworkServer.active && manager.matchMaker == null)
        {
            /*
            if (GUI.Button(new Rect(xpos, ypos, 200, 20), "LAN Host(H)"))
            {
                manager.StartHost();
            }
            ypos += spacing;
            */

            // Lan client button
            if (GUI.Button(new Rect(xpos, ypos, buttonWidth/2, buttonHeight), "LAN Client (C)", aButton))
            {
                manager.StartClient();
            }
            // IP input
            manager.networkAddress = GUI.TextField(new Rect(xpos + buttonWidth/2, ypos, buttonWidth/2, buttonHeight), manager.networkAddress, someText);
            ypos += spacing;
            
            // Start Server
            if (GUI.Button(new Rect(xpos, ypos, buttonWidth, buttonHeight), "LAN Server Only (S)", aButton))
            {
                manager.StartServer();
            }
            ypos += spacing;
        }
        else
        {
            if (NetworkServer.active)
            {
                GUI.Label(new Rect(xpos, ypos, buttonWidth, buttonHeight), "Server: port=" + manager.networkPort, someLabel);
                ypos += spacing;
            }
            if (NetworkClient.active)
            {
                GUI.Label(new Rect(xpos, ypos, buttonWidth, buttonHeight), "Client: address=" + manager.networkAddress + " port=" + manager.networkPort, someLabel);
                ypos += spacing;
            }
        }

        if (NetworkClient.active && !ClientScene.ready)
        {
            if (GUI.Button(new Rect(xpos, ypos, buttonWidth, buttonHeight), "Client Ready"))
            {
                ClientScene.Ready(manager.client.connection);

                if (ClientScene.localPlayers.Count == 0)
                {
                    ClientScene.AddPlayer(0);
                }
            }
            ypos += spacing;
        }

    }
}
