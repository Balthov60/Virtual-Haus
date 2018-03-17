using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class NetworkUIHandler : MonoBehaviour {

    private Text networkStatus;
    private Text clientValue;

    private ServerNetworkManager serverNetworkManager;
    private InputManager inputManager;
    private RayCast rayCast;

    private void Start()
    {
        networkStatus = GameObject.Find("NetworkStatus").GetComponent<Text>();
        clientValue = GameObject.Find("ClientValue").GetComponent<Text>();

        serverNetworkManager = GameObject.Find("NetworkManager").GetComponent<ServerNetworkManager>();
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
        rayCast = GameObject.Find("PointerController").GetComponent<RayCast>();

        GameObject.Find("IPValue").GetComponent<Text>().text = GetLocalIPAddress();
    }
	
	private void Update()
    {
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
