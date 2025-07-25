using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
public class LetterSpawnScript : MonoBehaviour
{
    float scanTimer = 0f;
    public float scanInterval = 1f; // every 1 second
    int currentVowelCount = 0;
    int currentTotalLetterCount = 0;
    public List<LetterData> letterDataList;
    public List<float> spawnXValues;
    public float earlyGameTimer = 12f;
    public float lowRangeSpawnInterval = 0.5f;
    public float highRangeSpawnInterval = 2.0f;
    public float chanceOfAdditionalLetter = 0.2f;
    public float letterMovementSpeed;
    public float earlyLowRangeSpawnInterval = 0.5f;
    public float earlyHighRangeSpawnInterval = 1.5f;
    public float earlyChanceOfAdditionalLetter = 0.1f;
    public float earlyLetterMovementSpeed = 2f;
    public float currentLowRangeSpawnInterval;
    public float currentHighRangeSpawnInterval;
    public float currentChanceOfAdditionalLetter;
    public float currentLetterMovementSpeed;
    private Coroutine _spawnRoutine;
    private float? lastXValue = null;
    public float chancesOfABomb = 0.0f;
    public float chancesOfElectic = 0.0f;
    Letter letterScript;
    public float level;
    float sceneTimer;
    bool earlyTimerSet = false;
    bool lateTimerSet = false;
    private Queue<char> plantedLetterQueue = new Queue<char>();
    public float vowelRatio;
    private List<string> longWords = new List<string>();
    public TextAsset wordListFile;
    private float wordInjectTimer = 0f;
    public float wordInjectInterval = 20f;

    void OnDisable()
    {
        if (_spawnRoutine != null)
            StopCoroutine(_spawnRoutine);
    }
    void Awake()
    {
        currentLowRangeSpawnInterval = earlyLowRangeSpawnInterval;
        currentHighRangeSpawnInterval = earlyHighRangeSpawnInterval;
        currentChanceOfAdditionalLetter = earlyChanceOfAdditionalLetter;
        currentLetterMovementSpeed = earlyLetterMovementSpeed;
    }
    IEnumerator LevelSetter()
    {
        //wait until end of frame to ensure GameManager.Instance is ready
        yield return new WaitForEndOfFrame();
        earlyGameTimer = 12f + (level * 2f);
        if (GameManager.Instance.GMLevel > 1 && earlyGameTimer < 30)
            earlyGameTimer = 30f;
        if (GameManager.Instance.GMLevel > 30)
            earlyGameTimer = 0f;

        lowRangeSpawnInterval = 3f - (level * 0.1f);
        if (lowRangeSpawnInterval < 0.5f)
            lowRangeSpawnInterval = 0.5f;

        highRangeSpawnInterval = 4f - (level * 0.1f);
        if (highRangeSpawnInterval < 1f)
            highRangeSpawnInterval = 1f;

        chanceOfAdditionalLetter = 0.01f + (level * 0.005f);
        if (chanceOfAdditionalLetter >= 0.16f)
            chanceOfAdditionalLetter = 0.0f;
        letterMovementSpeed = -0.175f - (level * 0.01f);
        earlyLowRangeSpawnInterval = 1f;
        earlyHighRangeSpawnInterval = 2f;
        earlyChanceOfAdditionalLetter = 0.11f;
        earlyLetterMovementSpeed = -0.42f;
    }
    void Start()
    {
        _spawnRoutine = StartCoroutine(SpawnLoop());
        level = GameManager.Instance.GMLevel;
        StartCoroutine(LevelSetter());
        if (level >= 1 && level <= 10)
        {
            chancesOfABomb = 0f;
            chancesOfElectic = 0.0f;
        }
        if (level >= 11 && level <= 15)
        {
            chancesOfABomb = 0.15f;
            chancesOfElectic = 0.0f;
        }
        if (level >= 15 && level <= 20)
        {
            chancesOfABomb = 0.2f;
            chancesOfElectic = 0.0f;
        }
        if (level >= 21 && level <= 25)
        {
            chancesOfABomb = 0.2f;
            chancesOfElectic = 0.0f;
        }
        if (level >= 25 && level <= 30)
        {
            chancesOfABomb = 0.2f;
            chancesOfElectic = 0.0f;
        }
        if (level == 31)
        {
            chancesOfABomb = 0.0f;
            chancesOfElectic = 0.50f;
        }
        if (level == 32)
        {
            chancesOfABomb = 0.0f;
            chancesOfElectic = 0.45f;
        }
        if (level == 33)
        {
            chancesOfABomb = 0.0f;
            chancesOfElectic = 0.45f;
        }
        if (level == 34)
        {
            chancesOfABomb = 0.0f;
            chancesOfElectic = 0.40f;
        }
        if (level == 35)
        {
            chancesOfABomb = 0.0f;
            chancesOfElectic = 0.40f;

        }
        if (level == 36)
        {
            chancesOfABomb = 0.0f;
            chancesOfElectic = 0.35f;
        }
        if (level >= 37 && level <= 40)
        {
            chancesOfABomb = 0.20F;
            chancesOfElectic = 0.30f;
        }
        MakeSixLetterWords();
    }
    void MakeSixLetterWords()
    {
        wordListFile = Resources.Load<TextAsset>("my_scrabble_wordlist");
        if (wordListFile != null)
        {
        string[] allWords = wordListFile.text.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
        foreach (string rawWord in allWords)
        {
            string cleaned = rawWord.Trim().Replace("'", "").ToUpper();

            if (cleaned.Length >= 6 && cleaned.Length <= 10)
            {
                longWords.Add(cleaned);
            }
        }
        if (longWords.Count == 0)
        Debug.LogWarning("No long words found in word list!");
        }
    }
    void FixedUpdate()
    {
        sceneTimer += Time.fixedDeltaTime;
        if (sceneTimer < earlyGameTimer && !earlyTimerSet)
        {
            currentLowRangeSpawnInterval = earlyLowRangeSpawnInterval;
            currentHighRangeSpawnInterval = earlyHighRangeSpawnInterval;
            currentChanceOfAdditionalLetter = earlyChanceOfAdditionalLetter;
            currentLetterMovementSpeed = earlyLetterMovementSpeed;
            earlyTimerSet = true;
            Debug.Log("Using early game spawn parameters. early timerSet: " + earlyTimerSet);
        }
        if (sceneTimer > earlyGameTimer && !lateTimerSet)
        {
            if (currentChanceOfAdditionalLetter != chanceOfAdditionalLetter ||
                currentLowRangeSpawnInterval != lowRangeSpawnInterval ||
                currentHighRangeSpawnInterval != highRangeSpawnInterval ||
                currentLetterMovementSpeed != letterMovementSpeed)
            {
                Debug.Log("Updating spawn parameters after early game timer.");
                currentLowRangeSpawnInterval = lowRangeSpawnInterval;
                currentHighRangeSpawnInterval = highRangeSpawnInterval;
                currentChanceOfAdditionalLetter = chanceOfAdditionalLetter;
                currentLetterMovementSpeed = letterMovementSpeed;
                lateTimerSet = true;
                Debug.Log("Using late game spawn parameters. late timerSet: " + lateTimerSet);
            }
        }
        scanTimer += Time.deltaTime;
        if (scanTimer >= scanInterval)
        {
            UpdateLetterStats();
            scanTimer = 0f;
        }
        InjectRandomWord();
    }
    void InjectRandomWord()
    {
        wordInjectTimer += Time.deltaTime;
        if (wordInjectTimer >= wordInjectInterval)
        {
            TryInjectWord();
            wordInjectTimer = 0f;
            Debug.LogWarning("INJECTING WORD");
        }
    }
    void TryInjectWord()
    {
        if (longWords.Count == 0)
        {
            Debug.LogWarning("No long words available for injection.");
            return;
        }
        string randomWord = longWords[Random.Range(0, longWords.Count)];
        PlantWord(randomWord);
        Debug.Log($"Planted word: {randomWord}");
    }
    void UpdateLetterStats()
    {
        Letter[] letters = FindObjectsOfType<Letter>();
        currentVowelCount = 0;
        currentTotalLetterCount = letters.Length;

        foreach (Letter letter in letters)
        {
            if (letter.isVowel) currentVowelCount++;
        }
        //Debug.Log($"VowelRatio: {(float)currentVowelCount / Mathf.Max(currentTotalLetterCount, 1)}");

        currentTotalLetterCount = letters.Length;
    }
    private IEnumerator SpawnLoop()
    {
        yield return new WaitForSeconds(0.5f);
        while (true)
        {
            SpawnLetterFromWeightedList();
            float randomSpawnInterval = Random.Range(currentLowRangeSpawnInterval, currentHighRangeSpawnInterval);
            yield return new WaitForSeconds(randomSpawnInterval);
        }
    }
    public void PlantWord(string word)
    {
        List<char> scrambled = new List<char>(word.ToUpper());
        
        // Shuffle the letters
        for (int i = scrambled.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (scrambled[i], scrambled[j]) = (scrambled[j], scrambled[i]);
        }

        // Enqueue in scrambled order
        foreach (char c in scrambled)
        {
            plantedLetterQueue.Enqueue(c);
        }
    }

    void SpawnLetterFromWeightedList()
    {
        List<float> availableX = new List<float>(spawnXValues);
        if (lastXValue.HasValue)
            availableX.Remove(lastXValue.Value);

        float randomXValue = availableX[Random.Range(0, availableX.Count)];
        lastXValue = randomXValue;

        // 👇 NEW: choose planted letter if available
        LetterData selectedLetter;
        if (plantedLetterQueue.Count > 0)
        {
            char plantedChar = plantedLetterQueue.Dequeue();
            selectedLetter = letterDataList.Find(ld => ld.character == plantedChar);
        }
        else
        {
            selectedLetter = GetWeightedRandomLetter();
        }

        GameObject newLetter = Instantiate(selectedLetter.prefab, new Vector3(randomXValue, transform.position.y, 0), Quaternion.identity);
        Letter letterComponent = newLetter.GetComponent<Letter>();
        letterComponent.spawnXLocation = randomXValue;
        letterComponent.letterIsBomb = Random.Range(0f, 1f) < chancesOfABomb;

        if (!letterComponent.letterIsBomb)
        {
            letterComponent.isElectric = Random.Range(0f, 1f) < chancesOfElectic;
        }

        letterComponent.Setup(selectedLetter);

        // Handle possible second letter as normal (we won’t plant more than one at a time for now)
        if (Random.Range(0f, 1f) < currentChanceOfAdditionalLetter)
        {
            List<float> secondAvailableX = new List<float>(spawnXValues);
            secondAvailableX.Remove(randomXValue);
            float secondX = secondAvailableX[Random.Range(0, secondAvailableX.Count)];
            LetterData secondLetter = GetWeightedRandomLetter();
            GameObject secondObj = Instantiate(secondLetter.prefab, new Vector3(secondX, transform.position.y, 0), Quaternion.identity);
            Letter secondLetterComponent = secondObj.GetComponent<Letter>();
            secondLetterComponent.spawnXLocation = secondX;
            secondLetterComponent.letterIsBomb = Random.Range(0f, 1f) < chancesOfABomb;
            if (!secondLetterComponent.letterIsBomb)
                secondLetterComponent.isElectric = Random.Range(0f, 1f) < chancesOfElectic;
            secondLetterComponent.Setup(secondLetter);
        }
    }
    public void StompSpawn()
    {
        List<float> shuffledX = new List<float>(spawnXValues);
        ShuffleList(shuffledX); // Same helper as before

        for (int i = 0; i < shuffledX.Count; i++)
        {
            LetterData selectedLetter;

            if (i == 0 || i == 1) // First two are vowels
                selectedLetter = GetWeightedRandomVowel();
            else
                selectedLetter = GetWeightedRandomConsonant();

            GameObject newLetter = Instantiate(selectedLetter.prefab, new Vector3(shuffledX[i], transform.position.y, 0), Quaternion.identity);

            Letter letterComponent = newLetter.GetComponent<Letter>();
            letterComponent.spawnXLocation = shuffledX[i];
            letterComponent.letterIsBomb = Random.Range(0f, 1f) < chancesOfABomb;
            if (!letterComponent.letterIsBomb)
                letterComponent.isElectric = Random.Range(0f, 1f) < chancesOfElectic;

            letterComponent.Setup(selectedLetter);
        }
    }
    void ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
    LetterData GetWeightedRandomLetter()
    {
        vowelRatio = (float)currentVowelCount / (float)Mathf.Max(currentTotalLetterCount, 1); // prevent divide by zero

        if (vowelRatio < 0.35f)
        {
            return GetWeightedRandomVowel();
        }
        else if (vowelRatio > 0.45f)
        {
            return GetWeightedRandomConsonant();
        }
        else
        {
            return GetWeightedRandomFromList(letterDataList);
        }
    }
    LetterData GetWeightedRandomVowel()
    {
        List<LetterData> vowelList = letterDataList.FindAll(letter => letter.isVowel);
        return GetWeightedRandomFromList(vowelList);
    }
    LetterData GetWeightedRandomConsonant()
    {
        List<LetterData> consonantList = letterDataList.FindAll(letter => !letter.isVowel);
        return GetWeightedRandomFromList(consonantList);
    }
    LetterData GetWeightedRandomFromList(List<LetterData> list)
    {
        float totalWeight = 0f;
        foreach (var letter in list)
            totalWeight += letter.frequency;

        float rand = Random.Range(0, totalWeight);
        float cumulative = 0f;

        foreach (var letter in list)
        {
            cumulative += letter.frequency;
            if (rand <= cumulative)
                return letter;
        }

        return list[list.Count - 1]; // fallback
    }
}
