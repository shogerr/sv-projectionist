using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

public class Sensor : MonoBehaviour {

    public SensorBridge sensorBridge;
    public SensorBridge.Sensor sensor;
    public List<SensorBridge.Reading> readings;

    public System.DateTime StartDate;
    public System.DateTime EndDate;

    public int sensorID;

    public bool willUpdate = false;

    private void Start()
    {
    }

    void Update()
    {
    }

    public void UpdateSensorReadings()
    {
        sensor = sensorBridge.FindSensor(sensorID);

        Debug.Log(sensor.SensorID);
        if (sensor == null)
            return;

        var startDate = StartDate.ToString("yyyy-MM-dd");
        var endDate = EndDate.ToString("yyyy-MM-dd");

        Debug.Log(startDate);
        Debug.Log(endDate);

        // Updating Sensor
        StartCoroutine(sensorBridge.api.Request(sensorBridge.api.ListSensorData(sensorID, startDate, endDate), s =>
        {
            XmlSerializer d = new XmlSerializer(typeof(SensorBridge.SensorReadingList));
            using (var r = new System.IO.StringReader(s))
            {
                SensorBridge.SensorReadingList l = (SensorBridge.SensorReadingList)d.Deserialize(r);
                readings = l.readings;
            }
        }));
    }
}
