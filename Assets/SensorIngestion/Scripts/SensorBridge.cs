using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;
using System.Xml.Serialization;

public static class StringExtensions
{
    public static bool IsNullOrWhiteSpace(string value)
    {
        if (value != null)
        {
            for (int i = 0; i < value.Length; i++)
            {
                if (!char.IsWhiteSpace(value[i]))
                    return false;
            }
        }
        return true;
    }
}

public class SensorBridge : MonoBehaviour {
    public GameObject loginUi;
    public InputField jobIdInput;

    private int jobId;
    private bool sensorsComplete = false;
    public Node[] nodes;

    // Reference to api actionables
    private APICall api;

    private bool willUpdate = true;

    public class Reading
    {
        [XmlElement("dataID")]
        public int dataID;
        [XmlElement("sensorID")]
        public int SensorID;
        [XmlElement("raw")]
        public int Raw;
        [XmlElement("engUnit")]
        public int EngUnit;
        [XmlElement("engUnit")]
        public XmlDateTime TimeStamp;
    }

    public class Sensor
    {
        [XmlElement("sensorID")]
        public int SensorID;
        [XmlElement("input")]
        public int Input;
        [XmlElement("sensorTypeID")]
        public int SensorTypeID;
        [XmlElement("sensorTypeName")]
        public string SensorTypeName;
        [XmlElement("type")]
        public int Type;
        [XmlElement("created")]
        public XmlDateTime Created;
        [XmlElement("modified")]
        public XmlDateTime Modified;
        [XmlElement("offset")]
        public int Offset;
        [XmlElement("custom1")]
        public int Custom1;
        [XmlElement("custom2")]
        public int Custom2;
        [XmlElement("custom3")]
        public int Custom3;
        [XmlElement("comments")]
        public string Comments;
    }

    public class Node
    {
        [XmlElement("nodeID")]
        public int NodeID;
        [XmlElement("phyID")]
        public int PhyID;
        [XmlElement("name")]
        public string Name;
        [XmlElement("created")]
        public XmlDateTime Created;
        [XmlElement("modified")]
        public XmlDateTime Modified;
        [XmlIgnore]
        public Sensor[] sensors;
    }

    [XmlType("result")]
    public class SensorList
    {
        [XmlArray("sensors")]
        [XmlArrayItem("sensor", typeof(Sensor))]
        public Sensor[] Sensors;
    }

    [XmlType("result")]
    public class NodeList
    {
        [XmlArray("nodes")]
        [XmlArrayItem("node", typeof(Node))]
        public Node[] Nodes;
    }

    public class XmlDateTime : IXmlSerializable
    {
        public System.DateTime Value { get; set; }
        public bool HasValue { get { return Value != System.DateTime.MinValue; } }
        private const string XML_DATE_FORMAT = "yyyy-MM-dd' 'HH:mm:ss";

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            if (reader.IsEmptyElement)
            {
                reader.ReadStartElement();
                return;
            }

            string someDate = reader.ReadInnerXml();
            if (StringExtensions.IsNullOrWhiteSpace(someDate) == false)
            {
                Value = XmlConvert.ToDateTime(someDate, XML_DATE_FORMAT);
            }
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            if (Value == System.DateTime.MinValue)
                return;

            writer.WriteRaw(XmlConvert.ToString(Value, XML_DATE_FORMAT));
        }

        public static implicit operator System.DateTime(XmlDateTime custom)
        {
            return custom.Value;
        }

        public static implicit operator XmlDateTime(System.DateTime custom)
        {
            return new XmlDateTime() { Value = custom };
        }
    }

    class APICall
    {
        string apiURL = "https://analytics.smtresearch.ca/api/";

        private string ConstructURL(APICommand c)
        {
            return apiURL + c.ToString();
        }

        public IEnumerator DoAction(APICommand c, System.Action<string> callback)
        {
            Debug.Log("Doing action");
            string cookiehead = "PHPSESSID=" + GlobalVariables.phpsessid;
            UnityWebRequest www = UnityWebRequest.Get(ConstructURL(c));
            www.SetRequestHeader("Cookie", cookiehead);
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError) // if we have an error, log it
            {
                Debug.Log(www.error);
            }
            else // if not, show what we got in the request
            {
                string sensor_xml = www.downloadHandler.text;
                callback(sensor_xml);
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

        public APICommand ListSensor(int nodeID)
        {
            var p = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("nodeID", nodeID.ToString())
            };

            var c = new APICommand("listSensor", p);
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

    // Use this for initialization
    void Start () {
        api = new APICall();
	}
	
	// Update is called once per frame
	void Update () {
        if (willUpdate == true && nodes != null)
        {
            for (int i = 0; i < nodes.Length; i++)
            {
                SetSensorsFromNode(nodes[i].NodeID, i);
            }

            willUpdate = false;
        }
        else if(sensorsComplete)
        {
            foreach(Node n in nodes)
            {
                Debug.Log(n.NodeID);
                foreach (Sensor s in n.sensors)
                    Debug.Log(s.SensorID);
            }
        }
	}

    public void AllowLogin()
    {
        if (!loginUi.activeSelf)
            loginUi.SetActive(true);
        else
            loginUi.SetActive(false);
    }

    public void SetNodes()
    {
        jobId = int.Parse(jobIdInput.text);

        StartCoroutine(api.DoAction(api.ListNode(jobId), s =>
        {
            XmlSerializer d = new XmlSerializer(typeof(NodeList));
            using (var r = new System.IO.StringReader(s))
            {
                NodeList n = (NodeList)d.Deserialize(r);
                nodes = n.Nodes;
            }
        }));
    }

    public void SetSensorsFromNode(int nodeId, int i)
    {
        StartCoroutine(api.DoAction(api.ListSensor(nodeId), s =>
        {
            XmlSerializer d = new XmlSerializer(typeof(SensorList));
            using (var r = new System.IO.StringReader(s))
            {
                SensorList l = (SensorList)d.Deserialize(r);
                nodes[i].sensors = l.Sensors;

                if (i == nodes.Length-1)
                    sensorsComplete = true;
            }
        }));
    }
}
