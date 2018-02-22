using UnityEngine;

public class TrackpadHandler : MonoBehaviour {

    private static readonly string MENU_TRACKPAD_X_NAME = "MenuControllerTrackpadX";
    private static readonly string MENU_TRACKPAD_Y_NAME = "MenuControllerTrackpadY";
    private static readonly string POINTER_TRACKPAD_X_NAME = "PointerControllerTrackpadX";
    private static readonly string POINTER_TRACKPAD_Y_NAME = "PointerControllerTrackpadY";
    private static readonly string MENU_TRACKPAD_BUTTON_NAME = "MenuControllerTrackpadButton";

    private static readonly Vector2 DEFAULT_TRACKPAD_POSITION = new Vector2(0, 0);

    private float? previousMenuTrackpadAngle;
    private float pointerTrackpadRotationOffset;

    private float? previousPointerTrackpadAngle;
    private float menuTrackpadRotationOffset;

    void Start()
    {
        previousMenuTrackpadAngle = previousPointerTrackpadAngle = null;
        menuTrackpadRotationOffset = pointerTrackpadRotationOffset = 0;
    }
	
	void Update()
    {
        UpdateTrackpadRotationOffset(GetMenuTrackpadPos(), ref previousMenuTrackpadAngle, ref menuTrackpadRotationOffset);
        UpdateTrackpadRotationOffset(GetPointerTrackpadPos(), ref previousPointerTrackpadAngle, ref pointerTrackpadRotationOffset);
    }

    /// <summary>
    /// Get rotation offset for the trackpad in params.
    /// </summary>
    /// <param name="trackpadPos"></param>
    /// <param name="previousTrackpadAngle"></param>
    /// <param name="trackpadRotationOffset"></param>
    private void UpdateTrackpadRotationOffset(Vector2 trackpadPos, ref float? previousTrackpadAngle, ref float trackpadRotationOffset)
    {
        trackpadRotationOffset = 0;

        if (trackpadPos == DEFAULT_TRACKPAD_POSITION)
        {
            previousTrackpadAngle = null;
            return;
        }

        float currentRotationAngle = GetTrackpadAngle(trackpadPos);

        if (previousTrackpadAngle == null) // Need at least one previous value to perform calculation
        {
            previousTrackpadAngle = currentRotationAngle;
            return;
        }

        trackpadRotationOffset = (currentRotationAngle - (float)previousTrackpadAngle);
        previousTrackpadAngle = currentRotationAngle;
    }
    private float GetTrackpadAngle(Vector2 trackpadPos)
    {
        trackpadPos.Normalize();
        if (trackpadPos.y <= 0)
        {
            return 360 - (Mathf.Acos(trackpadPos.x) * Mathf.Rad2Deg);
        }
        else
        {
            return (Mathf.Acos(trackpadPos.x) * Mathf.Rad2Deg);
        }
    }

    
    // Public Interface

    public bool IsMenuTrackpadClicked()
    {
        return Input.GetButton(MENU_TRACKPAD_BUTTON_NAME);
    }

    public Vector2 GetMenuTrackpadPos()
    {
        return new Vector2(Input.GetAxis(MENU_TRACKPAD_X_NAME), Input.GetAxis(MENU_TRACKPAD_Y_NAME));
    }
    public Vector2 GetPointerTrackpadPos()
    {
        return new Vector2(Input.GetAxis(POINTER_TRACKPAD_X_NAME), Input.GetAxis(POINTER_TRACKPAD_Y_NAME));
    }

    public float GetMenuTrackpadRotationOffset()
    {
        return menuTrackpadRotationOffset;
    }
    public float GetPointerTrackpadRotationOffset()
    {
        return pointerTrackpadRotationOffset;
    }
}
