using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportUI : MonoBehaviour {

    private GameObject player;
    private RayCast rayCast;
		
    private void Start()
    {
        player = GameObject.Find("Player");
        rayCast = GameObject.Find("PointerController").GetComponent<RayCast>();
    }

    public void DisplayMovableUIInFrontOfPlayer(GameObject movableUI)
    {
        movableUI.GetComponent<RectTransform>().anchoredPosition3D = player.transform.position;
        movableUI.GetComponent<RectTransform>().LookAt(rayCast.source.transform);

        Vector3 newpos = player.transform.position + (player.transform.forward - 0.75f * player.transform.right);
        newpos.y = 1.5f;

        movableUI.GetComponent<RectTransform>().anchoredPosition3D = newpos;
        movableUI.GetComponent<RectTransform>().transform.rotation = player.transform.rotation;
    }

    public void DisplayMovableUIAwayFromPlayer(GameObject movableUI)
    {
        movableUI.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, -40, 0);
    }
}
