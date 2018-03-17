using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    public Shader unlitShader;

    void Start()
    {
        GetComponent<Camera>().SetReplacementShader(unlitShader, "");
    }
}
