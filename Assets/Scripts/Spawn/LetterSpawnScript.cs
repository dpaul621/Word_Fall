using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
public class LetterSpawnScript : MonoBehaviour
{
    public int currentLetterTier = -1;  
    private float scanTimer = 0f;
    public float scanInterval = 1f; 
    int currentVowelCount = 0;
    public int currentTotalLetterCount = 0;
    public List<LetterData> letterDataList;
    public List<float> spawnXValues;
    public float earlyGameTimer = 8f;
    float gameTimer;
    public float lowRangeSpawnInterval = 0.5f;
    public float highRangeSpawnInterval = 2.0f;
    public float chanceOfAdditionalLetter = 0.2f;
    public float letterMovementSpeed;
    public float currentLowRangeSpawnInterval;
    public float currentHighRangeSpawnInterval;
    public float currentChanceOfAdditionalLetter;
    public float currentLetterMovementSpeed;
    private Coroutine _spawnRoutine;
    private float? lastXValue = null;
    public float chancesOfABomb = 0.0f;
    public float chancesOfElectic = 0.0f;
    public float level;
    private Queue<char> plantedLetterQueue = new Queue<char>();
    public float vowelRatio;
    private List<string> longWords = new List<string>();
    public TextAsset wordListFile;
    private float wordInjectTimer = 0f;
    public float wordInjectInterval = 20f;
    public List<float> removeXValues;

    void OnDisable()
    {
        if (_spawnRoutine != null)
            StopCoroutine(_spawnRoutine);
    }
    IEnumerator NewLevelSetter()
    {
        yield return new WaitForEndOfFrame(); // wait until end of frame to ensure GameManager.Instance is ready
        DifficultySettings settings = GetDifficultySettings(GameManager.Instance.GMLevel);
        lowRangeSpawnInterval = settings.lowRangeSpawn;
        highRangeSpawnInterval = settings.highRangeSpawn;
        chanceOfAdditionalLetter = settings.chanceOfAdditionalLetter;
        letterMovementSpeed = settings.letterSpeed;
        chancesOfABomb = settings.chanceOfBomb;
        chancesOfElectic = settings.chanceOfElectric;

        currentLowRangeSpawnInterval = lowRangeSpawnInterval;
        currentHighRangeSpawnInterval = highRangeSpawnInterval;
        currentChanceOfAdditionalLetter = chanceOfAdditionalLetter;
        currentLetterMovementSpeed = letterMovementSpeed;
    }
    void Start()
    {
        _spawnRoutine = StartCoroutine(SpawnLoop());
        level = GameManager.Instance.GMLevel;
        StartCoroutine(NewLevelSetter());
        MakeSixLetterWords();
        if (GameManager.Instance.GMLevel < 25)
        {
            earlyGameTimer = 0f; 
        }
    }
    DifficultySettings GetDifficultySettings(int level)
    {
        float t = Mathf.Clamp01((level - 1f) / 99f); // Normalized value (0 to 1) across 100 levels

        int subLevel = ((level - 1) % 20) + 1;  // ✅ Valid outside return

        return new DifficultySettings
        {
            lowRangeSpawn = Mathf.Lerp(2.9f, 0.5f, t),
            highRangeSpawn = Mathf.Lerp(3.9f, 1f, t),
            chanceOfAdditionalLetter = Mathf.Lerp(0.015f, 0.6f, t),
            letterSpeed = Mathf.Lerp(-0.185f, -0.575f, t),

            chanceOfBomb = (subLevel >= 11 && subLevel <= 15) ? 0.15f :
                        (subLevel >= 18 && subLevel <= 20) ? 0.2f :
                        0f,

            chanceOfElectric = (subLevel == 16) ? 0.5f :
                            (subLevel == 17) ? 0.4f :
                            (subLevel >= 18) ? 0.35f :
                            0f
        };
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
        gameTimer += Time.deltaTime;
        int tier = GetLetterDifficultyTier(currentTotalLetterCount);
        if (tier != currentLetterTier && gameTimer >= earlyGameTimer && GameManager.Instance.GMLevel <= 40)
        {
            ApplyTierDifficulty(tier);
            currentLetterTier = tier;
        }

        scanTimer += Time.deltaTime;
        if (scanTimer >= scanInterval)
        {
            UpdateLetterStats();
            scanTimer = 0f;
        }
        InjectRandomWord();
    }
    public void RemoveFromXSpawnValues(float xValue)
    {
        removeXValues.Add(xValue);
    }
    public void AddToXSpawnValues(float xValue)
    {
        removeXValues.Remove(xValue);
    }
    int GetLetterDifficultyTier(int count)
    {
        if (count < 5) return 0;
        if (count < 10) return 1;  
        if (count < 20) return 2;  
        if (count < 30) return 3;  
        return 4;                  
    }
    void ApplyTierDifficulty(int tier)
    {
        switch (tier)
        {
            case 0: // Extreme hard
                currentLowRangeSpawnInterval = Mathf.Max(lowRangeSpawnInterval * 0.35f, 0.25f);
                currentHighRangeSpawnInterval = Mathf.Max(highRangeSpawnInterval * 0.35f, 0.35f);
                currentChanceOfAdditionalLetter = Mathf.Min(chanceOfAdditionalLetter * 2.5f, 0.95f);
                currentLetterMovementSpeed = letterMovementSpeed * 1.75f;
                Debug.Log("Difficulty TIER 0 applied (extreme hard)");
                break;

            case 1: // Extra hard
                currentLowRangeSpawnInterval = Mathf.Max(lowRangeSpawnInterval * 0.55f, 0.3f);
                currentHighRangeSpawnInterval = Mathf.Max(highRangeSpawnInterval * 0.55f, 0.5f);
                currentChanceOfAdditionalLetter = Mathf.Min(chanceOfAdditionalLetter * 2.0f, 0.9f);
                currentLetterMovementSpeed = letterMovementSpeed * 1.6f;
                Debug.Log("Difficulty TIER 1 applied (extra hard)");
                break;

            case 2: // Hard
                currentLowRangeSpawnInterval = Mathf.Max(lowRangeSpawnInterval * 0.7f, 0.4f);
                currentHighRangeSpawnInterval = Mathf.Max(highRangeSpawnInterval * 0.7f, 0.7f);
                currentChanceOfAdditionalLetter = Mathf.Min(chanceOfAdditionalLetter * 1.5f, 0.8f);
                currentLetterMovementSpeed = letterMovementSpeed * 1.35f;
                Debug.Log("Difficulty TIER 2 applied (hard)");
                break;

            case 3: // Medium
                currentLowRangeSpawnInterval = lowRangeSpawnInterval * 0.85f;
                currentHighRangeSpawnInterval = highRangeSpawnInterval * 0.85f;
                currentChanceOfAdditionalLetter = chanceOfAdditionalLetter * 1.2f;
                currentLetterMovementSpeed = letterMovementSpeed * 1.2f;
                Debug.Log("Difficulty TIER 3 applied (medium)");
                break;

            case 4: // Normal game
                currentLowRangeSpawnInterval = lowRangeSpawnInterval;
                currentHighRangeSpawnInterval = highRangeSpawnInterval;
                currentChanceOfAdditionalLetter = chanceOfAdditionalLetter;
                currentLetterMovementSpeed = letterMovementSpeed;
                Debug.Log("TIER 5: Using standard level difficulty.");
                break;
        }
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
        if (removeXValues != null && removeXValues.Count > 0)
        {
            availableX.RemoveAll(x => removeXValues.Contains(x));
            Debug.Log($"Available X values for spawning: {string.Join(", ", availableX)}");
        }

        //if random x has no available x values, return
        if (availableX.Count == 0)
        {
            Debug.LogWarning("No available X values for spawning letters.");
            return;
        }
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
            if (lastXValue.HasValue)
                secondAvailableX.Remove(lastXValue.Value);
            if (removeXValues != null && removeXValues.Count > 0)
            {
                secondAvailableX.RemoveAll(x => removeXValues.Contains(x));
                Debug.Log($"Available X values for spawning: {string.Join(", ", secondAvailableX)}");
            }
            if (secondAvailableX.Count == 0)
            {
                Debug.LogWarning("No available X values for spawning letters.");
                return;
            }
            
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
        if (removeXValues != null && removeXValues.Count > 0)
            shuffledX.RemoveAll(x => removeXValues.Contains(x));
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
