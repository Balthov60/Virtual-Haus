using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateFurniture : MonoBehaviour {

    private InputManager inputManager;
    private DragFurniture dragFurniture;

    void Start() {
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
        dragFurniture = GameObject.Find("EditionHandler").GetComponent<DragFurniture>();
    }
	
	void Update() {
        if (dragFurniture.IsFurnitureSelected())
        {
            dragFurniture.GetFurnitureSelected().transform.Rotate(new Vector3(0, inputManager.GetTrackpadHandler().GetPointerTrackpadRotationOffset(), 0));
        }
    }
}
