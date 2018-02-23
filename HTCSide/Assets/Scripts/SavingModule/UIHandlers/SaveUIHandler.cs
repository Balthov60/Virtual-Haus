using UnityEngine;

public class SaveUIHandler : MonoBehaviour {

    private static readonly string MESSAGE_SUCCESS = "Votre appartement a bien été sauvegardé !";
    private static readonly string MESSAGE_NO_CURRENT_SAVE = "Vous n'avez pas de sauvegarde en cours \n Créé en une nouvelle...";
    private static readonly string MESSAGE_OVERRIDE = "Il existe déjà une sauvegarde sous cet ID\nVoulez vous la remplacer ?";

    private RayCast rayCast;
    private InputManager inputManager;

    private SavingManager savingManager;
    private IDSelectorUIHandler idSelectorUIHandler;

    public ValidationPopUp.Callback confirmationPopUpCallback;

    void Start () {
        rayCast = GameObject.Find("PointerController").GetComponent<RayCast>();
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();

        savingManager = GameObject.Find("SavingManager").GetComponent<SavingManager>();
        idSelectorUIHandler = transform.GetChild(0).Find("IDSelectorUI").GetComponent<IDSelectorUIHandler>();

        confirmationPopUpCallback = SaveOnSelectedID;
        idSelectorUIHandler.SetID(SavingUtils.GenerateFreeSaveID());
    }

    void Update()
    {
        if (inputManager.UserClick())
        {
            if (rayCast.GetHit().transform.name == "Save")
            {
                inputManager.CanClick = false;
                if (savingManager.UpdateCurrentSave())
                {
                    PopUp.DisplayScheduledPopUp(gameObject, MESSAGE_SUCCESS, 2);
                }
                else
                {
                    PopUp.DisplayScheduledPopUp(gameObject, MESSAGE_NO_CURRENT_SAVE, 4);
                }
            }
            else if (rayCast.GetHit().transform.name == "SaveAs")
            {
                inputManager.CanClick = false;
                string saveID = idSelectorUIHandler.GetID();

                if (SavingUtils.IsIdUsed(saveID))
                {
                    PopUp.DisplayValidationPopUp(gameObject, MESSAGE_OVERRIDE, confirmationPopUpCallback);
                }
                else
                {
                    SaveOnSelectedID();
                }
            }
        }
    }

    public void SaveOnSelectedID()
    {
        savingManager.SaveGameObjects(idSelectorUIHandler.GetID());

        PopUp.DisplayScheduledPopUp(gameObject, MESSAGE_SUCCESS, 2);
    }
}
