using UnityEngine;
using UnityEngine.Networking;

public class ClientNetworkManager : MonoBehaviour {

    private static readonly int NETWORK_PORT = 4875;
    private static readonly string NETWORK_IP_ADDRESS = "127.0.0.1";

    private NetworkClient client;
    private ClientStatus status;
    private int roomQuantityToLoad;

    private Transform player;

    private void Start()
    {
        status = ClientStatus.DISCONNECTED;
        player = GameObject.Find("Player").transform;

        SetupClient();
    }
    private void SetupClient()
    {
        client = new NetworkClient();
        client.RegisterHandler(MsgType.Connect, OnConnected);
        client.RegisterHandler(MsgType.Disconnect, OnConnectionError);

        client.RegisterHandler(VirtualHausMessageType.APPARTMENT_LOADING, LoadAppartment);
        client.RegisterHandler(VirtualHausMessageType.FURNITURE_LOADING, LoadFurnitures);

        client.RegisterHandler(VirtualHausMessageType.USER_POSITION, UpdateUserPosition);
        client.RegisterHandler(VirtualHausMessageType.NEW_FURNITURE_POSITION, UpdateFurniturePosition);

        client.Connect(NETWORK_IP_ADDRESS, NETWORK_PORT);
        status = ClientStatus.CONNECTING;
    }

    /*****************************/
    /* Public Messages Interface */
    /*****************************/

    /* Setup client */

    public void OnConnected(NetworkMessage netMsg)
    {
        client.Send(VirtualHausMessageType.CONNECTED, new EmptyMessage());
        status = ClientStatus.CONNECTED;
    }
    public void OnConnectionError(NetworkMessage netMsg)
    {
        client.Connect(NETWORK_IP_ADDRESS, NETWORK_PORT);
    }

    public void LoadAppartment(NetworkMessage netMsg)
    {
        AppartmentLoadingMessage msg = netMsg.ReadMessage<AppartmentLoadingMessage>();

        GameObject appartmentPrefab = Resources.Load<GameObject>("Appartments/" + msg.modelName + "/" + msg.modelName);
        roomQuantityToLoad = msg.roomQuantity;

        GameObject appartment = Instantiate(appartmentPrefab, msg.appartmentPosition, Quaternion.identity);
        appartment.transform.localScale = msg.appartmentScale;

        status = ClientStatus.APPARTMENT_LOADED;
        client.Send(VirtualHausMessageType.APPARTMENT_LOADED, new EmptyMessage());
    }

    public void LoadFurnitures(NetworkMessage netMsg)
    {
        FurnitureLoadingMessage msg = netMsg.ReadMessage<FurnitureLoadingMessage>();
        PlaceFurnitures(JsonUtility.FromJson<NewFurnituresInformations>(msg.jsonFurnitures));

        if (--roomQuantityToLoad == 0) // Receive furnitures room by room to prevent oversized messages.
        {
            client.Send(VirtualHausMessageType.USER_READY, new EmptyMessage());
            status = ClientStatus.READY;
        }
    }
    private void PlaceFurnitures(NewFurnituresInformations furnitures)
    {
        foreach (NewFurnitureInformations furniture in furnitures.list)
        {
            GameObject furniturePrefab = Resources.Load<GameObject>("Furnitures/Prefab/" + furniture.prefabName);

            GameObject instance = Instantiate(furniturePrefab, furniture.furniturePosition, furniture.furnitureRotation);
            instance.name = furniture.furnitureName;
        }
    }

    /* Apply updates made by server */

    public void UpdateUserPosition(NetworkMessage netMsg)
    {
        UserPositionMessage msg = netMsg.ReadMessage<UserPositionMessage>();

        player.position = msg.userPosition;
        player.rotation = msg.userRotation;
    }
    public void UpdateFurniturePosition(NetworkMessage netMsg)
    {
        NewFurniturePositionMessage msg = netMsg.ReadMessage<NewFurniturePositionMessage>();

        Transform toUpdate = GameObject.Find(msg.furnitureName).transform;
        toUpdate.position = msg.furniturePosition;
        toUpdate.rotation = msg.furnitureRotation;
    }

    /* Send Update to server */
    public void SendFurniturePosUpdate(GameObject furniture)
    {
        NewFurniturePositionMessage furniturePositionMessage = new NewFurniturePositionMessage()
        {
            furnitureName = furniture.transform.name,
            furniturePosition = furniture.transform.position,
            furnitureRotation = furniture.transform.rotation
        };

        client.Send(VirtualHausMessageType.NEW_FURNITURE_POSITION, furniturePositionMessage);
    }
}
