using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPointerHandler : MonoBehaviour
{

    public GameObject camera;

    void Update()
    {
        Vector3 newRotation = transform.rotation.eulerAngles;
        newRotation.y = camera.transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Euler(newRotation);

        transform.localPosition = camera.transform.localPosition = Vector3.zero;
    }
}
