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
                    PopUp.DisplayBasicPopUp(gameObject, "Votre appartement a bien été sauvegardé", 3);
                }
                else
                {
                    PopUp.DisplayBasicPopUp(gameObject, "Vous n'avez pas de sauvegarde en cours \n Créé en une nouvelle", 4);
                }
                canClick = false;
            }
            else if (rayCast.GetHit().transform.name == "SaveAs")
            {
                IDSelectorUIHandler idSelectorUIHandler = transform.GetChild(0).Find("IDSelectorUI").GetComponent<IDSelectorUIHandler>();
                string saveID = idSelectorUIHandler.GetID();

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
