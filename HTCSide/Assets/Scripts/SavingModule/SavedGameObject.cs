using System;
using UnityEngine;

[Serializable]
public class SavedGameObject
{
    public string name;
    public Vector3 position;
    public Quaternion rotation;

    public SavedGameObject(string name, Transform transform)
    {
        this.name = name;
        this.rotation = transform.rotation;
        this.position = transform.position;
    }
}