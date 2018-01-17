using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

/// TODO: handle multiple scene and multiple save.

/// <summary>
/// Consider only game objects with "Editable" tag.
/// Save name, position and rotation for each game objects
/// those data are store in a JSON File
/// </summary>
public class SavingManager : MonoBehaviour {

    private GameObject[] editableGameObjects;

    private static readonly string EDITABLE_TAG = "Editable";
    private static string SAVING_DIRECTORY;
    private static string SAVING_FILE = "test.save";
    private String currentConnexionID;
    private String currentDirectoryName;
    public GUIText fileSaveID;

    public void Start()
    {
        SAVING_DIRECTORY = Application.dataPath + "/Saves";
        fileSaveID = GetComponent<GUIText>();
        editableGameObjects = GameObject.FindGameObjectsWithTag(EDITABLE_TAG);
        
    }

    /// <summary>
    /// load games objects data and place them on the scene.
    /// </summary>
    public void LoadGameObjects()
    {
        string savingPath = SAVING_DIRECTORY + '/' + SceneManager.GetActiveScene().name + '/' + SAVING_FILE;
        if (File.Exists(savingPath))
        {
            string data = File.ReadAllText(savingPath);
            SavedScene savedScene = JsonUtility.FromJson(data, typeof(SavedScene)) as SavedScene;

            PlaceGameObjects(savedScene);
        }
    }
    private void PlaceGameObjects(SavedScene savedScene)
    {
        foreach (SavedGameObject savedGameObject in savedScene.savedGameObjects)
        {
            GameObject gameObject = GameObject.Find(savedGameObject.title);
            gameObject.transform.position = savedGameObject.position;
            gameObject.transform.rotation = savedGameObject.rotation;
        }
    }

    /// <summary>
    /// Save each editable game objects informations.
    /// </summary>
    public void SaveGameObjects()
    {
        editableGameObjects = GameObject.FindGameObjectsWithTag(EDITABLE_TAG);
        SavedScene savedScene = new SavedScene(SceneManager.GetActiveScene().name, editableGameObjects);
        

        CreateSavingDirectoryIfNotExist(savedScene.name);

        String saveID = GenerateUserSaveID();
        SetCurrentConnexionID(saveID);

        File.WriteAllText(SAVING_DIRECTORY + "/" + savedScene.name + "/" + saveID + ".save", JsonUtility.ToJson(savedScene));

        String[] directoriesName = GetDirectoriesName(SAVING_DIRECTORY);
        foreach (string directoryName in directoriesName)
        {
            String[] filesName = GetFilesName(SAVING_DIRECTORY + "/" + directoryName);
            foreach(string fileName in filesName)
            {
                Debug.Log(directoryName + " " + fileName);
            }
        }
    }

    /// <summary>
    /// Search if there are saves that have been created 
    /// </summary>
    /// <returns>true if used usedSaveIdentifiers.Count == 0</returns>
    public bool IsUserSavedExist()
    {
        List<String> usedSaveIdentifiers = new List<string>();
        usedSaveIdentifiers = GetUsedSaveIDs();
        print(usedSaveIdentifiers.Count);

        if (usedSaveIdentifiers.Count == 0) { return true; }
        else { return false; }

    }

    public void UpdateCurrentSave()
    {
        editableGameObjects = GameObject.FindGameObjectsWithTag(EDITABLE_TAG);
        SavedScene savedScene = new SavedScene(SceneManager.GetActiveScene().name, editableGameObjects);

        if (GetConnexionID() != null)
        {
            String[] directoriesName = GetDirectoriesName(SAVING_DIRECTORY);
            foreach (string directoryName in directoriesName)
            {
                String[] filesName = GetFilesName(SAVING_DIRECTORY + "/" + directoryName);
                foreach (string fileName in filesName)
                {
                    if (fileName == GetConnexionID())
                    {
                        File.Delete(SAVING_DIRECTORY + "/" + savedScene.name + "/" + GetConnexionID() + ".save");
                        print("Fichier :" + GetConnexionID() + "Deleted");
                        File.WriteAllText(SAVING_DIRECTORY + "/" + savedScene.name + "/" + GetConnexionID() + ".save", JsonUtility.ToJson(savedScene));
                        print("Fichier :" + GetConnexionID() + "Rewrite");
                    }
                }
            }
        }
    }

    public void SetCurrentConnexionID(String connexionID)
    {
        this.currentConnexionID = connexionID;
    }

    public String GetConnexionID() { return this.currentConnexionID; }

    /// <summary>
    /// Create current scene saving directory if not exist
    /// </summary>
    /// <param name="directory">Saving directory for current scene</param>
    private void CreateSavingDirectoryIfNotExist(string directory)
    {
        ;
        if (!Directory.Exists(SAVING_DIRECTORY))
            Directory.CreateDirectory(SAVING_DIRECTORY);
        if (!Directory.Exists(SAVING_DIRECTORY + '/' + directory))
            Directory.CreateDirectory(SAVING_DIRECTORY + '/' + directory);
    }

    private String GenerateFreeSaveID()
    {
        List<String> usedSaveIdentifiers = GetUsedSaveIDs();
        String saveIdentifier;

        do
        {
            saveIdentifier = GenerateRandomSaveID();
        }
        while (usedSaveIdentifiers.Contains(saveIdentifier));

        return saveIdentifier;

    }
    private String GenerateUserSaveID()
    {
        foreach (char c in Input.inputString)
        {
            if (c == '\b') // has backspace/delete been pressed?
            {
                if (fileSaveID.text.Length != 0)
                {
                    fileSaveID.text = fileSaveID.text.Substring(0, fileSaveID.text.Length - 1);
                }
            }
            else if ((c == '\n') || (c == '\r')) // enter/return
            {
                print("User entered their name: " + fileSaveID.text);
            }
            else
            {
                fileSaveID.text += c;
            }
        }
        return fileSaveID.ToString();
    }
    private List<String> GetUsedSaveIDs()
    {
        List<String> usedSaveIdentifiers = new List<string>();

        String[] directoriesName = GetDirectoriesName(SAVING_DIRECTORY);
        foreach (string directoryName in directoriesName)
        {
            String[] filesName = GetFilesName(SAVING_DIRECTORY + "/" + directoryName);
            foreach (string fileName in filesName)
            {
                usedSaveIdentifiers.Add(fileName);
            }
        }

        return usedSaveIdentifiers;
    }
    private String GenerateRandomSaveID()
    {
        String saveIdentifier = "";

        for (int i = 0; i < 3; i++)
            saveIdentifier += (char) UnityEngine.Random.Range('A', 'Z');

        return saveIdentifier;
    }

    private String[] GetDirectoriesName(string path)
    {
        String[] directories = Directory.GetDirectories(path);

        for (int i = 0; i < directories.Length; i++)
            directories[i] = directories[i].Split('\\')[1];

        return directories;
    }
    private String[] GetFilesName(string path)
    {
        String[] files = Directory.GetFiles(path, "*.save");

        for (int i = 0; i < files.Length; i++)
            files[i] = files[i].Split('\\')[1].Split('.')[0];

        return files;
    }

    
}


[Serializable]
public class SavedScene
{
    public string name;
    public List<SavedGameObject> savedGameObjects;

    public SavedScene(string name, GameObject[] gameObjectToSave)
    {
        this.name = name;
       

        savedGameObjects = new List<SavedGameObject>();
        foreach (GameObject editableGameObject in gameObjectToSave)
        {
            savedGameObjects.Add(new SavedGameObject(editableGameObject.name, editableGameObject.transform));
        }
    }
}

[Serializable]
public class SavedGameObject
{
    public string title;
    public Vector3 position;
    public Quaternion rotation;

    public SavedGameObject(string title, Transform transform)
    {
        this.title = title;
        this.rotation = transform.rotation;
        this.position = transform.position;
    }
}

