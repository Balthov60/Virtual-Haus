using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SavingUtils
{
    public static readonly string SAVING_EXTENSION = ".save";

    public static string SavingDirectory
    {
        get { return Application.dataPath + "/Saves/" + SceneManager.GetActiveScene().name + "/"; }
    }
    public static void CreateSavingDirectoryIfNotExist()
    {
        if (!Directory.Exists(SavingDirectory))
            Directory.CreateDirectory(SavingDirectory);
    }

    /* Saving IDs Utilities methods */

    public static String GenerateFreeSaveID()
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
    private static List<String> GetUsedSaveIDs()
    {
        List<String> usedSaveIdentifiers = new List<string>();

        String[] directoriesName = GetDirectoriesName(SavingDirectory);
        foreach (string directoryName in directoriesName)
        {
            String[] filesName = GetFilesName(SavingDirectory + "/" + directoryName);
            foreach (string fileName in filesName)
            {
                usedSaveIdentifiers.Add(fileName);
            }
        }

        return usedSaveIdentifiers;
    }
    private static String GenerateRandomSaveID()
    {
        String saveIdentifier = "";

        for (int i = 0; i < 3; i++)
            saveIdentifier += (char)UnityEngine.Random.Range('A', 'Z');

        return saveIdentifier;
    }

    private static String[] GetDirectoriesName(string path)
    {
        String[] directories = Directory.GetDirectories(path);

        for (int i = 0; i < directories.Length; i++)
            directories[i] = directories[i].Split('\\')[1];

        return directories;
    }
    private static String[] GetFilesName(string path)
    {
        String[] files = Directory.GetFiles(path, "*" + SAVING_EXTENSION);

        for (int i = 0; i < files.Length; i++)
            files[i] = files[i].Split('\\')[1].Split('.')[0];

        return files;
    }
}
