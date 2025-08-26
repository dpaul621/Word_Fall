using System.IO;
using UnityEngine;

public static class OneModeSaveManager
{
    private static string SaveFilePath => Path.Combine(Application.persistentDataPath, "oneModeProgress.json");

    public static OneDifficultyModeProgress Load()
    {
        if (File.Exists(SaveFilePath))
        {
            string json = File.ReadAllText(SaveFilePath);
            return JsonUtility.FromJson<OneDifficultyModeProgress>(json);
        }

        return new OneDifficultyModeProgress
        {
            deviceId = SystemInfo.deviceUniqueIdentifier,
            oneDifficultyModeLevel = 0 // start at 0 (or 1 if you prefer)
        };
    }

    public static void Save(OneDifficultyModeProgress newProgress)
    {
        // Merge-with-existing and only bump forward, like your current logic
        OneDifficultyModeProgress existing = Load();
        bool shouldSave = false;

        if (string.IsNullOrEmpty(existing.deviceId))
        {
            existing.deviceId = SystemInfo.deviceUniqueIdentifier;
            shouldSave = true;
            Debug.Log($"Setting deviceId to {existing.deviceId} and should save {shouldSave}");
        }

        if (newProgress.oneDifficultyModeLevel > existing.oneDifficultyModeLevel)
        {
            Debug.Log($"Updating oneDifficultyModeLevel from {existing.oneDifficultyModeLevel} to {newProgress.oneDifficultyModeLevel}");
            existing.oneDifficultyModeLevel = newProgress.oneDifficultyModeLevel;
            shouldSave = true;
        }

        if (shouldSave)
        {
            string json = JsonUtility.ToJson(existing);
            File.WriteAllText(SaveFilePath, json);
            Debug.Log($"Saved OneDifficultyModeProgress and should save = {shouldSave}");
        }
    }

    public static bool HasSaveFile() => File.Exists(SaveFilePath);

    public static void DeleteSaveFile()
    {
        if (File.Exists(SaveFilePath))
            File.Delete(SaveFilePath);
    }
}
