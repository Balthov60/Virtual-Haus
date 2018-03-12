using UnityEngine;

public class MovableUIHandler : MonoBehaviour {

    private RayCast rayCast;
    private InputManager inputManager;

    private DragFurniture dragFurniture;

    void Start()
    {
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
        rayCast = GameObject.Find("PointerController").GetComponent<RayCast>();

        dragFurniture = GameObject.Find("EditionHandler").GetComponent<DragFurniture>();
    }

    void Update ()
    {
        if (dragFurniture.IsClicked())
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
            newpos.y = 0.5f;


            GetComponent<RectTransform>().anchoredPosition3D = newpos;

            Vector3 lookAt = rayCast.source.transform.position;
            lookAt.y = 1f;
            GetComponent<RectTransform>().LookAt(lookAt);
        }

        if (inputManager.IsTriggerClicked() && rayCast.Hit() && dragFurniture.CanClick())
        {
            if (HitMoveButton())
            {
                dragFurniture.MakeSelectedObjectMovable();
            }
            else if (HitRemoveButton())
            {
                dragFurniture.RemoveSelectedObject();
            }
        }
    }

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
