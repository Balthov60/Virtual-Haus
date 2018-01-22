using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LetterSelectorUIHandler : MonoBehaviour {

    private RayCast rayCast;
    private InputManager inputManager;
    private Text saveLetterSelector;
    private GameObject saveAsSelector;
    private SavingManager savingManager;

    private string currentStringSelected;

    private char asciiCodeLetter;

    private bool canClick = true;



	void Start () {
        rayCast = GameObject.Find("PointerController").GetComponent<RayCast>();
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
        savingManager = new SavingManager();
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

                SetCurrentStringSelected(currentStringSelected);
                print(currentStringSelected);

                canClick = false;
            }

        }

        if (!canClick)
        {
            canClick = !inputManager.IsTriggerClicked();
        }

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

    public string GetCurrentStringSelected() { return this.currentStringSelected;  }
    public void SetCurrentStringSelected(string currentStringSelected) { this.currentStringSelected = currentStringSelected; }
}
