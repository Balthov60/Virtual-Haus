using UnityEngine;
using UnityEngine.UI;

public class PopUp {

    private static readonly string POP_UP_FOLDER_PATH = "UIComponents/PopUpModule/Prefabs/";

    /// <summary>
    /// Display A basic pop up for a defined duration.
    /// </summary>
    /// <param name="source">UI Container</param>
    /// <param name="message">Pop up Message</param>
    /// <param name="duration">Display duration in second</param>
    public static void DisplayScheduledPopUp(GameObject source, string message, int duration)
    {
        GameObject popUp = InstantiatePopUp("BasicTextPopUp", source);

        popUp.GetComponent<ScheduledPopUp>().ScheduledRemoval(duration);

        DisplayPopUp(popUp, source, message);
    }

    /// <summary>
    /// Display A Validation pop up who will wait for a user interaction.
    /// </summary>
    /// <param name="source">UI Container</param>
    /// <param name="message">Pop up Message</param>
    /// <param name="callback">Function to call if user click yes</param>
    public static void DisplayValidationPopUp(GameObject source, string message, ValidationPopUp.Callback callback)
    {
        GameObject popUp = InstantiatePopUp("ValidationPopUp", source);

        popUp.GetComponent<ValidationPopUp>().SetCallback(callback);

        DisplayPopUp(popUp, source, message);
    }

    /* Common Methods */

    private static GameObject InstantiatePopUp(string popUpName, GameObject source)
    {
        GameObject popUpPrefab = Resources.Load<GameObject>(POP_UP_FOLDER_PATH + popUpName);

        return Object.Instantiate(popUpPrefab, source.transform);
    }

    private static void DisplayPopUp(GameObject popUp, GameObject source, string message)
    {
        Vector2 popUpSize = popUp.GetComponent<RectTransform>().sizeDelta;
        Vector2 sizeToFit = source.GetComponent<RectTransform>().sizeDelta * 0.9f;

        Vector3 localScale = GetPopUpScale(popUpSize, sizeToFit);
        localScale.z = 1;
        popUp.transform.localScale = localScale;

        popUp.transform.GetChild(0).GetComponent<Text>().text = message;
    }
    private static Vector2 GetPopUpScale(Vector2 popUpSize, Vector2 sizeToFit)
    {
        if (sizeToFit.x / popUpSize.x < sizeToFit.y / popUpSize.y)
        {
            return Vector2.one * (sizeToFit.x / popUpSize.x);
        }
        else
        {
            return Vector2.one * (sizeToFit.y / popUpSize.y);
        }
    }
}
