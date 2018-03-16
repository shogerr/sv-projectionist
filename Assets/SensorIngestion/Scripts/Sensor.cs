using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

public class Sensor : MonoBehaviour {

    public SensorBridge sensorBridge;
    public SensorBridge.Sensor sensor;
    public SensorBridge.Reading[] readings;

    public System.DateTime startDate;
    public System.DateTime EndDate;

    public int sensorID;

    public bool willUpdate = false;

    private void Start()
    {
    }

    void Update()
    {
        /*
        if (sensorBridge.SensorsComplete() && readings != null)
            Debug.Log(readings.Length);
            */

        if (sensor != null && willUpdate == true)
        {
            UpdateSensorReadings();
        }
        if (readings != null)
        {
            foreach (SensorBridge.Reading r in readings)
            {
                Debug.Log(r);
            }
        }
    }

    public void UpdateSensorReadings()
    {
        // Updating Sensor
        StartCoroutine(sensorBridge.api.Request(sensorBridge.api.ListSensorData(sensorID, '*', '*'), s =>
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
