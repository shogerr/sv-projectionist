using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExplorerPanel : MonoBehaviour
{

    public GameObject panel;

    private Font ArialFont;
    // Use this for initialization
    void Start()
    {
        ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
        var buttons = GetComponentsInChildren<Button>();
        buttons[0].onClick.AddListener(() => { DisplayNodes(); });
        buttons[1].onClick.AddListener(() => { DisplaySensors(); });
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void ClearDisplay()
    {
        var children = panel.GetComponentsInChildren<Transform>();
        Debug.Log("Destroying " + children.Length);
        int i = 0;
        foreach (var c in children)
        {
            i++;
            if (i == 1)
                continue;

            Debug.Log("Destroying" + c.name);
            Destroy(c.gameObject);
        }
    }

    private void DisplayNodes()
    {
        ClearDisplay();
        Debug.Log("Displaying Nodes");
        List<SensorBridge.Node> nodes = SensorBridge.Instance.nodes;
        if (nodes == null)
            return;

        foreach (var n in nodes)
        {
            AddText(n.Name, n.Name, panel.transform);
        }

    }

    private void DisplaySensors()
    {
        ClearDisplay();
        List<SensorBridge.Node> nodes = SensorBridge.Instance.nodes;
        if (nodes == null)
            return;


        foreach (var n in nodes)
        {
            AddText(n.Name, n.Name, panel.transform);
            foreach (var s in n.sensors)
            {
                AddText(s.SensorID.ToString(), s.Name + "\n" + s.SensorID ,  panel.transform);
            }

        }
    }

    private void AddText(string name, string value, Transform parent)
    {
        var o = new GameObject(name);
        var text = o.AddComponent<Text>();
        text.text = value;
        text.font = ArialFont;
        text.color = new Color(0, 0, 0);
        o.transform.SetParent(panel.transform);
        o.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 50);
    }
}
