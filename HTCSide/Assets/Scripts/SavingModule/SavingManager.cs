using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Consider only GameObject with Tags given by user in script options.
/// 
/// Save name, position and rotation for each game objects
/// those data are store in a JSON File
/// </summary>
public class SavingManager : MonoBehaviour {

    [SerializeField] private List<string> tags;

    private List<GameObject> editableGameObjects;
    private string currentSaveId;

    private void Start()
    {
        currentSaveId = null;
        editableGameObjects = new List<GameObject>();

        foreach (string tag in tags)
        {
            editableGameObjects.AddRange(GameObject.FindGameObjectsWithTag(tag));
        }
    }

    public string GetCurrentSaveID()
    {
        return currentSaveId;
    }

    /// <summary>
    /// Save each editable game objects on current save ID.
    /// </summary>
    /// <returns>boolean : true if currentSaveId exist false elsewhere</returns>
    public bool UpdateCurrentSave()
    {
        if (currentSaveId == null) return false;
        SaveGameObjects(currentSaveId);
        return true;
    }
    /// <summary>
    /// Save each editable game objects informations.
    /// </summary>
    public void SaveGameObjects(string savingID)
    {
        SavedScene savedScene = new SavedScene(SceneManager.GetActiveScene().name, editableGameObjects);
        SavingUtils.CreateSavingDirectoryIfNotExist();

        string json = JsonUtility.ToJson(savedScene);
        File.WriteAllText(SavingUtils.SavingDirectory + savingID + SavingUtils.SAVING_EXTENSION, json);
    }

    /// <summary>
    /// load games objects data and place them on the scene.
    /// </summary>
    /// <returns>boolean : true if savingID exist false elsewhere</returns>
    public bool LoadGameObjects(string savingID)
    {
        string savingPath = SavingUtils.SavingDirectory + savingID + SavingUtils.SAVING_EXTENSION;
        if (!File.Exists(savingPath)) return false;

        string json = File.ReadAllText(savingPath);
        SavedScene savedScene = JsonUtility.FromJson<SavedScene>(json);

        PlaceGameObjects(savedScene);
        currentSaveId = savingID;

        return true;
    }

    private void PlaceGameObjects(SavedScene savedScene)
    {
        foreach (SavedGameObject savedGameObject in savedScene.savedGameObjects)
        {
            GameObject gameObject = GameObject.Find(savedGameObject.name);
            gameObject.transform.position = savedGameObject.position;
            gameObject.transform.rotation = savedGameObject.rotation;
        }
    }
}