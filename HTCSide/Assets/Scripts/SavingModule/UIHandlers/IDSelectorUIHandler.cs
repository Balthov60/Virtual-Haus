using UnityEngine;
using UnityEngine.UI;

public class IDSelectorUIHandler : MonoBehaviour
{
    private RayCast rayCast;
    private InputManager inputManager;

    void Start()
    {
        rayCast = GameObject.Find("PointerController").GetComponent<RayCast>();
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
    }

    void Update()
    {
        if (!inputManager.UserClick()) return;

        if (rayCast.GetHit().transform.name == "ButtonDown")
        {
            inputManager.CanClick = false;

            Text currentLetterSelector = rayCast.GetHit().transform.parent.Find("LetterView").GetComponentInChildren<Text>();
            currentLetterSelector.text = GetNextChar(currentLetterSelector.text[0]).ToString();
        }
        else if (rayCast.GetHit().transform.name == "ButtonUp")
        {
            inputManager.CanClick = false;

            Text currentLetterSelector = rayCast.GetHit().transform.parent.Find("LetterView").GetComponentInChildren<Text>();
            currentLetterSelector.text = GetPreviousChar(currentLetterSelector.text[0]).ToString();
        }
    }

    private char GetNextChar(char c)
    {
        int ascii = c;

        ascii = (++c - 65) % 26 + 65;

        return (char)ascii;
    }
    private char GetPreviousChar(char c)
    {
        // Forced to use a test negative modulo doesn't work
        int ascii = c;
        ascii = (--c - 65);

        if (ascii < 0)
        {
            ascii += 26;
        }
        ascii = ascii % 26 + 65;

        return (char)ascii;
    }

    public string GetID()
    {
        string selectedID = "";

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name == "LetterSelector")
            {
                selectedID += transform.GetChild(i).Find("LetterView").GetComponent<Text>().text;
            }
        }

        return selectedID;
    } 
    public void SetID(string id)
    {
        for (int i = 0; i < id.Length; i++)
        {
            if (transform.GetChild(i).name == "LetterSelector")
            {
                transform.GetChild(i).Find("LetterView").GetComponent<Text>().text = id[i].ToString();
            }
        }
    }
}
