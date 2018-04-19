using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

public class Sensor : MonoBehaviour {

    public SensorBridge sensorBridge;
    private SensorBridge.Sensor sensor;

    public List<SensorBridge.Reading> readings;

    public GameObject WorldObject;

    public System.DateTime StartDate;
    public System.DateTime EndDate;

    public int SensorID;

    private void Start()
    {
    }

    void Update()
    {
        ModifyWorldObjet();
    }

    public void ModifyWorldObjet()
    {
        if (WorldObject == null || readings == null || readings.Count == 0) return;
        
        var value = readings[0].EngUnit;
        Debug.Log(Mathf.Clamp(value, 0, 1.0F));
        WorldObject.GetComponent<Renderer>().material.color = new Color(Mathf.Clamp(value, 0, 1.0F), 0, 0);
    }

    public void UpdateSensorReadings()
    {
        sensor = sensorBridge.FindSensor(SensorID);

        if (sensor == null)
            return;

        var startDate = StartDate.ToString("yyyy-MM-dd");
        var endDate = EndDate.ToString("yyyy-MM-dd");

        // Updating Sensor
        StartCoroutine(sensorBridge.api.Request(sensorBridge.api.ListSensorData(SensorID, startDate, endDate), s =>
        {
            XmlSerializer d = new XmlSerializer(typeof(SensorBridge.SensorReadingList));
            using (var r = new System.IO.StringReader(s))
            {
                SensorBridge.SensorReadingList l = (SensorBridge.SensorReadingList)d.Deserialize(r);
                readings = l.readings;
            }
        }));
    }

    public double[] ReadingsArray()
    {
        if (readings == null) return null;

        double[] a = new double[readings.Count];
        int i = 0;
        foreach (SensorBridge.Reading r in readings)
        {
            a[i] = r.Raw;
            i++;
        }
        return a;
    }
}
