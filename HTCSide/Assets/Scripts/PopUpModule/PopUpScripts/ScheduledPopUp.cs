using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScheduledPopUp : DefaultPopUp
{
    public void ScheduledRemoval(int duration)
    {
        Invoke("Remove", duration);
    }
}


