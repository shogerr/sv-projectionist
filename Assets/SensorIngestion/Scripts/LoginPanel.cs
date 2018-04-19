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
        GetComponentInChildren<Button>().onClick.AddListener(() => { LoginHandler(); });
    }

    void Update()
    {
        username = userInput.text;
        password = passInput.text;

        if (sensorBridge.LoggedIn)
        {
            GetComponentsInChildren<Text>()[1].text = "Logged In";
            var b = GetComponentInChildren<Button>();
            b.GetComponentInChildren<Text>().text = "Logout";
        }
        else
        {
            GetComponentsInChildren<Text>()[1].text = "Not Logged In";
            var b = GetComponentInChildren<Button>();
            b.GetComponentInChildren<Text>().text = "Login";
        }
    }

    public string Password()
    {
        return password;
    }
    public string Username()
    {
        return username;
    }

    private void LoginHandler()
    {
        if (sensorBridge.LoggedIn)
            sensorBridge.Logout();
        else
            sensorBridge.LoginUser(username, password);
    }

    private void ClearInputFields()
    {
        var inputFields = GetComponentsInChildren<InputField>();
        foreach (var i in inputFields)
            i.GetComponent<Text>().text = "";
    }
}
