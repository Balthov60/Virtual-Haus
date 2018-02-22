using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SavedScene
{
    public string name;
    public List<SavedGameObject> savedGameObjects;

    public SavedScene(string name, List<GameObject> gameObjectToSave)
    {
        this.name = name;

        savedGameObjects = new List<SavedGameObject>();
        foreach (GameObject editableGameObject in gameObjectToSave)
        {
            savedGameObjects.Add(new SavedGameObject(editableGameObject.name, editableGameObject.transform));
        }
    }
}

