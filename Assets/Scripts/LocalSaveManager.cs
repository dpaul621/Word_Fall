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
            return JsonUtility.FromJson<PlayerProgress>(json);
        }

        // Default on first run
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
}
