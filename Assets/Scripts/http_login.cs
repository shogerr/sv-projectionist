using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System.Xml;
using System.IO;

// PURPOSE OF THIS SCRIPT IS TO ACQUIRE THE PHPSESSID REQUIRED FOR ALL HTTP REQUESTS
public class http_login : MonoBehaviour
{
    // Use this for initialization
    public void Start()
    {
        UnityWebRequest www = UnityWebRequest.Get("https://analytics.smtresearch.ca/api/?action=logout");
        www.SendWebRequest(); // 
        StartCoroutine(GetText("https://analytics.smtresearch.ca/api/?action=login&user_username=osuuser&user_password=peavyhall"));
    }

    // Update is called once per frame
    public IEnumerator GetText(string URL)
    {
        UnityWebRequest www = UnityWebRequest.Get(URL);
        yield return www.SendWebRequest(); // 

        if (www.isNetworkError || www.isHttpError) // if we have an error, log it
        {
            Debug.Log(www.error);
        }
        else // if not, show what we got in the request
        {
            // Show results as text
            string login_xml = www.downloadHandler.text;
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(login_xml);


            XmlNodeList elemList = xDoc.GetElementsByTagName("PHPSESSID");
            GlobalVariables.phpsessid = (elemList[0].InnerXml);

        }
    }
}




public static class GlobalVariables
{
    public static string phpsessid;
    public static int selected_nodeID;
    public static int selected_sensor;
    public static int current_node;
}
