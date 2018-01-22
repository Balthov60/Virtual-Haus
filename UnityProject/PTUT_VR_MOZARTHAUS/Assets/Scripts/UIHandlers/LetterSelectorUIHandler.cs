using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LetterSelectorUIHandler : MonoBehaviour {

    private readonly Vector3 SAVING_SELECTOR = new Vector3(-8.7f, 2.0f, -25.38f);

    private RayCast rayCast;

    private InputManager inputManager;

    private Text saveLetterSelector;
    
    private SavingManager savingManager;
    private SavingUIHandler savingUIHandler;

    private GameObject player;
    private GameObject saveAsSelector;


    private string currentStringSelected;
    private char asciiCodeLetter;
    private bool canClick = true;

	void Start () {
        rayCast = GameObject.Find("PointerController").GetComponent<RayCast>();
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();

        player = GameObject.Find("Player");

        savingManager = GameObject.Find("SavingManager").GetComponent<SavingManager>();
        savingUIHandler = GameObject.Find("SavingUi").GetComponent<SavingUIHandler>();

        saveAsSelector = GameObject.Find("SelectorSaveAsMenu");

        DisplaySaveId();
    }
	

	void Update () {


        if (inputManager.IsTriggerClicked() && rayCast.Hit() && canClick)
        {
            

            if (rayCast.GetHit().transform.name == "ButtonDown")
            {

               saveLetterSelector = rayCast.GetHit().transform.parent.Find("LetterView").GetComponentInChildren<Text>();

                if (asciiCodeLetter == 'A')
                {
                    
                    asciiCodeLetter = 'Z';
                    saveLetterSelector.text = asciiCodeLetter.ToString();

                    canClick = false;

                }
                else
                {
                    asciiCodeLetter--;
                    saveLetterSelector.text = asciiCodeLetter.ToString();

                    canClick = false;
                }
            }
            else if(rayCast.GetHit().transform.name == "ButtonUp")
            {
                saveLetterSelector = rayCast.GetHit().transform.parent.Find("LetterView").GetComponentInChildren<Text>();

                if (asciiCodeLetter == 'Z')
                {
                    asciiCodeLetter = 'A';
                    saveLetterSelector.text = asciiCodeLetter.ToString();

                    canClick = false;
                }
                else
                {
                    asciiCodeLetter++;
                    saveLetterSelector.text = asciiCodeLetter.ToString();

                    canClick = false;
                }
            }
            else if(rayCast.GetHit().transform.name == "SaveAs")
            {
                for(int i = 0; i < gameObject.transform.childCount; i++)
                { 

                    if(gameObject.transform.GetChild(i).name == "LetterSelector")
                    {
                        currentStringSelected += gameObject.transform.GetChild(i).Find("LetterView").GetComponentInChildren<Text>().text;
                    }
                    
                }

                if(savingManager.GetCurrentConnexionID() == null)
                {
                    savingManager.SaveGameObjects(currentStringSelected);
                }                    
                else
                {
                    savingManager.UpdateCurrentSave();
                }

                TeleportBack();
                canClick = false;
            }

        }

        if (!canClick)
        {
            canClick = !inputManager.IsTriggerClicked();
        }

    }

    private void TeleportBack()
    {
        player.transform.position = savingUIHandler.GetPlayerSavingPosition();
        saveAsSelector.transform.position = SAVING_SELECTOR;
    }

    private void DisplaySaveId()
    {
        if (savingManager.GetCurrentConnexionID() == null)
        {

            string temp = savingManager.GenerateFreeSaveID();
            char[] freeGeneratedID = temp.ToCharArray();

            for (int i = 0; i < freeGeneratedID.Length; i++)
            {
                gameObject.transform.GetChild(i).Find("LetterView").GetComponentInChildren<Text>().text = freeGeneratedID[i].ToString();
                asciiCodeLetter = freeGeneratedID[i];
            }
        }
        else
        {
            string temp = savingManager.GetCurrentConnexionID();
            char[] freeGeneratedID = temp.ToCharArray();

            for (int i = 0; i < freeGeneratedID.Length; i++)
            {
                gameObject.transform.GetChild(i).Find("LetterView").GetComponentInChildren<Text>().text = freeGeneratedID[i].ToString();
                asciiCodeLetter = freeGeneratedID[i];
            }
        }
    }

}
