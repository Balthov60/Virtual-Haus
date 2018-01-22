using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SavingUIHandler : MonoBehaviour {

    private readonly Vector3 SAVING_HAUS_SPAWN = new Vector3(-0.5f, 3.1f, -26.38f);
    private static readonly string PRESSED_BUTTON_NAME = "SavingMod";

    //Attributes 
    private RayCast rayCast;
    private InputManager inputManager;
    private GameObject player;
    private Vector3 playerSavingPosition;

    private bool isTeleportableOnSavingUI = true;
    SavingManager savingManager;



    // Use this for initialization
    void Start () {
        rayCast = GameObject.Find("PointerController").GetComponent<RayCast>();
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
        player = GameObject.Find("Player");
        savingManager = new SavingManager();
    }
	
	// Update is called once per frame
	void Update () {


        if (IsModChangeButtonPressed() && isTeleportableOnSavingUI)
        {
            playerSavingPosition = player.transform.position;
            print("Save Position :" + playerSavingPosition);
            player.transform.position = SAVING_HAUS_SPAWN;
            isTeleportableOnSavingUI = false;
        }

        if (inputManager.IsTriggerClicked() && rayCast.Hit())
        {

            if (rayCast.GetHit().transform.name == "Back")
            {
                print(player.transform.position);
                player.transform.position = playerSavingPosition;
                isTeleportableOnSavingUI = true;
            }
            else if (rayCast.GetHit().transform.name == "Save")
            {
                if (savingManager.GetCurrentConnexionID() == null)
                {
                    print("No Save before");
                    SaveAs();
                    player.transform.position = playerSavingPosition;
                    isTeleportableOnSavingUI = true;
                }
                else
                {
                    print("Save before");
                    Save();
                    player.transform.position = playerSavingPosition;
                    isTeleportableOnSavingUI = true;
                }
                
            }
            else if (rayCast.GetHit().transform.name == "Save_as")
            {
                SaveAs();
                player.transform.position = playerSavingPosition;
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
    }


    /// <summary>
    /// Function that allow you to Save the new Content on the Emplacement you want to. 
    /// </summary>
    public void SaveAs()
    {
        savingManager.SaveGameObjects();
    }

}
