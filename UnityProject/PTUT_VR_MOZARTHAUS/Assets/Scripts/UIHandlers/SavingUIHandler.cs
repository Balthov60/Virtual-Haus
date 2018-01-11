using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SavingUIHandler : MonoBehaviour {

    private readonly Vector3 SAVING_HAUS_SPAWN = new Vector3(-0.5f, 4.591f, -26.38f);
    private static readonly string PRESSED_BUTTON_NAME = "SavingMod";

    //Attributes 
    private RayCast rayCast;
    private InputManager inputManager;
    private GameObject player;
    private Vector3 playerSavingPosition;

    private bool isTeleportableOnSavingUI = true;
    private bool isAbleToSave = false;

    SavingManager savingManager;



    // Use this for initialization
    void Start () {
        rayCast = GameObject.Find("PointerController").GetComponent<RayCast>();
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
        player = GameObject.Find("Player");
    }
	
	// Update is called once per frame
	void Update () {

        print(Input.GetButtonDown(PRESSED_BUTTON_NAME));

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
                print("Hello !");
                player.transform.position = playerSavingPosition;
                isTeleportableOnSavingUI = true;
            }
            else if (rayCast.GetHit().transform.name == "Save")
            {
                if (IsAbleToSave())
                {
                    SaveAs();
                    player.transform.position = playerSavingPosition;
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

    public bool IsAbleToSave()
    {
        String userSaved;
        if ()
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    /// <summary>
    /// Function that save the new content into the current file.
    /// </summary>
    public void Save()
    {

    }


    /// <summary>
    /// Function that allow you to Save the new Content on the Emplacement you want to. 
    /// </summary>
    public void SaveAs()
    {
        savingManager.SaveGameObjects();
    }

}
