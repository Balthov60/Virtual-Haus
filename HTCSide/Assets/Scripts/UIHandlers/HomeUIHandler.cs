using UnityEngine;

public class HomeUIHandler : MonoBehaviour
{
    private readonly Vector3 MOZART_HAUS_SPAWN = new Vector3(-2, 0, -3.5f);

    private static Vector2 MOZART_HAUS_MENU_BUTTON_POSITION;
    private static Vector2 APPARTEMENTS_MENU_BUTTON_POSITION;
    private static Vector2 PARAMETERS_MENU_BUTTON_POSITION;

    public GameObject selector;

    private RayCast rayCast;
    private InputManager inputManager;
    private GameObject player;

    public void Start()
    {
        rayCast = GameObject.Find("PointerController").GetComponent<RayCast>();
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
        player = GameObject.Find("Player");

        MOZART_HAUS_MENU_BUTTON_POSITION = 
            GameObject.Find("MozartHausMenuButton").GetComponent<RectTransform>().anchoredPosition;
        APPARTEMENTS_MENU_BUTTON_POSITION = 
            GameObject.Find("AppartementsMenuButton").GetComponent<RectTransform>().anchoredPosition;
        PARAMETERS_MENU_BUTTON_POSITION = 
            GameObject.Find("ParametersMenuButton").GetComponent<RectTransform>().anchoredPosition;
    }

    public void Update()
    {
        selector.SetActive(false);
        
        if (rayCast.Hit())
        {
            if (rayCast.GetHit().transform.name == "MozartHausMenuButton")
            {
                selector.SetActive(true);
                selector.GetComponent<RectTransform>().anchoredPosition = MOZART_HAUS_MENU_BUTTON_POSITION;
                
                if (inputManager.UserClick())
                {
                    TeleportToMozartHaus();
                }
            }
            else if (rayCast.GetHit().transform.name == "AppartementsMenuButton")
            {
                selector.SetActive(true);
                selector.GetComponent<RectTransform>().anchoredPosition = APPARTEMENTS_MENU_BUTTON_POSITION;

                //TODO implement function when IsTriggerClicked() == true
            }
            else if (rayCast.GetHit().transform.name == "ParametersMenuButton")
            {
                selector.SetActive(true);
                selector.GetComponent<RectTransform>().anchoredPosition = PARAMETERS_MENU_BUTTON_POSITION;
                
                //TODO: implement function when IsTriggerClicked() == true
            }
        }
    }

    /// <summary>
    /// Teleport Player To MozartHaus (ignore y position)
    /// </summary>
    public void TeleportToMozartHaus()
    {
        Vector3 newPlayerPosition = MOZART_HAUS_SPAWN;
        newPlayerPosition.y = player.transform.position.y;

        player.transform.position = newPlayerPosition;
    }
}
