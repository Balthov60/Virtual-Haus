using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheduledGameObjectRemoval : MonoBehaviour
{
    public void ScheduledRemoval(int duration)
    {
        Invoke("RemoveGO", duration);
    }

    private void RemoveGO()
    {
        Destroy(gameObject);
    }
}


