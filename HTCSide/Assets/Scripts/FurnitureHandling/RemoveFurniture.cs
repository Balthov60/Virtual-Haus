using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemoveFurniture : MonoBehaviour {

    private RayCast rayCast;
    private InputManager inputManager;
    private ModHandler modHandler;
    private GameObject furnitureMenu;

    private ServerNetworkManager networkManager;

    private bool canClick = true;

    void Start () {
        rayCast = GameObject.Find("PointerController").GetComponent<RayCast>();
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
        modHandler = GameObject.Find("ModHandler").GetComponent<ModHandler>();

        furnitureMenu = GameObject.Find("FurnitureMenu");

        networkManager = GameObject.Find("NetworkManager").GetComponent<ServerNetworkManager>();
    }

    void Update()
    {
        if (modHandler.IsInRemoveMod() && rayCast.HitFurniture())
        {
            if (inputManager.IsTriggerClicked() && canClick)
            {
                if (modHandler.IsInRemoveMod() && rayCast.HitFurniture())
                {
                    GameObject gameObject = rayCast.GetHit().transform.gameObject;
                    canClick = false;
                    gameObject.transform.position = new Vector3(0, -50, 0);
                    networkManager.SendFurniturePosUpdate(gameObject);

                    UpdateUI(gameObject);
                }
            }
        }
        if (!canClick)
        {
            canClick = !inputManager.IsTriggerClicked();
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
