using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class ServerNetworkManager : MonoBehaviour
{
    private static readonly int NETWORK_PORT = 4875;

    private Transform currentAppartment;
    private Transform player;

    private Dictionary<int, bool> connectionsReady;

    void Start()
    {
        currentAppartment = GameObject.Find("Appartment").transform;
        player = GameObject.Find("Player").transform;

        connectionsReady = new Dictionary<int, bool>();
    }
    public void SetupServer()
    {
        connectionsReady = new Dictionary<int, bool>();

        NetworkServer.RegisterHandler(VirtualHausMessageType.CONNECTED, OnClientConnect);
        NetworkServer.RegisterHandler(VirtualHausMessageType.APPARTMENT_LOADED, OnClientAppartmentLoaded);
        NetworkServer.RegisterHandler(VirtualHausMessageType.USER_READY, OnClientReady);

        NetworkServer.RegisterHandler(VirtualHausMessageType.NEW_FURNITURE_POSITION, UpdateFurniturePosition);
        NetworkServer.RegisterHandler(MsgType.Disconnect, OnClientDisconnect);

        NetworkServer.Listen(NETWORK_PORT);
    }
    public void StopServer()
    {
        foreach (KeyValuePair<int, bool> connection in connectionsReady)
        {
            NetworkServer.SendToClient(connection.Key, MsgType.Disconnect, new EmptyMessage());
        }

        NetworkServer.Reset();
    }

    void Update()
    {
        SendPlayerPosUpdate();
    }

    /* For Handler */

    public int GetClientQuantity()
    {
        return connectionsReady.Count;
    }


    /*****************************/
    /* Public Messages Interface */
    /*****************************/

    /* Setup client */

    /// <summary>
    /// When client connect to server :
    /// Register him & send the current appartment informations. 
    /// </summary>
    /// <param name="netMsg"></param>
    public void OnClientConnect(NetworkMessage netMsg)
    {
        connectionsReady.Add(netMsg.conn.connectionId, false);

        AppartmentLoadingMessage appartmentLoadingMessage = new AppartmentLoadingMessage
        {
            modelName = PrefabUtility.GetPrefabParent(currentAppartment).name,
            roomQuantity = GameObject.Find("Furnitures").transform.childCount,

            appartmentScale = currentAppartment.localScale,
            appartmentPosition = currentAppartment.position,
        };

        NetworkServer.SendToClient(netMsg.conn.connectionId, VirtualHausMessageType.APPARTMENT_LOADING, appartmentLoadingMessage);
    }

    /// <summary>
    /// When client finished appartment loading :
    /// Send him furnitures used for the current appartment.
    /// 
    /// Send this furnitures room by room to prevent oversized messages.
    /// </summary>
    /// <param name="netMsg"></param>
    public void OnClientAppartmentLoaded(NetworkMessage netMsg)
    {
        for (int i = 0; i < GameObject.Find("Furnitures").transform.childCount; i++)
        {
            FurnitureLoadingMessage furnitureLoadingMessage = new FurnitureLoadingMessage
            {
                jsonFurnitures = JsonUtility.ToJson(new NewFurnituresInformations(i))
            };

            NetworkServer.SendToClient(netMsg.conn.connectionId, VirtualHausMessageType.FURNITURE_LOADING, furnitureLoadingMessage);
        }
    }

    /// <summary>
    /// Client is ready when all furnitures have been loaded.
    /// </summary>
    /// <param name="netMsg"></param>
    public void OnClientReady(NetworkMessage netMsg)
    {
        connectionsReady[netMsg.conn.connectionId] = true;
    }


    /* Send update to client */

    public void SendFurniturePosUpdate(GameObject furniture)
    {
        NewFurniturePositionMessage furniturePositionMessage = new NewFurniturePositionMessage()
        {
            furnitureName = furniture.transform.name,
            furniturePosition = furniture.transform.localPosition,
            furnitureRotation = furniture.transform.rotation
        };

        SendMessageToAllClientReady(VirtualHausMessageType.NEW_FURNITURE_POSITION, furniturePositionMessage);
    }
    private void SendPlayerPosUpdate()
    {
        UserPositionMessage userPositionMessage = new UserPositionMessage()
        {
            userPosition = player.position,
            userRotation = player.rotation
        };

        SendMessageToAllClientReady(VirtualHausMessageType.USER_POSITION, userPositionMessage);
    }

    private void SendMessageToAllClientReady(short msgType, MessageBase message)
    {
        // TODO: Fix this error (resuming game after a script update remove this connectionsReady GO)
        if (connectionsReady == null) return;

        foreach (KeyValuePair<int, bool> connection in connectionsReady)
        {
            if (connection.Value == true)
            {
                NetworkServer.SendToClient(connection.Key, msgType, message);
            }
        }
    }

    
    /* Apply updates made my client */

    public void UpdateFurniturePosition(NetworkMessage netMsg)
    {
        NewFurniturePositionMessage msg = netMsg.ReadMessage<NewFurniturePositionMessage>();

        Transform toUpdate = GameObject.Find(msg.furnitureName).transform;
        toUpdate.position = msg.furniturePosition;
        toUpdate.rotation = msg.furnitureRotation;
    }


    /* End client connection */

    public void OnClientDisconnect(NetworkMessage netMsg)
    {
        connectionsReady.Remove(netMsg.conn.connectionId);
    }
}
