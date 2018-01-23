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
        savingUIHandler = GameObject.Find("SavingUI").GetComponent<SavingUIHandler>();

        saveAsSelector = GameObject.Find("SelectorSaveAsMenu");
        saveLetterSelector = GameObject.Find("LetterView").GetComponent<Text>();
    }
	

	void Update () {


        if (inputManager.IsTriggerClicked() && rayCast.Hit() && canClick)
        {

           
            
            if (rayCast.GetHit().transform.name == "ButtonDown")
            {
                print("asciiCodeLetter : " + asciiCodeLetter);
                saveLetterSelector = rayCast.GetHit().transform.parent.Find("LetterView").GetComponentInChildren<Text>();
                char[] temp = saveLetterSelector.text.ToCharArray();
                asciiCodeLetter = temp[0];
                saveLetterSelector.text = asciiCodeLetter.ToString();


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
                char[] temp = saveLetterSelector.text.ToCharArray();
                asciiCodeLetter = temp[0];
                saveLetterSelector.text = asciiCodeLetter.ToString();


                if (asciiCodeLetter == 'Z')
                {
                    asciiCodeLetter = 'A';
                    saveLetterSelector.text = asciiCodeLetter.ToString();

                    canClick = false;
                }
                else
                {
                    asciiCodeLetter++;
                    saveLetterSelector.text = asciiCodeLetter.ToString(); ;

                    canClick = false;
                }
            }
            else if(rayCast.GetHit().transform.name == "SaveAs")
            {
                canClick = false;

                print(gameObject.transform.childCount); // Renvoie 4 (pour SelectorSaveAsPanel) puis renvoie 5 (pour LoadSavePanel)


                for (int i = 0; i < gameObject.transform.childCount; i++)
                {                    
                    if (gameObject.transform.GetChild(i).name == "LetterSelector")
                    {
                        currentStringSelected += gameObject.transform.GetChild(i).Find("LetterView").GetComponentInChildren<Text>().text;
                        //print("parent : " + gameObject.transform.parent.name); //Appelé une première fois pour SelectorSaveAsPanel puis une deuxième fois pour LoadSavePanel
                        //print("Gparent : " + gameObject.transform.parent.transform.parent.name); //Appelé une première fois pour SelectorSaveAsPanel puis une deuxième fois pour LoadSavePanel
                    }

                }

                if(savingManager.GetCurrentConnexionID() == null || savingManager.GetCurrentConnexionID() != currentStringSelected)
                {
                    savingManager.SaveGameObjects(currentStringSelected);
                    print(currentStringSelected);
                }                    
                else if(savingManager.GetCurrentConnexionID() == currentStringSelected)
                {
                    savingManager.UpdateCurrentSave();
                }

                TeleportBack();
                currentStringSelected = "";
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

    public void DisplaySaveID()
    {
        char[] freeGeneratedID = savingUIHandler.GetRandomSaveIDForSelector();
        for (int i = 0; i < freeGeneratedID.Length; i++)
        {
            gameObject.transform.GetChild(i).Find("LetterView").GetComponentInChildren<Text>().text = freeGeneratedID[i].ToString();
            //print(freeGeneratedID[i].ToString());
        }
        
    }

}
