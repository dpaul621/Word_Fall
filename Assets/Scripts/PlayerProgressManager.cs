using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerProgressManager : MonoBehaviour
{
    private string baseUrl = "https://wordfall-backend.onrender.com";
    private string deviceId;
    public PlayerProgress progress;

    private void Start()
    {
        deviceId = SystemInfo.deviceUniqueIdentifier;
    }

    public void SaveEasyProgress(int level)
    {
        StartCoroutine(SendEasyProgress(new EasyProgress { deviceId = deviceId, levelEasy = level }));
    }

    public void SaveMediumProgress(int level)
    {
        StartCoroutine(SendMediumProgress(new MediumProgress { deviceId = deviceId, levelMedium = level }));
    }

    public void SaveHardProgress(int level)
    {
        StartCoroutine(SendHardProgress(new HardProgress { deviceId = deviceId, levelHard = level }));
    }
    public void SaveProgress()
    {
        StartCoroutine(SendProgress(new PlayerProgress { deviceId = deviceId, levelHard = 0, levelMedium = 0, levelEasy = 0 }));
    }

    IEnumerator SendProgress(PlayerProgress progress)
    {
        string json = JsonUtility.ToJson(progress);
        yield return StartCoroutine(SendProgressRequest(json));
    }

    IEnumerator SendEasyProgress(EasyProgress progress)
    {
        string json = JsonUtility.ToJson(progress);
        yield return StartCoroutine(SendProgressRequest(json));
    }

    IEnumerator SendMediumProgress(MediumProgress progress)
    {
        string json = JsonUtility.ToJson(progress);
        yield return StartCoroutine(SendProgressRequest(json));
    }

    IEnumerator SendHardProgress(HardProgress progress)
    {
        string json = JsonUtility.ToJson(progress);
        yield return StartCoroutine(SendProgressRequest(json));
    }

    IEnumerator SendProgressRequest(string json)
    {
        UnityWebRequest request = new UnityWebRequest(baseUrl + "/save", "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Progress saved: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Error saving progress: " + request.error);
        }
    }

    public void LoadProgress()
    {
        StartCoroutine(GetProgress());
    }

IEnumerator GetProgress()
{
    progress = null;
    string url = baseUrl + "/progress/" + deviceId;
    UnityWebRequest request = UnityWebRequest.Get(url);
    yield return request.SendWebRequest();

    if (request.result == UnityWebRequest.Result.Success)
    {
        progress = JsonUtility.FromJson<PlayerProgress>(request.downloadHandler.text);
    }
    else
    {
        if (request.error == "Cannot connect to destination host")
        {
            Debug.LogWarning("Server is not reachable: " + request.error);
        }
        else if (request.error == "HTTP/1.1 404 Not Found")
        {
            Debug.LogWarning("No progress found — creating new entry for device: " + deviceId);
            // Wait for save to complete
            yield return StartCoroutine(SendProgress(new PlayerProgress
            {
                deviceId = deviceId,
                levelEasy = 1, // or 0 depending on your logic
                levelMedium = 0,
                levelHard = 0
            }));

            // 🔁 Try fetching again now that we've created it
            yield return StartCoroutine(GetProgress());
        }
        else
        {
            Debug.LogError("Other request error: " + request.error);
        }
    }
}


    [System.Serializable]
    public class EasyProgress
    {
        public string deviceId;
        public int levelEasy;
    }

    [System.Serializable]
    public class MediumProgress
    {
        public string deviceId;
        public int levelMedium;
    }

    [System.Serializable]
    public class HardProgress
    {
        public string deviceId;
        public int levelHard;
    }
}
