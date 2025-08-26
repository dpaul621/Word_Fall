using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class Word : MonoBehaviour
{
    public float lettersCleared;
    public float lettersCLearedByWord;
    public float amountOfLettersToAdvance;
    public List<GameObject> letterObjects = new List<GameObject>();
    public List<GameObject> allLetters = new List<GameObject>();
    public Image levelTrackerImage;
    public WordFade wordFadeScript;
    public float fillSpeed = 2f;
    public GameObject VictoryImage;
    public GameObject LevelCompleteImage;
    public float multiplyerBonus;
    public float bonusPercentage = 0;
    public ScoreAddedImage scoreAddedImage;
    public float explosionAtEndSpeed = 0.1f; 
    public bool isLevelComplete = false;
    public float penaltyForShortWord = -0.1f; // -10% penalty

    void Start()
    {
        scoreAddedImage = GameObject.FindObjectOfType<ScoreAddedImage>();
        //access game manager script level
        amountOfLettersToAdvance = CalculateLettersToAdvance(GameManager.Instance.GMLevel);
    }
    float CalculateLettersToAdvance(int level)
    {
        float letters = 9.896f + Mathf.Log(level + 1.040f) * 19.784f + Mathf.Pow(level, 0.844f);
        return Mathf.Round(letters);
    }
    void Update()
    {
        LetersClearedTracker();
        if (levelTrackerImage != null)
        {
            float targetFill = lettersCleared / amountOfLettersToAdvance;
            StartCoroutine(AnimateFill(targetFill));
        }
        else
        {
            Debug.LogWarning("Level Tracker Image is not assigned.");
        }
    }
public void AddLetter(GameObject letter)
{
    if (letter != null && !letterObjects.Contains(letter))
    {
        // Decide which sound to play based on how many letters are already in the list
        int currentCount = letterObjects.Count; // 0-based before adding

        // Clamp to available sounds (1–8)
        int soundIndex = Mathf.Clamp(currentCount + 1, 1, 6);

        // Convert to enum name dynamically (selectSound1..selectSound8)
        SFXType soundType = (SFXType)System.Enum.Parse(typeof(SFXType), $"selectSound{soundIndex}");

        // Play the sound
        AudioManager.Instance.PlaySFX(soundType);

        // Finally, add the letter
        letterObjects.Add(letter);
    }
    else
    {
        // Deselect if already in list
        if (letter != null && letterObjects.IndexOf(letter) >= 0)
        {
            letter.GetComponent<SpriteRenderer>().color = Color.white;
            Letter letterScript = letter.GetComponent<Letter>();
            letterScript.inputFieldText.text = letterScript.inputFieldText.text.Remove(letterObjects.IndexOf(letter), 1);
            letterScript.selected = false;
            letterObjects.Remove(letter);
        }
        else
        {
            Debug.LogWarning("Attempted to add a null letter.");
        }
    }
}
    public void RemoveLastLetter()
    {
        if (letterObjects.Count > 0)
        {
            GameObject lastLetter = letterObjects[letterObjects.Count - 1];
            if (lastLetter != null)
            {
                lastLetter.GetComponent<SpriteRenderer>().color = Color.white;
                Letter letterScript = lastLetter.GetComponent<Letter>();
                letterScript.selected = false;
            }
        }
        if (letterObjects.Count > 0)
        {
            letterObjects.RemoveAt(letterObjects.Count - 1);
        }
        else
        {
            Debug.LogWarning("Attempted to remove a letter from an empty list.");
        }
    }
    public void RemoveAllLetters()
    {
        if (letterObjects.Count > 0)
        {
            foreach (GameObject letter in letterObjects)
            {
                if (letter != null)
                {
                    letter.GetComponent<SpriteRenderer>().color = Color.white;
                    Letter letterScript = letter.GetComponent<Letter>();
                    letterScript.selected = false;
                }
            }
            letterObjects.Clear();
        }
        else
        {
            Debug.LogWarning("Attempted to remove letters from an empty list.");
        }
    }
    public void ClearLetters()
    {
        lettersCLearedByWord = 0;
        int count = letterObjects.Count;
        float index = 0;
        foreach (GameObject letter in letterObjects)
        {
            if (letter != null)
            {
                lettersCLearedByWord++;
                Letter letterScript = letter.GetComponent<Letter>();
                if (letterScript.letterIsBomb)
                {
                    letterScript.TriggerBomb();
                }
                else if (letterScript.isElectric)
                {
                    letterScript.TriggerElectricEffect();
                }
                else if (letterScript.selected)
                {
                    letterScript.Death();
                    Destroy(letterScript.selectedEffect);
                    letterScript.selected = false;
                }
                else
                {
                    Debug.Log("Letter cleared: " + letterScript.gameObject.name);
                    letterScript.animator.SetTrigger("smallDeath");
                    Destroy(letterScript.selectedEffect);
                }
                if (index == count - 1)
                {
                    if (lettersCLearedByWord == 3)
                    {
                        bonusPercentage = 0f;
                        Debug.Log("Short word cleared, applying penalty: " + bonusPercentage);
                        lettersCleared += lettersCLearedByWord;
                    }
                    if (lettersCLearedByWord == 4)
                    {
                        bonusPercentage = 0.3f; // 30% bonus
                        multiplyerBonus = lettersCLearedByWord * (1 + bonusPercentage);
                        lettersCleared += multiplyerBonus;
                        wordFadeScript.Nice();
                    }
                    if (lettersCLearedByWord == 5)
                    {
                        bonusPercentage = 0.5f; // 50% bonus
                        multiplyerBonus = lettersCLearedByWord * (1 + bonusPercentage);
                        lettersCleared += multiplyerBonus;
                        wordFadeScript.Nice();
                    }
                    if (lettersCLearedByWord >= 6 && lettersCLearedByWord <= 7)
                    {
                        bonusPercentage = 0.75f; // 75% bonus
                        multiplyerBonus = lettersCLearedByWord * (1 + bonusPercentage);
                        lettersCleared += multiplyerBonus;
                        wordFadeScript.Excellent();
                    }
                    if (lettersCLearedByWord >= 8 && lettersCLearedByWord <= 10)
                    {
                        bonusPercentage = 1f; // 100% bonus
                        multiplyerBonus = lettersCLearedByWord * (1 + bonusPercentage);
                        lettersCleared += multiplyerBonus;
                        wordFadeScript.Superb();
                    }
                    if (lettersCLearedByWord >= 11)
                    {
                        bonusPercentage = 3f; // 300% bonus
                        multiplyerBonus = lettersCLearedByWord * (1 + bonusPercentage);
                        lettersCleared += multiplyerBonus;
                        wordFadeScript.Wow();
                    }
                    if (lettersCLearedByWord > 3)
                    {
                        scoreAddedImage.ShowScoreAddedMessage();
                    }
                }
            }
            index++;
        }
        //
        
        letterObjects.Clear();
    }
    void LetersClearedTracker()
    {
        if (lettersCleared >= amountOfLettersToAdvance && !isLevelComplete)
        {
            StompGuy stompGuy = FindObjectOfType<StompGuy>();
            if (stompGuy != null)
            {
                stompGuy.keepStomping = false;
            }
            LetterSpawnScript letterSpawnScript = FindObjectOfType<LetterSpawnScript>();
            if (letterSpawnScript != null)
            {
                letterSpawnScript.enabled = false;
            }
            isLevelComplete = true;
            StartCoroutine(LoadNextScene());
        }
    }
    private IEnumerator AnimateFill(float targetFill)
    {
        float startFill = levelTrackerImage.fillAmount;
        float timeout = 3f;
        float elapsed = 0f;

        while (Mathf.Abs(levelTrackerImage.fillAmount - targetFill) > 0.001f && elapsed < timeout)
        {
            elapsed += Time.deltaTime;
            levelTrackerImage.fillAmount = Mathf.Lerp(startFill, targetFill, Mathf.Clamp01(elapsed * fillSpeed));
            yield return null;
        }

        levelTrackerImage.fillAmount = targetFill;
    }

    IEnumerator LoadNextScene()
    {
        StartCoroutine(TriggerSmallDeathsCoroutine());
        yield return new WaitForSeconds(0.75f);
        AudioManager.Instance.PlaySFX(SFXType.levelComplete, 1f);
        if (GameManager.Instance.GMLevel >= GameManager.Instance.MaxLevel)
        {
            Debug.Log("You have completed all levels! Game Over.");
            VictoryImage.SetActive(true);
            yield return new WaitForSeconds(10f);
        }
        else
        {
            Debug.Log("level up! Loading next scene...");
            LevelCompleteImage.SetActive(true);
            GameManager.Instance.GMLevel++;
            yield return new WaitForSeconds(4f);
            SceneManager.LoadScene(1);
            SaveProgress();
        }
    }
    void SaveProgress()
    {
        int levelInt = GameManager.Instance.GMLevel;
        if (GameManager.Instance.oneDifficultyMode)
        {
            GameManager.Instance.SetOneDifficultyModeLevel(levelInt);
        }
        else
        {
            GameManager.Instance.SetLevelForCurrentDifficulty(levelInt);
        }

    }


    IEnumerator TriggerSmallDeathsCoroutine()
    {
        List<GameObject> allLetters = new List<GameObject>(GameObject.FindGameObjectsWithTag("FloatingLetter"));
        //also add letters with tag 'letter on grid"
        allLetters.AddRange(GameObject.FindGameObjectsWithTag("LetterOnGrid"));
        foreach (GameObject letter in allLetters)
        {
            if (letter != null)
            {
                HapticFeedback.Trigger();
                Letter letterScript = letter.GetComponent<Letter>();
                if (letterScript != null && letterScript.animator != null)
                {
                    letterScript.Death();
                    Destroy(letterScript.ElectricEffect);
                }
            }
            yield return new WaitForSeconds(explosionAtEndSpeed); // Adjust the delay as needed
        }
    }
}
