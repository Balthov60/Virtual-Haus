using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveUIHandler : MonoBehaviour {

    private RayCast rayCast;
    private InputManager inputManager;

    private SavingManager savingManager;

    private bool canClick;

	void Start () {
        rayCast = GameObject.Find("PointerController").GetComponent<RayCast>();
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();

        savingManager = GameObject.Find("SavingManager").GetComponent<SavingManager>();

        canClick = true;
	}

    void Update()
    {
        if (inputManager.IsTriggerClicked() && rayCast.Hit() && canClick)
        {
            if (rayCast.GetHit().transform.name == "Save")
            {
                if (savingManager.UpdateCurrentSave())
                {
                    //TODO: Display Success Message
                }
                else
                {
                    //TODO: Display Failed Message
                }
                canClick = false;
            }
            else if (rayCast.GetHit().transform.name == "SaveAs")
            {
                IDSelectorUIHandler idSelectorUIHandler = transform.GetChild(0).Find("IDSelectorUI").GetComponent<IDSelectorUIHandler>();
                string saveID = idSelectorUIHandler.GetCurrentID();

                // TODO: display override message
                savingManager.SaveGameObjects(saveID);

                canClick = false;
            }
        }
        if (!canClick)
        {
            canClick = !inputManager.IsTriggerClicked();
        }
    }
}
