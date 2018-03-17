using UnityEngine;
using UnityEngine.UI;

public class DragFurniture : MonoBehaviour {

    private RayCast rayCast;
    private ModHandler modHandler;
    private InputManager inputManager;

    private MovableUIHandler movableUIHandler;
    private ServerNetworkManager networkManager;

    public GameObject movableUI;
    private GameObject furnitureSelected;

    private bool isOnDrag;
    private bool isClicked;

    private void Start()
    {
        rayCast = GameObject.Find("PointerController").GetComponent<RayCast>();
        modHandler = GameObject.Find("ModHandler").GetComponent<ModHandler>();
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();

        movableUIHandler = movableUI.GetComponent<MovableUIHandler>();
        networkManager = GameObject.Find("NetworkManager").GetComponent<ServerNetworkManager>();

        isOnDrag = isClicked = false;
    }

    private void Update()
    {
        if (!modHandler.IsInEditionMod() || !rayCast.Hit()) return;

        if (inputManager.IsTriggerClicked())
        {
            if (isOnDrag) // Place Game Object
            {
                inputManager.CanClick = false;

                furnitureSelected.GetComponent<Collider>().enabled = true;
                furnitureSelected = null;

                isOnDrag = false;
            }
            else if (rayCast.HitFurniture() && !isClicked && !isOnDrag) // Select Game Object
            {
                inputManager.CanClick = false;

                furnitureSelected = GameObject.Find(rayCast.GetHit().transform.name);
                isClicked = true;
            }
            else if (isClicked && !isOnDrag && !movableUIHandler.HitMovableUI()) // UnSelect Game Object
            {
                inputManager.CanClick = false;

                DestroyMovableUI();

                isClicked = false;
            }                              
        }
        if (isOnDrag) // Move Game Object
        {
            UpdateFurniturePosition(rayCast.GetHit());
            networkManager.SendFurniturePosUpdate(furnitureSelected);
        }
    }

    /* Edit Furniture */

    public void SelectObject(GameObject gameObject)
    {
        furnitureSelected = gameObject;
        furnitureSelected.GetComponent<Collider>().enabled = false;

        isOnDrag = true;
    }
    public void RemoveSelectedObject()
    {
        furnitureSelected.transform.position = new Vector3(0, -50, 0);
        networkManager.SendFurniturePosUpdate(furnitureSelected);

        Transform ui = GameObject.Find(furnitureSelected.name + "_ui").transform;
        Color color = ui.GetChild(2).GetComponent<Image>().color;
        color.a = 0f;
        ui.GetChild(2).GetComponent<Image>().color = color;

        furnitureSelected.GetComponent<Collider>().enabled = true;
        furnitureSelected = null;
        isClicked = false;

        DestroyMovableUI();
    }
    public void MakeSelectedObjectMovable()
    {
        furnitureSelected.GetComponent<Collider>().enabled = false;
        isClicked = false;
        isOnDrag = true;

        DestroyMovableUI();
    }

    private void UpdateFurniturePosition(RaycastHit hit)
    {
        HandleMainCollision(hit);
        HandleAdvancedCollision(hit);
    }

    /* Collision Handling */

    private void HandleMainCollision(RaycastHit hit)
    {
        Vector3 newPos = hit.point;

        Vector3 size = furnitureSelected.GetComponent<Renderer>().bounds.size / 2 + (0.025f * Vector3.one);
        size.Scale(hit.normal);

        newPos += size;
        if (hit.normal != Vector3.up)
            newPos.y = furnitureSelected.transform.localScale.y / 2;

        furnitureSelected.transform.position = newPos;
    }

    private void HandleAdvancedCollision(RaycastHit hit)
    {
        Collider[] colliders = Physics.OverlapBox(furnitureSelected.transform.position,
                                                  furnitureSelected.GetComponent<Renderer>().bounds.size / 2,
                                                  furnitureSelected.transform.rotation);

        foreach(Collider collider in colliders)
        {
            if (collider.transform.parent != null && collider.transform.parent.parent != null)
            {
                if (collider.transform.parent.parent.name == "Furnitures")
                {
                    Vector3 newPos = furnitureSelected.transform.position;

                    Vector3 temp = GetSharedDimension(collider);
                    temp.y = 0;
                    newPos -= temp;

                    furnitureSelected.transform.position = newPos;
                }
            }
        }
    }
    /// <summary>
    /// Return dimmension common between collider and funiture in drag
    /// </summary>
    /// <param name="collider"></param>
    /// <returns></returns>
    private Vector3 GetSharedDimension(Collider collider)
    {
        Vector3 direction = furnitureSelected.transform.position - collider.transform.position;
        direction.y = 0;

        Vector3 furnitureSide = furnitureSelected.GetComponent<Renderer>().bounds.size / 2;
        furnitureSide.Scale(direction.normalized);
        Vector3 furnitureInsidePoint = furnitureSelected.transform.position - furnitureSide;

        Vector3 colliderSide = collider.bounds.size / 2;
        colliderSide.Scale(direction.normalized);
        Vector3 colliderInsidePoint = collider.transform.position + colliderSide;

        Vector3 correction = (Vector3.one * 0.025f);
        correction.Scale(direction);

        return furnitureInsidePoint - colliderInsidePoint - correction;
    }

    /* Public Interface */

    public bool IsClicked()
    {
        return isClicked;
    }
    public void DestroyMovableUI()
    {
        movableUI.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, -40, 0);
    }
    public bool IsFurnitureSelected()
    {
        return furnitureSelected != null;
    }
    public GameObject GetFurnitureSelected()
    {
        return furnitureSelected;
    }
}