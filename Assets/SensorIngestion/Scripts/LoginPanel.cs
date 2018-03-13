using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : MonoBehaviour {
    private string username;
    private string password;

    public SensorBridge sensorBridge;

    public InputField userInput;
    public InputField passInput;

    private void Start()
    {
        GetComponentInChildren<Button>().onClick.AddListener(() => { sensorBridge.LoginUser(username, password); });
    }

    void Update()
    {
        username = userInput.text;
        password = passInput.text;
    }

    public string Password()
    {
        return password;
    }
    public string Username()
    {
        return username;
    }
}
