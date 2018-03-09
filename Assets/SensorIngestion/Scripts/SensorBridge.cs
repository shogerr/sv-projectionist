using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorBridge : MonoBehaviour {
    public GameObject ui;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AllowLogin()
    {
        Debug.Log(ui.activeSelf);
        if (!ui.activeSelf)
            ui.SetActive(true);
        else
            ui.SetActive(false);
    }
}
