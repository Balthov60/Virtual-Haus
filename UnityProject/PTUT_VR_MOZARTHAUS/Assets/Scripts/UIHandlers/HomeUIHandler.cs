using UnityEngine;

public class HomeUIHandler : MonoBehaviour
{
    private readonly Vector3 MOZART_HAUS_SPAWN = new Vector3(-2, 0.5f, -3.5f);
    private readonly Vector3 LOAD_PANEL = new Vector3(-0.8f, 3.6f, -14);

    private RayCast rayCast;
    private InputManager inputManager;
    private GameObject player;
    private GameObject loadPanel;


    public void Start()
    {
        rayCast = GameObject.Find("PointerController").GetComponent<RayCast>();
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
        player = GameObject.Find("Player");
        loadPanel = GameObject.Find("SelectorLoadSave");
    }

    public void Update()
    {     
        if (inputManager.IsTriggerClicked() && rayCast.Hit())
        {
            if (rayCast.GetHit().transform.name == "Mozart'Haus")
            {
                loadPanel.transform.position = LOAD_PANEL;
            }
            else if (rayCast.GetHit().transform.name == "Appartments")
            {
                print("Appartment"); // Not Implemented
            }
            else if (rayCast.GetHit().transform.name == "SettingsImport")
            {
                print("SettingsImport"); // Not Implemented
            }
        }
    }
}
