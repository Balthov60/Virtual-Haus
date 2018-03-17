using UnityEngine;
using UnityEngine.UI;

public class RemoveFurniture : MonoBehaviour {

    private RayCast rayCast;
    private ModHandler modHandler;
    private InputManager inputManager;

    private GameObject furnitureMenu;
    private ServerNetworkManager networkManager;

    void Start () {
        rayCast = GameObject.Find("PointerController").GetComponent<RayCast>();
        modHandler = GameObject.Find("ModHandler").GetComponent<ModHandler>();
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();

        furnitureMenu = GameObject.Find("FurnitureMenu");
        networkManager = GameObject.Find("NetworkManager").GetComponent<ServerNetworkManager>();
    }

    void Update()
    {
        if (!modHandler.IsInRemoveMod() || !rayCast.HitFurniture()) return;

        if (inputManager.UserClick())
        {
            GameObject gameObject = rayCast.GetHit().transform.gameObject;
            gameObject.transform.position = new Vector3(0, -50, 0);

            networkManager.SendFurniturePosUpdate(gameObject);
            UpdateUI(gameObject);
        }
    }

    private void UpdateUI(GameObject gameObject)
    {
        furnitureMenu.SetActive(true);

        Transform ui = GameObject.Find(gameObject.name + "_ui").transform;
        Color color = ui.GetChild(2).GetComponent<Image>().color;
        color.a = 0f;
        ui.GetChild(2).GetComponent<Image>().color = color;

        furnitureMenu.SetActive(false);
    }
}
