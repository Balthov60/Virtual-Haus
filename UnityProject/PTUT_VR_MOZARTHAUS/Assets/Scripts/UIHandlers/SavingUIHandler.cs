using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SavingUIHandler : MonoBehaviour {

    private readonly Vector3 SAVING_HAUS_SPAWN = new Vector3(-0.5f, 3.1f, -26.38f);
    private readonly Vector3 SAVING_SELECTOR = new Vector3(-0.7f, 2.0f, -25.38f);


    private static readonly string PRESSED_BUTTON_NAME = "SavingMod";

    //Attributes 
    private RayCast rayCast;
    private InputManager inputManager;
    private GameObject player;
    private Vector3 playerSavingPosition;
    private GameObject saveAsSelector;
    private SavingManager savingManager;

    private bool isTeleportableOnSavingUI = true;
    



    // Use this for initialization
    void Start () {
        rayCast = GameObject.Find("PointerController").GetComponent<RayCast>();
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
        player = GameObject.Find("Player");
        savingManager = GameObject.Find("SavingManager").GetComponent<SavingManager>();
        saveAsSelector = GameObject.Find("SelectorSaveAsMenu");
    }
	
	// Update is called once per frame
	void Update () {


        if (IsModChangeButtonPressed() && isTeleportableOnSavingUI)
        {
            playerSavingPosition = player.transform.position;
            SetPlayerSavingPosition(playerSavingPosition);

            player.transform.position = SAVING_HAUS_SPAWN;
            isTeleportableOnSavingUI = false;
        }

        if (inputManager.IsTriggerClicked() && rayCast.Hit())
        {

            if (rayCast.GetHit().transform.name == "Back")
            {
               
                player.transform.position = playerSavingPosition;
                isTeleportableOnSavingUI = true;
            }
            else if (rayCast.GetHit().transform.name == "Save")
            {
                if (savingManager.GetCurrentConnexionID() == null)
                {                  
                    OpenSaveAsPanel();
                    isTeleportableOnSavingUI = true;
                }
                else
                {                    
                    Save();
                    player.transform.position = playerSavingPosition;
                    isTeleportableOnSavingUI = true;
                }
                
            }
            else if (rayCast.GetHit().transform.name == "Save_as")
            {
                OpenSaveAsPanel();
                isTeleportableOnSavingUI = true;
            }
        }


    }

    public bool IsModChangeButtonPressed()
    {
        return Input.GetButtonDown(PRESSED_BUTTON_NAME);
    }

    /// <summary>
    /// Function that save the new content into the current file.
    /// </summary>
    public void Save()
    {
        savingManager.UpdateCurrentSave();
        player.transform.position = playerSavingPosition;
        isTeleportableOnSavingUI = true;
    }


    /// <summary>
    /// Function which opens the SaveAs Panel
    /// </summary>
    public void OpenSaveAsPanel()
    {
        saveAsSelector.transform.position = SAVING_SELECTOR;
    }

    public Vector3 GetPlayerSavingPosition() { return playerSavingPosition; }
    public void SetPlayerSavingPosition(Vector3 playerSavingPosition)
    {
        this.playerSavingPosition = playerSavingPosition;
    }
}
