using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SendCommand : MonoBehaviour {
    public UDPSend udp;
    public Slider[] slider;
    public Button next;

    Command cmd;

	// Use this for initialization
	void Start () {
        cmd = new Command();
        // Add slider listeners
        for (int i = 0; i < slider.Length; i++)
        {
            int j = i;
            slider[i].onValueChanged.AddListener(delegate { UpdateValue(j); });
        }
        // Add button listener
        //next.GetComponent<Button>().onClick.AddListener(NextView);
	}
	
	// Update is called once per frame
	void Update () {
        //GetComponent<UDPSend>().sendString("tester");
	}

    // Send UDP tx to send commands
    public void UpdateValue(int i)
    {
        Debug.Log(i);
        cmd.name = slider[i].name;
        cmd.value = slider[i].value;
        udp.SendString(JsonUtility.ToJson(cmd));
    }

    public void SendVisualization()
    {
        VisualizationData v = new VisualizationData();
        Sensor s = GameObject.Find("SensorController").GetComponent<SensorController>().ActiveSensorObject;
        v.visualization = GameObject.Find("VisualizationSelection").GetComponentInChildren<Text>().text.ToLower();
        v.values = s.ReadingsArray();
        udp.SendString(JsonUtility.ToJson(v));
    }

    // JSON ready class for serialization
    [System.Serializable]
    public class Command
    {
        public string name;
        public float value;
    }

    [System.Serializable]
    public class Visualization
    {
        public int SensorID;
        public string StartDate;
        public string EndDate;
    }
    [System.Serializable]
    public class VisualizationData
    {
        public string visualization;
        public double[] values; 
    }
}
