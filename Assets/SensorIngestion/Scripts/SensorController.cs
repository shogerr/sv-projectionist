using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SensorController : MonoBehaviour {
    public SensorBridge sensorBridge;

    private static SensorController _instance;

    public static SensorController Instance { get { return _instance; } }

    public Sensor ActiveSensorObject{get; set;}

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
	}

    void Update()
    {
        if (ActiveSensorObject != null && GameObject.Find("SensorController").GetComponentInChildren<Canvas>() != null)
            GameObject.Find("ActiveSensorText").GetComponent<UnityEngine.UI.Text>().text = ActiveSensorObject.name;
    }
	
    public void UpdateActiveSensor()
    {
        Debug.Log(GameObject.Find("StartDateField").GetComponent<UnityEngine.UI.InputField>().text);
        ActiveSensorObject.StartDate = System.DateTime.ParseExact(GameObject.Find("StartDateField").GetComponent<UnityEngine.UI.InputField>().text,
                                                                    "yyyy-MM-dd", 
                                                                    System.Globalization.CultureInfo.InvariantCulture);

        ActiveSensorObject.EndDate = System.DateTime.ParseExact(GameObject.Find("EndDateField").GetComponent<UnityEngine.UI.InputField>().text,
                                                                    "yyyy-MM-dd",
                                                                    System.Globalization.CultureInfo.InvariantCulture);
        ActiveSensorObject.UpdateSensorReadings();
    }

    public void DebugSensors()
    {
        foreach (SensorBridge.Node n in sensorBridge.nodes)
        {
            Debug.Log(n.ToString());
            foreach (SensorBridge.Sensor s in n.sensors)
            {
                Debug.Log(s);
            }
        }
    }

    public SensorBridge.Node FindNode(int nodeID)
    {
        return sensorBridge.nodes.Find(x => x.NodeID == nodeID);
    }

    public SensorBridge.Sensor FindSensor(int sensorID)
    {
        return (from n in sensorBridge.nodes
                from s in n.sensors
                where s.SensorID == sensorID
                select s).FirstOrDefault();
    }
}
