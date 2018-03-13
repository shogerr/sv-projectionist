using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorController : MonoBehaviour {
    public SensorBridge sensorBridge;

    private static SensorController _instance;

    private static SensorController Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;
    }
	// Use this for initialization
	void Start () {
        sensorBridge = SensorBridge.Instance;
        Debug.Log(sensorBridge);
	}
	
	// Update is called once per frame
	void Update () {
	}
}
