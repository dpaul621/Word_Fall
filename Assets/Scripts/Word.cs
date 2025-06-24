using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Word : MonoBehaviour
{
    public float lettersCleared;
    public float lettersCLearedByWord;
    public float amountOfLettersToAdvance; 
    public List<GameObject> letterObjects = new List<GameObject>();
    public Image levelTrackerImage;
    public WordFade wordFadeScript;
    public float fillSpeed = 2f;
    public GameObject VictoryImage;
    void Update()
    {
        LetersClearedTracker();
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
                Debug.Log("changing to white, changing bool to false");
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
                letter.GetComponent<Letter>().SubmitParticlesFunction();
                lettersCLearedByWord++;
                Destroy(letter);
                if (index == count - 1)
                {
                    if (lettersCLearedByWord == 3)
                    {
                        float multiplyerBonus = lettersCLearedByWord * 0.8f;
                        lettersCleared += multiplyerBonus;
                    }
                    if (lettersCLearedByWord == 4 || lettersCLearedByWord == 5)
                    {
                        lettersCleared += lettersCLearedByWord * 1.2f;
                        wordFadeScript.Nice();
                    }
                    if (lettersCLearedByWord >= 6 && lettersCLearedByWord <= 8)
                    {
                        lettersCleared += lettersCLearedByWord * 1.3f;
                        wordFadeScript.Excellent();
                    }
                    if (lettersCLearedByWord >= 9 && lettersCLearedByWord <= 10)
                    {
                        lettersCleared += lettersCLearedByWord * 1.4f;
                        wordFadeScript.Superb();
                    }
                    if (lettersCLearedByWord >= 11)
                    {
                        lettersCleared += lettersCLearedByWord * 1.5f;
                        wordFadeScript.Wow();
                    }
                }
            }
            index++;
        }
        letterObjects.Clear();
    }
    void LetersClearedTracker()
    {
        if (lettersCleared >= amountOfLettersToAdvance)
        {
            StartCoroutine(LoadNextScene());
        }
        float targetFill = lettersCleared / amountOfLettersToAdvance;
        StartCoroutine(AnimateFill(targetFill));
    }
    private IEnumerator AnimateFill(float targetFill)
    {
        float startFill = levelTrackerImage.fillAmount;
        float elapsed = 0f;

        while (Mathf.Abs(levelTrackerImage.fillAmount - targetFill) > 0.001f)
        {
            elapsed += Time.deltaTime;
            levelTrackerImage.fillAmount = Mathf.Lerp(startFill, targetFill, elapsed * fillSpeed);
            yield return null;
        }

        levelTrackerImage.fillAmount = targetFill; 
    }

    IEnumerator LoadNextScene()
    {
        if (SceneManager.GetActiveScene().buildIndex < 10)
        {
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        if (SceneManager.GetActiveScene().buildIndex == 10)
        {
            VictoryImage.SetActive(true);
            //display image saying "You have completed the game, please wait for the next update"
            Debug.Log("You have completed the game, please wait for the next update");
            yield return new WaitForSeconds(5f);
            SceneManager.LoadScene(0); 
        }
    }
}
