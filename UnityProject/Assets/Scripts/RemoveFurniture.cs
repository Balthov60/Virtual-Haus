﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveFurniture : MonoBehaviour {

    private RayCast rayCast;
    private InputManager inputManager;
    private ModHandler modHandler;

    private bool canClick = true;

    void Start () {
        rayCast = GameObject.Find("PointerController").GetComponent<RayCast>();
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
        modHandler = GameObject.Find("ModHandler").GetComponent<ModHandler>();
    }

    void Update()
    {
        if (modHandler.IsInRemoveMod() && rayCast.HitFurniture())
        {
            if (inputManager.IsTriggerClicked() && canClick)
            {
                if (modHandler.IsInRemoveMod() && rayCast.HitFurniture())
                {
                    canClick = false;
                    rayCast.GetHit().transform.position = new Vector3(0, -50, 0);
                }
            }
        }
        if (!canClick)
        {
            canClick = !inputManager.IsTriggerClicked();
        }
    }
}
