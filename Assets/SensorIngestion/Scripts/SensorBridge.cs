using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using System.Xml;

public class SensorBridge : MonoBehaviour {
    public GameObject ui;
    class APICall
    {
        string apiURL = "https://analytics.smtresearch.ca/api/";

        private string constructURL(APICommand c)
        {
            return apiURL + c.ToString();
        }

        public IEnumerator DoAction(APICommand c)
        {
            Debug.Log("Doing action");
            string cookiehead = "PHPSESSID=" + GlobalVariables.phpsessid;
            UnityWebRequest www = UnityWebRequest.Get(constructURL(c));
            www.SetRequestHeader("Cookie", cookiehead);
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError) // if we have an error, log it
            {
                Debug.Log(www.error);
            }
            else // if not, show what we got in the request
            {
                // Populate the dropdown
                string sensor_xml = www.downloadHandler.text;
                XmlDocument xDoc = new XmlDocument();
                xDoc.LoadXml(sensor_xml);
                Debug.Log(sensor_xml);
            }
        }
        public APICommand ListSensorData(int sensorID)
        {
            var p = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("sensorID", sensorID.ToString())
            };

            var c = new APICommand("listSensorData", p);
            return c;
        }
        public APICommand ListNode(int jobID)
        {
            var p = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("jobID", jobID.ToString())
            };

            var c = new APICommand("listNode", p);
            return c;
        }
    }

    class APICommand
    {
        string action;
        List<KeyValuePair<string, string>> parameters;

        public APICommand(string a, List<KeyValuePair<string, string>> p)
        {
            action = a;
            parameters = p;
        }

        public override string ToString()
        {
            string parameterString = "";
            foreach (KeyValuePair<string, string> p in parameters)
            {
                parameterString += "&" + p.Key + "=" + p.Value;
            }
            return "?action=" + action + parameterString;
        }
    }

    private APICall api;
    // Use this for initialization
    void Start () {
        api = new APICall();
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

    public void GetNodes()
    {
        StartCoroutine(api.DoAction(api.ListNode(3157)));
    }
}
