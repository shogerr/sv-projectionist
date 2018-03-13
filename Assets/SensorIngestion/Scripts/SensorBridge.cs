﻿using System.Collections;
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
    private static SensorBridge _instance;

    public static SensorBridge Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;
    }

    public InputField jobIdInput;

    private int jobId;
    public Node[] nodes;

    // Reference to api actionables
    public APICall api;

    // Have all of the sensors been updated?
    private bool sensorsComplete = false;

    // Should they be updated?
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
        [XmlElement("timestamp")]
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

    [XmlType("result")]
    public class SensorReadingList
    {
        [XmlArray("readings")]
        [XmlArrayItem("reading", typeof(Reading))]
        public Reading[] readings;
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

    public class APICall
    {
        string apiURL = "https://analytics.smtresearch.ca/api/";

        private string loginCredential;

        private string ConstructURL(APICommand c)
        {
            return apiURL + c.ToString();
        }

        public void setLoginCredential(string s)
        {
            loginCredential = s;
        }

        public IEnumerator Request(APICommand c, System.Action<string> callback)
        {
            Debug.Log("Doing action");
            string cookiehead = "PHPSESSID=" + loginCredential;
            UnityWebRequest www = UnityWebRequest.Get(ConstructURL(c));
            www.SetRequestHeader("Cookie", cookiehead);
            yield return www.SendWebRequest();

            // if we have an error, log it
            if (www.isNetworkError || www.isHttpError) 
            {
                Debug.Log(www.error);
            }
            // if not, show what we got in the request
            else 
            {
                string sensorXml = www.downloadHandler.text;
                Debug.Log(sensorXml);
                callback(sensorXml);
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

        public APICommand LoginUser(string user, string pass)
        {
            var p = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("user_username", user),
                new KeyValuePair<string, string>("user_password", pass)
            };

            var c = new APICommand("login", p);
            return c;
        }
    }

    public class APICommand
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
        if (!willUpdate)
            return;

        if (nodes == null)
        {
            SetNodes();
        }
        else
        {
            for (int i = 0; i < nodes.Length; i++)
            {
                SetSensorsFromNode(nodes[i].NodeID, i);
            }

            willUpdate = false;
        }
	}

    public void SetNodes()
    {
        jobId = int.Parse(jobIdInput.text);

        StartCoroutine(api.Request(api.ListNode(jobId), s =>
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
        StartCoroutine(api.Request(api.ListSensor(nodeId), s =>
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

    public void LoginUser(string username, string password)
    {
        APICommand c = api.LoginUser(username, password);
        StartCoroutine(api.Request(c, s =>
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(s);

            XmlNodeList elemList = xDoc.GetElementsByTagName("PHPSESSID");
            if (elemList[0] != null)
                api.setLoginCredential((elemList[0].InnerXml));
        }));
    }
    
    public bool SensorsComplete()
    {
        return sensorsComplete;
    }
}