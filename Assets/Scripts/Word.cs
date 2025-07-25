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

    private PlayerProgressManager progressManager;
    public bool isLevelComplete = false;
    public float penaltyForShortWord = -0.1f; // -10% penalty

    void Start()
    {
        progressManager = FindObjectOfType<PlayerProgressManager>();
        scoreAddedImage = GameObject.FindObjectOfType<ScoreAddedImage>();
        //access game manager script level
        if (GameManager.Instance != null)
        {
            //if game manager level is between 0 and 12
            amountOfLettersToAdvance = 20 + (GameManager.Instance.GMLevel * 5f); // Adjust this formula as needed
            //if this this makes 80, then only add two per level
            if (amountOfLettersToAdvance > 80f)
            {
                amountOfLettersToAdvance = 80f + ((GameManager.Instance.GMLevel - 12) * 2f);
            }
        }
        else
        {
            Debug.LogWarning("GameManager instance not found. Using default amount of letters to advance.");
            amountOfLettersToAdvance = 100f; // Default value if GameManager is not found
        }

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
            letterObjects.Add(letter);
        }
        else
        {
            //access the letter gamobjects componoent Letter, set the color to white, and remove that character from the list of characters in the input field. i will need to access the iinputfieldtexts list of characters, the index of that will corelate with the index from the list of letter gameobjects. use this index to delete the character
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
                    letterScript.animator.SetTrigger("smallDeath");
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
                        /*if (penaltyForShortWord < -0.89)
                        {
                            penaltyForShortWord = -0.9f;
                        }*/
                        //Debug.Log("Short word cleared, applying penalty: " + penaltyForShortWord);
                        //bonusPercentage = penaltyForShortWord; 
                        //multiplyerBonus = lettersCLearedByWord * (1 + bonusPercentage);
                        bonusPercentage = 0f; // 10% penalty
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
            //deactive letter spawn
            LetterSpawnScript letterSpawnScript = FindObjectOfType<LetterSpawnScript>();
            if (letterSpawnScript != null)
            {
                letterSpawnScript.enabled = false;
            }
            isLevelComplete = true;
            if (GameManager.Instance != null)
            {
                StartCoroutine(LoadNextScene());
            }
            else
            {
                Debug.LogWarning("GameManager instance not found. Cannot increase level.");
            }
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
        SaveProgress();
        if (GameManager.Instance.GMLevel >= 40)
        {
            Debug.Log("You have completed all levels! Game Over.");
            VictoryImage.SetActive(true);
            yield return new WaitForSeconds(2f);
        }
        else
        {
            Debug.Log("level up! Loading next scene...");
            LevelCompleteImage.SetActive(true);
            GameManager.Instance.GMLevel++;
            yield return new WaitForSeconds(5f);

        }
    }
    void SaveProgress()
    {
        int levelInt = (int)GameManager.Instance.GMLevel + 1; 

        if (GameManager.Instance.Difficulty == 1)
        {
            progressManager.SaveEasyProgress(levelInt);
        }
        else if (GameManager.Instance.Difficulty == 2)
        {
            progressManager.SaveMediumProgress(levelInt);
        }
        else if (GameManager.Instance.Difficulty == 3)
        {
            progressManager.SaveHardProgress(levelInt);
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
                    letterScript.animator.SetTrigger("smallDeath");
                    Destroy(letterScript.ElectricEffect);
                }
            }
            yield return new WaitForSeconds(explosionAtEndSpeed); // Adjust the delay as needed
        }
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
