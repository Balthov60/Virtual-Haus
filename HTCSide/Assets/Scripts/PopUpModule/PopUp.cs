using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PopUp {

    /// <summary>
    /// Display A pop up.
    /// </summary>
    /// <param name="source">UI Container</param>
    /// <param name="message">Pop up Message</param>
    /// <param name="duration">Display duration in second</param>
    public static void DisplayBasicPopUp(GameObject source, string message, int duration)
    {
        GameObject popUpPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Scripts/PopUpModule/BasicTextPopUp.prefab");
        GameObject popUp = Object.Instantiate(popUpPrefab, source.transform);

        Vector2 parentSize = source.GetComponent<RectTransform>().sizeDelta;

        popUp.GetComponent<RectTransform>().sizeDelta = parentSize;
        popUp.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = parentSize * 10;
        popUp.transform.GetChild(0).GetComponent<Text>().text = message;

        popUp.GetComponent<SheduledGameObjectRemoval>().ScheduledRemoval(duration);
    }
}
