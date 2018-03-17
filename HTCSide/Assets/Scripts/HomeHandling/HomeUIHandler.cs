using UnityEngine;

public class HomeUIHandler : MonoBehaviour
{
    private static readonly string NOT_AVAILABLE_MESSAGE = "Fonctionnalitée non disponnible...";
    private static Vector2 MOZART_HAUS_MENU_BUTTON_POSITION;
    private static Vector2 APPARTEMENTS_MENU_BUTTON_POSITION;
    private static Vector2 PARAMETERS_MENU_BUTTON_POSITION;

    [SerializeField] public Vector3 appartmentSpawn;
    public GameObject selector;

    private RayCast rayCast;
    private InputManager inputManager;

    private void Start()
    {
        rayCast = GameObject.Find("PointerController").GetComponent<RayCast>();
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();

        MOZART_HAUS_MENU_BUTTON_POSITION = 
            GameObject.Find("MozartHausMenuButton").GetComponent<RectTransform>().anchoredPosition;
        APPARTEMENTS_MENU_BUTTON_POSITION = 
            GameObject.Find("AppartementsMenuButton").GetComponent<RectTransform>().anchoredPosition;
        PARAMETERS_MENU_BUTTON_POSITION = 
            GameObject.Find("ParametersMenuButton").GetComponent<RectTransform>().anchoredPosition;
    }

    private void Update()
    {
        selector.SetActive(false);

        if (!rayCast.Hit()) return;
        
        if (rayCast.GetHit().transform.name == "MozartHausMenuButton")
        {
            MoveSelectorTo(MOZART_HAUS_MENU_BUTTON_POSITION);

            if (inputManager.UserClick())
            {
                inputManager.CanClick = false;
                HomeTeleportation.isInSpawn = false;
                TeleportToMozartHaus(); 
            }
        }
        else if (rayCast.GetHit().transform.name == "AppartementsMenuButton")
        {
            MoveSelectorTo(APPARTEMENTS_MENU_BUTTON_POSITION);

            if (inputManager.UserClick())
            {
                inputManager.CanClick = false;
                PopUp.DisplayScheduledPopUp(gameObject, NOT_AVAILABLE_MESSAGE, 2);
            }
        }
        else if (rayCast.GetHit().transform.name == "ParametersMenuButton")
        {
            MoveSelectorTo(PARAMETERS_MENU_BUTTON_POSITION);

            if (inputManager.UserClick())
            {
                inputManager.CanClick = false;
                PopUp.DisplayScheduledPopUp(gameObject, NOT_AVAILABLE_MESSAGE, 2);
            }
        }
    }

    private void MoveSelectorTo(Vector2 newPosition)
    {
        selector.SetActive(true);
        selector.GetComponent<RectTransform>().anchoredPosition = newPosition;
    }

    /// <summary>
    /// Teleport Player To MozartHaus (ignore y position)
    /// </summary>
    private void TeleportToMozartHaus()
    {
        GameObject player = GameObject.Find("Player");

        Vector3 newPlayerPosition = appartmentSpawn;
        newPlayerPosition.y = player.transform.position.y;

        player.transform.position = newPlayerPosition;
    }
}
