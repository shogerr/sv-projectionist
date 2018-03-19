using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class UDPSend : MonoBehaviour {
    private static int local_port;
    private string IP;
    public int port;

    IPEndPoint remoteEndPoint;
    UdpClient client;

    public void init()
    {
        IP = "127.0.0.1";
        port = 8051;

        remoteEndPoint = new IPEndPoint(IPAddress.Parse(IP), port);

        // Create our client
        client = new UdpClient();

        // Enable broadcasting messages
        client.EnableBroadcast = true;
    }

    // Use this for initialization
    void Start ()
    {
        init();
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void SendString(string msg)
    {
        try
        {
            byte[] data = Encoding.UTF8.GetBytes(msg);
            client.Send(data, data.Length, remoteEndPoint);
        }
        catch (Exception err)
        {
            print(err.ToString());
        }
    }
}
