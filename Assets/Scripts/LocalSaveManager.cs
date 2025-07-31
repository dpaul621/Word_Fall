using System.IO;
using UnityEngine;

public static class LocalSaveManager
{
    private static string SaveFilePath => Path.Combine(Application.persistentDataPath, "playerProgress.json");

    public static PlayerProgress Load()
    {
        if (File.Exists(SaveFilePath))
        {
            string json = File.ReadAllText(SaveFilePath);
            //Debug.Log("Loaded player progress from " + json);
            return JsonUtility.FromJson<PlayerProgress>(json);
        }

        return new PlayerProgress
        {
            deviceId = SystemInfo.deviceUniqueIdentifier,
            levelEasy = 1,
            levelMedium = 0,
            levelHard = 0
        };
    }

    public static void Save(PlayerProgress progress)
    {
        string json = JsonUtility.ToJson(progress);
        File.WriteAllText(SaveFilePath, json);
    }

    public static bool HasSaveFile()
    {
        return File.Exists(SaveFilePath);
    }
    public static void DeleteSaveFile()
    {
        Debug.Log("Attempting to delete save file...");
        string path = Application.persistentDataPath + "/playerProgress.json";
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Save file deleted.");
        }
        else
        {
            Debug.Log("No save file found to delete.");
        }
    }
}
