using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SavingUtils
{
    public static readonly string SAVING_EXTENSION = ".save";
    private static string SavingDirectory
    {
        get { return Application.dataPath + "/Saves/"; }
    }

    public static string CurrentSavingDirectory
    {
        get { return SavingDirectory + SceneManager.GetActiveScene().name + "/"; }
    }
    public static void CreateSavingDirectoryIfNotExist()
    {
        if (!Directory.Exists(CurrentSavingDirectory))
            Directory.CreateDirectory(CurrentSavingDirectory);
    }

    /* Saving IDs Utilities methods */

    public static bool IsIdUsed(string id)
    {
        return GetUsedSaveIDs().Contains(id);
    }
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

        foreach (string directoryPath in Directory.GetDirectories(SavingDirectory))
        {
            String[] filesName = GetFilesName(directoryPath);
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

    private static String[] GetFilesName(string path)
    {
        String[] files = Directory.GetFiles(path, "*" + SAVING_EXTENSION);

        for (int i = 0; i < files.Length; i++)
        {
            String[] fileSplitted = files[i].Split('\\');
            files[i] = fileSplitted[fileSplitted.Length - 1].Split('.')[0];
        }

        return files;
    }
}
