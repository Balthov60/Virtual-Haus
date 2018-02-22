using UnityEngine;

public class InputManager : MonoBehaviour {

    private static readonly string MOD_CHANGE_BUTTON_NAME = "ChangeModButton";
    private static readonly string CLICKED_TRIGGER_NAME = "PointerTrigger";

    private TrackpadHandler trackpadHandler;

    private void Start()
    {
        trackpadHandler = GameObject.Find("InputManager").GetComponent<TrackpadHandler>();
    }

    // Public Interface

    public TrackpadHandler GetTrackpadHandler()
    {
        return trackpadHandler;
    }

    public bool IsModChangeButtonClicked()
    {
        return Input.GetButtonDown(MOD_CHANGE_BUTTON_NAME);
    }
    public bool IsTriggerClicked()
    {
        return (Input.GetAxis(CLICKED_TRIGGER_NAME) == 1 || Input.GetButton(CLICKED_TRIGGER_NAME));
    }
}
