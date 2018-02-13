using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SendCommand : MonoBehaviour {
    public UDPSend udp;
    public Slider[] slider;

    Command cmd;

	// Use this for initialization
	void Start () {
        cmd = new Command();
        Debug.Log(slider.Length);
        for (int i = 0; i < slider.Length; i++)
        {
            int j = i;
            slider[i].onValueChanged.AddListener(delegate { UpdateValue(j); });
        }
	}
	
	// Update is called once per frame
	void Update () {
        //GetComponent<UDPSend>().sendString("tester");
	}

    public void UpdateValue(int i)
    {
        Debug.Log(i);
        cmd.name = slider[i].name;
        cmd.value = slider[i].value;
        udp.sendString(JsonUtility.ToJson(cmd));
    }

    [System.Serializable]
    public class Command
    {
        public string name;
        public float value;
    }
}
