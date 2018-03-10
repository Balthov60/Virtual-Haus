using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class NetworkUIHandler : MonoBehaviour {

    private Text networkStatus;
    private Text clientValue;
    private Text ipValue;

    private InputManager inputManager;
    private RayCast rayCast;
    private ServerNetworkManager serverNetworkManager;

    void Start () {
        networkStatus = GameObject.Find("NetworkStatus").GetComponent<Text>();
        clientValue = GameObject.Find("ClientValue").GetComponent<Text>();
        ipValue = GameObject.Find("IPValue").GetComponent<Text>();

        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
        rayCast = GameObject.Find("PointerController").GetComponent<RayCast>();
        serverNetworkManager = GameObject.Find("NetworkManager").GetComponent<ServerNetworkManager>();

        ipValue.text = GetLocalIPAddress();
    }
	
	void Update () {
        clientValue.text = serverNetworkManager.GetClientQuantity().ToString();

        if (inputManager.UserClick())
        {
            if (rayCast.GetHit().transform.name == "StartNetworkUI" && networkStatus.text != "Network On")
            {
                serverNetworkManager.SetupServer();
                networkStatus.text = "Network On";
                networkStatus.color = Color.green;
            }
            else if (rayCast.GetHit().transform.name == "StopNetworkUI" && networkStatus.text != "Network Off")
            {
                serverNetworkManager.StopServer();
                networkStatus.text = "Network Off";
                networkStatus.color = Color.red;
            }
        }
    }

    public static string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }

        return "No IP found";
    }
}
