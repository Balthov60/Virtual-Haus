using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadUIHandler : MonoBehaviour {

    private readonly Vector3 MOZART_HAUS_SPAWN = new Vector3(-2, 0.5f, -3.5f);


    private RayCast rayCast;
    private InputManager inputManager;

    private GameObject player;

    private SavingManager savingManager;
    private SavingUIHandler savingUIHandler;

    private GameObject loadSelector;



    private Text saveLetterSelector;


    private bool canClick = true;
    private char asciiCodeLetter;
    private string currentStringSelected;


    void Start () {
        rayCast = GameObject.Find("PointerController").GetComponent<RayCast>();
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();

        player = GameObject.Find("Player");

        savingManager = GameObject.Find("SavingManager").GetComponent<SavingManager>();
        savingUIHandler = GameObject.Find("SavingUI").GetComponent<SavingUIHandler>();

        loadSelector = GameObject.Find("SelectorLoadSave");
    }

    void Update () {
        if (inputManager.IsTriggerClicked() && canClick && rayCast.Hit())
        {
            if (rayCast.GetHit().transform.name == "ButtonDownLoad")
            {
                print("asciiCodeLetter : " + asciiCodeLetter);
                saveLetterSelector = rayCast.GetHit().transform.parent.Find("LetterViewLoad").GetComponentInChildren<Text>();
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
            else if (rayCast.GetHit().transform.name == "ButtonUpLoad")
            {
                saveLetterSelector = rayCast.GetHit().transform.parent.Find("LetterViewLoad").GetComponentInChildren<Text>();
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
            else if (rayCast.GetHit().transform.name == "Load")
            {
                for (int i = 0; i < gameObject.transform.childCount; i++)
                {

                    if (gameObject.transform.GetChild(i).name == "LetterSelectorLoad")
                    {
                        currentStringSelected += gameObject.transform.GetChild(i).Find("LetterViewLoad").GetComponentInChildren<Text>().text;
                    }

                }

                savingManager.LoadGameObjects(currentStringSelected);

                TeleportHome();
                canClick = false;
            }

        }

        if (!canClick)
        {
            canClick = !inputManager.IsTriggerClicked();
        }
    }

    private void TeleportHome()
    {
        player.transform.position = MOZART_HAUS_SPAWN;
    }

}


