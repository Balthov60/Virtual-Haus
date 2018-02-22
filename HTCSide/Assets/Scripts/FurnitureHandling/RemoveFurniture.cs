using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveFurniture : MonoBehaviour {

    private RayCast rayCast;
    private InputManager inputManager;
    private ModHandler modHandler;

    private ServerNetworkManager networkManager;

    private bool canClick = true;

    void Start () {
        rayCast = GameObject.Find("PointerController").GetComponent<RayCast>();
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
        modHandler = GameObject.Find("ModHandler").GetComponent<ModHandler>();

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
                    canClick = false;
                    rayCast.GetHit().transform.position = new Vector3(0, -50, 0);
                    networkManager.SendFurniturePosUpdate(rayCast.GetHit().transform.gameObject);
                }
            }
        }
        if (!canClick)
        {
            canClick = !inputManager.IsTriggerClicked();
        }
    }
}
