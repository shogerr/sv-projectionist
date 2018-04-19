using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;
using System.Xml.Serialization;
using System.Linq;

public class SensorBridge : MonoBehaviour {
    private static SensorBridge _instance;

    public static SensorBridge Instance { get { return _instance; } }

    public InputField jobIdInput;

    private int jobId;
    public List<Node> nodes;

    // Reference to api actionables
    public SmtApiCaller api;

    public bool LoggedIn
    {
        set { }
        get {
            if (api == null) return false;
            return api.LoginCredential == null ? false : true;
        }
    }
    // Have all of the sensors been updated?
    private bool sensorsComplete = false;

    public class Reading
    {
        [XmlElement("dataID")]
        public double dataID;
        //[XmlElement("sensorID")]
        public int SensorID;
        [XmlElement("raw")]
        public double Raw;
        [XmlElement("engUnit")]
        public float EngUnit;
        [XmlElement("timestamp")]
        public XmlDateTime TimeStamp;
    }

    public class Sensor
    {
        [XmlElement("sensorID")]
        public int SensorID;
        [XmlElement("name")]
        public string Name;
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
        public List<Sensor> sensors;

        public override string ToString()
        {
            var s = "";
            var allFields = this.GetType().GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            foreach (var f in allFields)
            {
                s += f.Name + " : " + f.GetValue(this) + "\n";
            }

            return s;
        }
    }

    [XmlType("result")]
    public class SensorList
    {
        [XmlArray("sensors")]
        [XmlArrayItem("sensor", typeof(Sensor))]
        public List<Sensor> Sensors;
    }

    [XmlType("result")]
    public class NodeList
    {
        [XmlArray("nodes")]
        [XmlArrayItem("node", typeof(Node))]
        public List<Node> Nodes;
    }

    [XmlType("result")]
    public class SensorReadingList
    {
        [XmlArray("readings")]
        [XmlArrayItem("reading", typeof(Reading))]
        public List<Reading> readings;
    }

    public class XmlDateTimeMilliSeconds : XmlDateTime
    {
        private const string XML_DATE_FORMAT = "yyyy-MM-dd' 'HH:mm:ss.FFF";

        public static implicit operator System.DateTime(XmlDateTimeMilliSeconds custom)
        {
            return custom.Value;
        }

        public static implicit operator XmlDateTimeMilliSeconds(System.DateTime custom)
        {
            return new XmlDateTimeMilliSeconds() { Value = custom };
        }
    }

    public class XmlDateTime : IXmlSerializable
    {
        public System.DateTime Value { get; set; }
        public bool HasValue { get { return Value != System.DateTime.MinValue; } }
        private const string XML_DATE_FORMAT = "yyyy-MM-dd' 'HH:mm:ss.FFF";

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

    public class SmtApiCaller
    {
        string apiURL = "https://analytics.smtresearch.ca/api/";

        private string loginCredential;

        public string LoginCredential
        {
            set
            {
                loginCredential = value;
            }
            get
            {
                return loginCredential;
            }
        }

        private string ConstructURL(APICommand c)
        {
            return apiURL + c.ToString();
        }

        public IEnumerator Request(APICommand c, System.Action<string> callback)
        {
            Debug.Log("Doing Request");
            UnityWebRequest www = UnityWebRequest.Get(ConstructURL(c));

            if (loginCredential != null)
            {
                string cookiehead = "PHPSESSID=" + loginCredential;
                www.SetRequestHeader("Cookie", cookiehead);
            }

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

        public APICommand ListSensorData(int sensorID, string startDate, string endDate)
        {
            var p = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("sensorID", sensorID.ToString()),
                new KeyValuePair<string, string>("startDate", startDate),
                new KeyValuePair<string, string>("endDate", endDate)
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

        public APICommand Login(string user, string pass)
        {
            var p = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("user_username", user),
                new KeyValuePair<string, string>("user_password", pass)
            };

            var c = new APICommand("login", p);
            return c;
        }

        public APICommand Logout()
        {
            var c = new APICommand("logout", null);
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
            if (parameters != null)
            {
                string parameterString = "";
                foreach (KeyValuePair<string, string> p in parameters)
                {
                    parameterString += "&" + p.Key + "=" + p.Value;
                }
                return "?action=" + action + parameterString;
            }
            else return "?action=" + action;
        }
    }
    private void Awake()
    {
        // Make the object a singleton
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;

        // Create the api reference 
        api = new SmtApiCaller();
    }

    // Wrapper to allow UI button call of UpdateBridge
    public void UpdateBridgeCaller()
    {
        SensorBridge.Instance.nodes = null;
        StartCoroutine(UpdateBridge());
    }

    // Update all nodes and sensors
    public IEnumerator UpdateBridge() {
        if (nodes == null)
            SetNodes();

        while(nodes == null)
            yield return null;

        for (int i = 0; i < nodes.Count; i++)
            SetSensorsFromNode(nodes[i].NodeID, i);
	}

    // Set the nodes SensorBridge holds
    public void SetNodes()
    {
        if (api == null || api.LoginCredential == null)
            return;

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

    // Create a list of sensors for a given node
    public void SetSensorsFromNode(int nodeId, int i)
    {
        if (api.LoginCredential == null)
            return;

        StartCoroutine(api.Request(api.ListSensor(nodeId), s =>
        {
            XmlSerializer d = new XmlSerializer(typeof(SensorList));
            using (var r = new System.IO.StringReader(s))
            {
                SensorList l = (SensorList)d.Deserialize(r);
                nodes[i].sensors = l.Sensors;

                if (i == nodes.Count-1)
                    sensorsComplete = true;
            }
        }));
    }

    public void LoginUser(string username, string password)
    {
        APICommand c = api.Login(username, password);
        StartCoroutine(api.Request(c, s =>
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(s);

            XmlNodeList elemList = xDoc.GetElementsByTagName("PHPSESSID");
            if (elemList[0] != null)
                api.LoginCredential = (elemList[0].InnerXml);
        }));
    }

    public void Logout()
    {
        StartCoroutine(api.Request(api.Logout(), s => { }));
        api.LoginCredential = null;
    }

    public Node FindNode(int nodeID)
    {
        return nodes.Find(x => x.NodeID == nodeID);
    }

    // Find a sensor by ID value.
    public Sensor FindSensor(int sensorID)
    {
        return (from n in nodes
                from s in n.sensors
                where s.SensorID == sensorID
                select s).FirstOrDefault();
    }

    public bool SensorsComplete()
    {
        return sensorsComplete;
    }
}
