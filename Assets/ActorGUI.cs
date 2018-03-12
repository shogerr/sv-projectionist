using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorGUI : MonoBehaviour {

    int buttonDim = 32;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
     void OnGUI()
    {
        if (GUI.Button(new Rect(Screen.width-buttonDim, Screen.height-buttonDim, buttonDim, buttonDim), "s"))
        {

        }
    }
}
