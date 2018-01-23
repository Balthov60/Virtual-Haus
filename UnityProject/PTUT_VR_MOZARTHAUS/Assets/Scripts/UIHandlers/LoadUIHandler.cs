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
           
            if (rayCast.GetHit().transform.name == "Load")
            {
                print(rayCast.GetHit().transform.parent.transform.childCount);
                for (int i = 0; i < gameObject.transform.parent.transform.childCount; i++)
                {
                    print("For");
                    print(gameObject.transform.parent.transform.GetChild(i).name);
                    if (gameObject.transform.parent.transform.GetChild(i).name == "LetterSelector")
                    {
                        print("If");
                        currentStringSelected += gameObject.transform.parent.transform.GetChild(i).Find("LetterView").GetComponent<Text>().text;
                    }

                }
                print(currentStringSelected); // Renvoie Null
                savingManager.LoadGameObjects(currentStringSelected);

                TeleportHome();
                canClick = false;
            }
            else if (rayCast.GetHit().transform.name == "NewGame")
            {
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


