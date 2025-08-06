using System;

[System.Serializable]
public class PlayerProgress
{
    public string deviceId;
    public int levelEasy;
    public int levelMedium;
    public int levelHard;
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

