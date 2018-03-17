using UnityEngine;

public class PlayerPointerHandler : MonoBehaviour
{
    public GameObject mainCamera;

    void Update()
    {
        Vector3 newRotation = transform.rotation.eulerAngles;
        newRotation.y = mainCamera.transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Euler(newRotation);

        transform.localPosition = mainCamera.transform.localPosition = Vector3.zero;
    }
}
