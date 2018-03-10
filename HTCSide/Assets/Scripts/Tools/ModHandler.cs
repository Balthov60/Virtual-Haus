using UnityEngine;

public class ModHandler : MonoBehaviour
{
    private Mod mod;
    private InputManager inputManager;
    private GameObject furnitureMenu;
    private GameObject roomTpUi;

    private GameObject removeModTuto;
    private GameObject editionModTuto;
    private GameObject teleportationModTuto;

    private void Start()
    {
        mod = Mod.EDITION;
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
        furnitureMenu = GameObject.Find("FurnitureMenu");
        roomTpUi = GameObject.Find("RoomTpUI");

        removeModTuto = GameObject.Find("RemoveModTuto");
        editionModTuto = GameObject.Find("EditionModTuto");
        teleportationModTuto = GameObject.Find("TeleportationModTuto");

        removeModTuto.SetActive(false);
        teleportationModTuto.SetActive(false);
    }

    private void Update()
    {
        if (inputManager.GetTrackpadHandler().IsMenuTrackpadClicked())
        {
            Vector2 menuTrackPadPos = inputManager.GetTrackpadHandler().GetMenuTrackpadPos();
            if (menuTrackPadPos.x < 0 && menuTrackPadPos.y < 0)
            {
                mod = Mod.REMOVE;
                furnitureMenu.SetActive(false);
                roomTpUi.SetActive(false);

                removeModTuto.SetActive(true);
                editionModTuto.SetActive(false);
                teleportationModTuto.SetActive(false);
            }
            else if (menuTrackPadPos.x > 0 && menuTrackPadPos.y < 0)
            {
                mod = Mod.UTILITIES;
                furnitureMenu.SetActive(false);
                roomTpUi.SetActive(true);

                removeModTuto.SetActive(false);
                editionModTuto.SetActive(false);
                teleportationModTuto.SetActive(true);
            }
            else
            {
                mod = Mod.EDITION;
                furnitureMenu.SetActive(true);
                roomTpUi.SetActive(false);

                removeModTuto.SetActive(false);
                editionModTuto.SetActive(true);
                teleportationModTuto.SetActive(false);
            }
        }

        UpdateForNonVR();
    }    
    private void UpdateForNonVR()
    {
        if (Input.GetKey(KeyCode.I))
        {
            mod = Mod.EDITION;

            removeModTuto.SetActive(false);
            editionModTuto.SetActive(true);
            teleportationModTuto.SetActive(false);
        }
        else if (Input.GetKey(KeyCode.O))
        {
            mod = Mod.UTILITIES;

            removeModTuto.SetActive(false);
            editionModTuto.SetActive(false);
            teleportationModTuto.SetActive(true);
        }
        else if (Input.GetKey(KeyCode.P))
        {
            mod = Mod.REMOVE;

            removeModTuto.SetActive(true);
            editionModTuto.SetActive(false);
            teleportationModTuto.SetActive(false);
        }
    }

    /* Getter */

    public bool IsInUtilitiesMod()
    {
        return mod == Mod.UTILITIES;
    }
    public bool IsInEditionMod()
    {
        return mod == Mod.EDITION;
    }
    public bool IsInRemoveMod()
    {
        return mod == Mod.REMOVE;
    }
}

public enum Mod
{
    UTILITIES,
    EDITION,
    REMOVE
}
