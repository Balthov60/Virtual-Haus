using UnityEngine;

public class InputManager : MonoBehaviour {

    private static readonly string MOD_CHANGE_BUTTON_NAME = "ChangeModButton";
    private static readonly string HOME_BUTTON_NAME = "HomeButton";
    private static readonly string CLICKED_TRIGGER_NAME = "PointerTrigger";

    private RayCast rayCast;
    private TrackpadHandler trackpadHandler;

    /* Click Handling (Prevent Double Click) */

    private static readonly float CLICK_COOLDOWN_IN_SECOND = 0.2f;
    private bool _canClick;

    public bool CanClick
    {
        set
        {
            if (!value) Invoke("EnableClick", CLICK_COOLDOWN_IN_SECOND);
            _canClick = value;
        }
        get
        {
            return _canClick;
        }
    }
    private void EnableClick()
    {
        _canClick = true;
    }

    /*****************************************/

    private void Start()
    {
        rayCast = GameObject.Find("PointerController").GetComponent<RayCast>();
        trackpadHandler = GameObject.Find("InputManager").GetComponent<TrackpadHandler>();

        CanClick = true;
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

    public bool IsHomeButtonClicked()
    {
        return Input.GetButtonDown(HOME_BUTTON_NAME);
    }

    /// <summary>
    /// If you want to check if user click on something user UserClick()
    /// </summary>
    /// <returns>True when trigger is clicked.</returns>
    public bool IsTriggerClicked()
    {
        return CanClick && (Input.GetAxis(CLICKED_TRIGGER_NAME) == 1 || Input.GetButton(CLICKED_TRIGGER_NAME));
    }

    /// <summary>
    /// Check if Trigger is Clicked, if raycast hit something & check if canClick is true to prevent double click.
    /// 
    /// /!\ set InputManager.canClick to false if user click make an action.
    /// </summary>
    /// <returns>boolean true if user click works</returns>
    public bool UserClick()
    {
        return (IsTriggerClicked() && rayCast.Hit());
    }
}
