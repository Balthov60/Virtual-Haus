using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingUIHandler : MonoBehaviour {

    private RayCast rayCast;
    private InputManager inputManager;
    private SavingManager savingManager;

    private IDSelectorUIHandler idSelectorUIHandler;

    private bool canClick;

    void Start() {
        rayCast = GameObject.Find("PointerController").GetComponent<RayCast>();
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
        savingManager = GameObject.Find("SavingManager").GetComponent<SavingManager>();
        idSelectorUIHandler = transform.GetChild(0).Find("IDSelectorUI").GetComponent<IDSelectorUIHandler>();

        canClick = true;
    }
	
	void Update() {
        if (inputManager.IsTriggerClicked() && canClick && rayCast.Hit())
        {
            if (rayCast.GetHit().transform.name == "Load")
            {
                savingManager.LoadGameObjects(idSelectorUIHandler.GetCurrentID()); // TODO: Use return to display fail or success message                 
                canClick = false;
            }
        }

        if (!canClick)
        {
            canClick = !inputManager.IsTriggerClicked();
        }
    }
}
