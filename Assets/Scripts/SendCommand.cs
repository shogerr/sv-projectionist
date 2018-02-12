using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SendCommand : MonoBehaviour {
    public UDPSend udp;
    public Slider slider;


    Command cmd;

	// Use this for initialization
	void Start () {
        cmd = new Command();
        slider.onValueChanged.AddListener(delegate { UpdateValue(); });
        //udp = UDPSend.GetComponent<UDPSend>();	
	}
	
	// Update is called once per frame
	void Update () {
        //GetComponent<UDPSend>().sendString("tester");
	}

    public void UpdateValue()
    {
        cmd.value = slider.value;
        udp.sendString(JsonUtility.ToJson(cmd));
    }

    [System.Serializable]
    public class Command
    {
        public float value;
    }
}
