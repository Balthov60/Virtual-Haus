using UnityEngine;

public class MovableUIHandler : MonoBehaviour {

    private RayCast rayCast;
    private InputManager inputManager;

    private DragFurniture dragFurniture;

    private void Start()
    {
        rayCast = GameObject.Find("PointerController").GetComponent<RayCast>();
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();

        dragFurniture = GameObject.Find("EditionHandler").GetComponent<DragFurniture>();
    }

    private void Update()
    {
        if (dragFurniture.IsClicked())
            UpdateUIPosition();

        if (inputManager.UserClick())
        {
            if (HitMoveButton())
            {
                inputManager.CanClick = false;
                dragFurniture.MakeSelectedObjectMovable();
            }
            else if (HitRemoveButton())
            {
                inputManager.CanClick = false;
                dragFurniture.RemoveSelectedObject();
            }
        }
    }

    /// <summary>
    /// Update ui position in order to make it face the player
    /// and force it to be in front of the furniture.
    /// </summary>
    private void UpdateUIPosition()
    {
        Vector3 newpos = dragFurniture.GetFurnitureSelected().transform.position;
        Vector3 difference = (rayCast.source.transform.position - dragFurniture.GetFurnitureSelected().transform.position).normalized;
        Vector3 furnitureSize = dragFurniture.GetFurnitureSelected().GetComponent<Renderer>().bounds.size;

        if (furnitureSize.x > furnitureSize.z)
        {
            newpos = newpos + difference * (furnitureSize.x * 0.65f);
        }
        else
        {
            newpos = newpos + difference * (furnitureSize.z * 0.65f);
        }
        newpos.y = 1f;


        GetComponent<RectTransform>().anchoredPosition3D = newpos;

        Vector3 lookAt = rayCast.source.transform.position;
        lookAt.y = 1f;
        GetComponent<RectTransform>().LookAt(lookAt);
    }

    /* Raycast Test */

    private bool HitMoveButton()
    {
        return (rayCast.GetHit().transform.name == "MoveButton");
    }
    private bool HitRemoveButton()
    {
        return (rayCast.GetHit().transform.name == "RemoveButton");
    }
    public bool HitMovableUI()
    {
        return HitMoveButton() || HitRemoveButton();
    }
}
