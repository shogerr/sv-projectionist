using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SendCommand : MonoBehaviour {
    public UDPSend udp;
    public Slider[] slider;
    public Button next;

    private int currentView;

    Command cmd;

	// Use this for initialization
	void Start () {
        currentView = 0;

        cmd = new Command();
        Debug.Log(slider.Length);
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
        udp.sendString(JsonUtility.ToJson(cmd));
    }

    // JSON ready class for serialization
    [System.Serializable]
    public class Command
    {
        public string name;
        public float value;
    }
}
