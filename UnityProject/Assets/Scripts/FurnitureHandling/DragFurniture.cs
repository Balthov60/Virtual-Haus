using UnityEngine;

public class DragFurniture : MonoBehaviour {

    private RayCast rayCast;
    private InputManager inputManager;
    private ModHandler modHandler;

    public GameObject movableUI;
    private MovableUIHandler movableUIHandler;

    private GameObject furnitureSelected;
    private GameObject pointerController;

    private ServerNetworkManager networkManager;

    private bool isOnDrag = false;
    private bool isClicked = false;
    private bool canClick = true;


    void Start()
    {
        rayCast = GameObject.Find("PointerController").GetComponent<RayCast>();
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
        modHandler = GameObject.Find("ModHandler").GetComponent<ModHandler>();
        movableUIHandler = movableUI.GetComponent<MovableUIHandler>();

        networkManager = GameObject.Find("NetworkManager").GetComponent<ServerNetworkManager>();
        pointerController = GameObject.Find("PointerController");

        isOnDrag = isClicked = false;
        canClick = true;
    }

    void Update()
    {
        if (modHandler.IsInEditionMod() && rayCast.Hit())
        {
            if (inputManager.IsTriggerClicked() && canClick)
            {
                canClick = false;

                if (isOnDrag) // Place Game Object
                {
                    furnitureSelected.GetComponent<Collider>().enabled = true;
                    furnitureSelected = null;

                    isOnDrag = false;
                }
                else if (rayCast.HitFurniture() && !isClicked && !isOnDrag) // Select Game Object
                {
                    furnitureSelected = GameObject.Find(rayCast.GetHit().transform.name);
                    isClicked = true;
                }
                else if (isClicked && !isOnDrag && !movableUIHandler.HitMovableUI()) // UnSelect Game Object
                {
                    DestroyMovableUI();

                    isClicked = false;
                }                              
            }
            if (isOnDrag) // Move Game Object
            {
                UpdateFurniturePosition(rayCast.GetHit());
                networkManager.SendFurniturePosUpdate(furnitureSelected);
            }

            if (!canClick)
            {
                canClick = !inputManager.IsTriggerClicked();
            }
        }
    }

    private void DestroyMovableUI()
    {
        movableUI.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, -40, 0);
    }

    public void SelectObject(GameObject gameObject)
    {
        furnitureSelected = gameObject;
        furnitureSelected.GetComponent<Collider>().enabled = false;

        isOnDrag = true;
        canClick = false;
    }
    public void RemoveSelectedObject()
    {
        furnitureSelected.transform.position = new Vector3(0, -50, 0);
        networkManager.SendFurniturePosUpdate(furnitureSelected);

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
        canClick = false;

        DestroyMovableUI();
    }

    private void UpdateFurniturePosition(RaycastHit hit)
    {
        HandleWallCollision(hit);
        HandleFurnitureCollision(hit);
    }
    private void HandleWallCollision(RaycastHit hit)
    {
        Vector3 newPos = hit.point;

        if (hit.transform.name == "AppartmentMesh" || hit.transform.name.Contains("Wall"))
        {
            float yAngle = hit.transform.rotation.eulerAngles.y;

            if ((yAngle <= 120 && yAngle >= 60) || (yAngle <= 300 && yAngle >= 240)) // Test Axis
            {
                float xSize = furnitureSelected.GetComponent<Renderer>().bounds.size.x;

                if (hit.point.x > pointerController.transform.position.x) // Test Positif / Negatif
                {
                    newPos.x = newPos.x - xSize / 2;
                }
                else
                {
                    newPos.x = newPos.x + xSize / 2;
                }
            }
            else if (yAngle <= 30 || yAngle >= 330 || (yAngle > 150 && yAngle <= 210)) // Test Axis
            {
                float zSize = furnitureSelected.GetComponent<Renderer>().bounds.size.z;

                if (hit.point.z > pointerController.transform.position.z) // Test Positif / Negatif
                {
                    newPos.z = newPos.z - zSize / 2;
                }
                else
                {
                    newPos.z = newPos.z + zSize / 2;
                }
            }
        }

        newPos.y = furnitureSelected.transform.localScale.y / 2;
        furnitureSelected.transform.position = newPos;
    }
    private void HandleFurnitureCollision(RaycastHit hit)
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
                    Vector3 difference = furnitureSelected.transform.position - collider.transform.position;

                    Vector3 distance = (furnitureSelected.GetComponent<Renderer>().bounds.size / 2);
                    Vector3 maxDistance = (furnitureSelected.GetComponent<Renderer>().bounds.size / 2);
                    distance.Scale(difference.normalized);

                    Vector3 selectedToCollideInsidePoint = furnitureSelected.transform.position + distance;

                  
                    distance = collider.bounds.size / 2;
                    maxDistance += distance;
                    distance.Scale(difference.normalized);

                    Vector3 collideToSelectInsidePoint = collider.transform.position + distance;

                    distance = (selectedToCollideInsidePoint - collideToSelectInsidePoint);
                    distance.Scale(difference.normalized);

                    Vector3 distanceToUpdate = (maxDistance - distance);

                    Vector3 newPos = furnitureSelected.transform.position;

                    Vector3 direction = (rayCast.source.transform.position - furnitureSelected.transform.position);
                    
                    if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                    {
                        if (direction.x > 0)
                        {
                            newPos.x += distanceToUpdate.x;
                        }
                        else
                        {
                            newPos.x -= distanceToUpdate.x;
                        }
                    }
                    else
                    {
                        if (direction.z > 0)
                        {
                            newPos.z += distanceToUpdate.z;
                        }
                        else
                        {
                            newPos.z -= distanceToUpdate.z;
                        }
                    }

                    furnitureSelected.transform.position = newPos;
                }
            }
        }
    }

    public bool IsFurnitureSelected()
    {
        return furnitureSelected != null;
    }
    public GameObject GetFurnitureSelected()
    {
        return furnitureSelected;
    }
    public bool CanClick()
    {
        return canClick;
    }
    public bool IsClicked()
    {
        return isClicked;
    }
}