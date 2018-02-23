using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultPopUp : MonoBehaviour
{

    protected void Start()
    {
        gameObject.transform.position -= gameObject.transform.parent.forward * 0.15f;
    }

    protected void Remove()
    {
        Destroy(gameObject);
    }
}

