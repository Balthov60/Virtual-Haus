using UnityEngine;

public class HomeTeleportation : MonoBehaviour {

    private static readonly Vector3 MOZART_HAUS_HOME = new Vector3(17, 0, 85);

    private InputManager inputManager;
    private GameObject player;

    public static bool isInSpawn;
    private Vector3 previousPosition;

    private void Start()
    {
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
        player = GameObject.Find("Player");

        isInSpawn = true;
    }

    private void Update()
    {
        if (!inputManager.IsHomeButtonClicked()) return;

        if (isInSpawn)
        {
            if (previousPosition != Vector3.zero)
            {
                player.transform.position = previousPosition;
                isInSpawn = false;
            }
        }
        else
        {
            previousPosition = player.transform.position;
            player.transform.position = MOZART_HAUS_HOME;
            isInSpawn = true;
        }
	}
}
