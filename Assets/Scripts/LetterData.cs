using UnityEngine;

[CreateAssetMenu(fileName = "LetterData", menuName = "WordGame/LetterData")]
public class LetterData : ScriptableObject
{
    public char character;
    public float frequency; // e.g., E = 12.7f, Z = 0.1f
    public GameObject prefab;
    public bool isVowel;
}

