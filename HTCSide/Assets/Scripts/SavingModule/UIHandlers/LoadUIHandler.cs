using System;
using UnityEngine;

public class LoadUIHandler : MonoBehaviour
{
    private static readonly string MESSAGE_SUCCESS = "Votre appartement a bien été chargé !";
    private static readonly string MESSAGE_FAILED = "Impossible de charger cet appartement\nCette sauvegarde n'existe pas...";
    private static readonly string MESSAGE_RESET_CONFIRM = "Etes vous sur de vouloir supprimer tout les meubles ?";
    private static readonly string MESSAGE_RESET = "Tout les meubles de l'appartement ont bien été supprimé.";

    private RayCast rayCast;
    private InputManager inputManager;
    private SavingManager savingManager;

    private GameObject furnitures;
    public ValidationPopUp.Callback resetButtonCallback;

    private IDSelectorUIHandler idSelectorUIHandler;
    private FurnitureUIHandler furnitureUIHandler;

    private void Start()
    {
        rayCast = GameObject.Find("PointerController").GetComponent<RayCast>();
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
        savingManager = GameObject.Find("SavingManager").GetComponent<SavingManager>();

        furnitures = GameObject.Find("Furnitures");
        resetButtonCallback = resetFurnituresPosition;

        idSelectorUIHandler = transform.GetChild(0).Find("IDSelectorUI").GetComponent<IDSelectorUIHandler>();
        furnitureUIHandler = GameObject.Find("FurnitureMenu").GetComponent<FurnitureUIHandler>();
    }

    private void Update()
    {
        if (!inputManager.UserClick()) return;

        if (rayCast.GetHit().transform.name == "Reset")
        {
            inputManager.CanClick = false;
            PopUp.DisplayValidationPopUp(gameObject, MESSAGE_RESET_CONFIRM, resetButtonCallback);
        }
        else if (rayCast.GetHit().transform.name == "Load")
        {
            inputManager.CanClick = false;

            if (savingManager.LoadGameObjects(idSelectorUIHandler.GetID()))
            {
                PopUp.DisplayScheduledPopUp(gameObject, MESSAGE_SUCCESS, 2);
                furnitureUIHandler.UpdateRightUIPart(0);
            }
            else
            {
                PopUp.DisplayScheduledPopUp(gameObject, MESSAGE_FAILED, 3);
            }
        }
    }

    private void resetFurnituresPosition()
    {
        for (int i = 0; i < furnitures.transform.childCount; i++)
        {
            Transform room = furnitures.transform.GetChild(i);
            for (int j = 0; j < room.childCount; j++)
            {
                room.GetChild(j).transform.position = DragFurniture.DEFAULT_FURNITURE_POSITION;
            }
        }
        furnitureUIHandler.UpdateRightUIPart(0);

        PopUp.DisplayScheduledPopUp(gameObject, MESSAGE_RESET, 3);
    }
}
