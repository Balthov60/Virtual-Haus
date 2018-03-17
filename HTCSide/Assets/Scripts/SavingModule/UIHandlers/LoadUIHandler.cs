using UnityEngine;

public class LoadUIHandler : MonoBehaviour
{
    private static readonly string MESSAGE_SUCCESS = "Votre appartement a bien été chargé !";
    private static readonly string MESSAGE_FAILED = "Impossible de charger cet appartement\nCette sauvegarde n'existe pas...";

    private RayCast rayCast;
    private InputManager inputManager;
    private SavingManager savingManager;

    private IDSelectorUIHandler idSelectorUIHandler;

    private void Start()
    {
        rayCast = GameObject.Find("PointerController").GetComponent<RayCast>();
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
        savingManager = GameObject.Find("SavingManager").GetComponent<SavingManager>();
        idSelectorUIHandler = transform.GetChild(0).Find("IDSelectorUI").GetComponent<IDSelectorUIHandler>();
    }
	
	private void Update()
    {
        if (!inputManager.UserClick() || !(rayCast.GetHit().transform.name == "Load")) return;

        if (savingManager.LoadGameObjects(idSelectorUIHandler.GetID()))
        {
            PopUp.DisplayScheduledPopUp(gameObject, MESSAGE_SUCCESS, 2);
        }
        else
        {
            PopUp.DisplayScheduledPopUp(gameObject, MESSAGE_FAILED, 3);
        }
    }
}
