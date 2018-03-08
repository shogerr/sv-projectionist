using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Xml;

public class select_sensor : MonoBehaviour {

    public Dropdown dropdown;
    int check = 25;

    static List<string> ids;

    string url1 = "https://analytics.smtresearch.ca/api/?action=listSensor&nodeID=25750";
    string url2 = "https://analytics.smtresearch.ca/api/?action=listSensor&nodeID=25752";
    string url3 = "https://analytics.smtresearch.ca/api/?action=listSensor&nodeID=25751";
    string url4 = "https://analytics.smtresearch.ca/api/?action=listSensor&nodeID=25815";
    string url5 = "https://analytics.smtresearch.ca/api/?action=listSensor&nodeID=26168";
    string url6 = "https://analytics.smtresearch.ca/api/?action=listSensor&nodeID=25631";

    string dataurl = "https://analytics.smtresearch.ca/api/?action=listSensorData&sensorID=";

        


    // Use this for initialization
    public void Start () {
        PopulateList();
	}

    public void Update()
    {
        if (check == GlobalVariables.current_node)
        {
            return;
        }
        else
        {
            dropdown.ClearOptions();
            PopulateList();
        }
        
    }


    void PopulateList()
    {
        check = GlobalVariables.current_node;
        switch (GlobalVariables.current_node) // list different sensors based on selected node
        {
            case 1:
                StartCoroutine(get_sensors(url1));
                break;
            case 2:
                StartCoroutine(get_sensors(url2));
                break;
            case 3:
                StartCoroutine(get_sensors(url3));
                break;
            case 4:
                StartCoroutine(get_sensors(url4));
                break;
            case 5:
                StartCoroutine(get_sensors(url5));
                break;
            case 6:
                StartCoroutine(get_sensors(url6));
                break;
            default:
                dropdown.ClearOptions();
                break;
        }
    }



    public IEnumerator get_sensors(string url)
    {
        string cookiehead = "PHPSESSID=" + GlobalVariables.phpsessid;
        UnityWebRequest www = UnityWebRequest.Get(url);
        www.SetRequestHeader("Cookie", cookiehead);
        yield return www.SendWebRequest(); // 

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

            int n;

            XmlNodeList elemList = xDoc.GetElementsByTagName("name");
            XmlNodeList sensor_ids = xDoc.GetElementsByTagName("sensorID");

            ids = new List<string>(sensor_ids.Count);
            List<string> sensors = new List<string>(elemList.Count);

            for (n = 0; n < elemList.Count; n++)
            {
                string id = sensor_ids[n].InnerXml;
                ids.Add(id);
                Debug.Log(ids[n]);
                string s = elemList[n].InnerXml; // string cleanup
                string s2 = s.Remove(0, 9);
                string s3 = s2.Trim(new char[] { ']', '>' });
                sensors.Add(s3);
            }
            dropdown.AddOptions(sensors);
        }
    }

    public void fetch_data(int index)
    {
        GlobalVariables.selected_sensor = index;
        switch (GlobalVariables.selected_sensor) // list different sensors based on selected node
        {
            case 1:
                GlobalVariables.selected_sensor = 1;
                break;
            case 2:
                GlobalVariables.selected_sensor = 2;
                break;
            case 3:
                GlobalVariables.selected_sensor = 3;
                break;
            case 4:
                GlobalVariables.selected_sensor = 4;
                break;
            case 5:
                GlobalVariables.selected_sensor = 5;
                break;
            case 6:
                GlobalVariables.selected_sensor = 6;
                break;
            default:
                break;
        }

    }

    public void ingest_data()
    {
        string current_id = ids[GlobalVariables.selected_sensor];
        string requesturl = dataurl + current_id + "&startDate=2018-01-01&endDate=2018-03-05";
        StartCoroutine(get_readings(requesturl));
    }

    public IEnumerator get_readings(string url)
    {
        string cookiehead = "PHPSESSID=" + GlobalVariables.phpsessid;
        UnityWebRequest www = UnityWebRequest.Get(url);
        www.SetRequestHeader("Cookie", cookiehead);
        yield return www.SendWebRequest(); // 

        if (www.isNetworkError || www.isHttpError) // if we have an error, log it
        {
            Debug.Log(www.error);
        }
        else // if not, show what we got in the request
        {
            string readings_xml = www.downloadHandler.text;
            XmlDocument xDoc2 = new XmlDocument();
            xDoc2.LoadXml(readings_xml);
            int i;

            XmlNodeList readingList = xDoc2.GetElementsByTagName("raw");
            List<string> readings = new List<string>(readingList.Count);

            for (i = 0; i < readingList.Count; i++)
            {
                readings[i] = readingList[i].InnerXml;
                Debug.Log(readings[i]);
            }
        }
    }
}

