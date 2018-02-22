using UnityEngine;

public class TeleportationHandler : MonoBehaviour
{
    private RayCast rayCast;
    private InputManager inputManager;
    private ModHandler modHandler;
    private GameObject player;

    private bool canTeleport = true;

    void Start()
    {
        rayCast = GameObject.Find("PointerController").GetComponent<RayCast>();
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
        modHandler = GameObject.Find("ModHandler").GetComponent<ModHandler>();
        player = GameObject.Find("Player");
    }

    void Update()
    {
        if (inputManager.IsTriggerClicked() && canTeleport && modHandler.IsInUtilitiesMod() && rayCast.Hit() && !rayCast.GetHit().transform.name.Contains("Wall"))
        {
            Teleport(rayCast.GetHit());
            canTeleport = false;
        }
        else if (!canTeleport)
        {
            canTeleport = !inputManager.IsTriggerClicked();
            
        }
    }

    public void Teleport(RaycastHit hit)
    {
        Vector3 newPlayerPos = hit.point;
        newPlayerPos.y = player.transform.position.y;

        player.transform.position = newPlayerPos;
    }
}




