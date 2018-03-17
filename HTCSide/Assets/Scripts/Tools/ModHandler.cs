using UnityEngine;

public class ModHandler : MonoBehaviour
{
    private Mod mod;
    private InputManager inputManager;
    private DragFurniture dragFurniture;
    private GameObject furnitureMenu;
    private GameObject minimapUI;

    private GameObject removeModTuto;
    private GameObject editionModTuto;
    private GameObject teleportationModTuto;

    private void Start()
    {
        mod = Mod.EDITION;
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
        dragFurniture = GameObject.Find("EditionHandler").GetComponent<DragFurniture>();
        furnitureMenu = GameObject.Find("FurnitureMenu");

        removeModTuto = GameObject.Find("RemoveModTuto");
        editionModTuto = GameObject.Find("EditionModTuto");
        teleportationModTuto = GameObject.Find("TeleportationModTuto");

        removeModTuto.SetActive(false);
        teleportationModTuto.SetActive(false);

        minimapUI = GameObject.Find("MinimapUI");
        minimapUI.SetActive(false);
    }

    private void Update()
    {
        if (dragFurniture.IsClicked() || dragFurniture.IsOnDrag()) return;

        if (inputManager.GetTrackpadHandler().IsMenuTrackpadClicked())
        {
            Vector2 menuTrackPadPos = inputManager.GetTrackpadHandler().GetMenuTrackpadPos();
            if (menuTrackPadPos.x < 0 && menuTrackPadPos.y < 0)
            {
                mod = Mod.REMOVE;
                furnitureMenu.SetActive(false);
                minimapUI.SetActive(false);

                removeModTuto.SetActive(true);
                editionModTuto.SetActive(false);
                teleportationModTuto.SetActive(false);
            }
            else if (menuTrackPadPos.x > 0 && menuTrackPadPos.y < 0)
            {
                mod = Mod.UTILITIES;
                furnitureMenu.SetActive(false);
                minimapUI.SetActive(true);

                removeModTuto.SetActive(false);
                editionModTuto.SetActive(false);
                teleportationModTuto.SetActive(true);
            }
            else
            {
                mod = Mod.EDITION;
                furnitureMenu.SetActive(true);
                minimapUI.SetActive(false);

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
