using UnityEngine;

public class TeleportationHandler : MonoBehaviour
{
    private RayCast rayCast;
    private InputManager inputManager;
    private ModHandler modHandler;
    private GameObject player;

    void Start()
    {
        rayCast = GameObject.Find("PointerController").GetComponent<RayCast>();
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
        modHandler = GameObject.Find("ModHandler").GetComponent<ModHandler>();
        player = GameObject.Find("Player");
    }

    void Update()
    {
        if (modHandler.IsInUtilitiesMod() && inputManager.UserClick() && rayCast.GetHit().transform.name.Contains("Floor"))
        {
            inputManager.CanClick = false;

            Teleport(rayCast.GetHit());
        }
    }

    public void Teleport(RaycastHit hit)
    {
        Vector3 newPlayerPos = hit.point;
        newPlayerPos.y = player.transform.position.y;

        player.transform.position = newPlayerPos;
    }
}




