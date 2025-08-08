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

    public static void Save(PlayerProgress newProgress)
    {
        PlayerProgress existingProgress = Load();

        // Only update if new progress is higher for each difficulty
        bool shouldSave = false;

        if (newProgress.levelEasy > existingProgress.levelEasy)
        {
            existingProgress.levelEasy = newProgress.levelEasy;
            shouldSave = true;
        }

        if (newProgress.levelMedium > existingProgress.levelMedium)
        {
            existingProgress.levelMedium = newProgress.levelMedium;
            shouldSave = true;
        }

        if (newProgress.levelHard > existingProgress.levelHard)
        {
            existingProgress.levelHard = newProgress.levelHard;
            shouldSave = true;
        }

        // Save only if there were any changes
        if (shouldSave)
        {
            string json = JsonUtility.ToJson(existingProgress);
            File.WriteAllText(SaveFilePath, json);
        }
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
