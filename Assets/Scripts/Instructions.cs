using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instructions : MonoBehaviour
{
    public GameObject instructionsPanel;
    public List<GameObject> instructionsList;
    public int currentInstructionIndex = 0;

    public GameObject level1Instructions;
    public GameObject bombInstructions;
    public GameObject electricInstructions;
    public GameObject level41Instructions;
    public GameObject level81Instructions;
    public GameObject level121Instructions;
    public GameObject level161Instructions;

    void Start()
    {
        if (GameManager.Instance.GMLevel == 11)
        {
            bombInstructions.SetActive(true);
            StartInstructions(bombInstructions);
            instructionsPanel.SetActive(true);
            Debug.Log("Instructions started for bomb level.");
        }
        if (GameManager.Instance.GMLevel == 16)
        {
            electricInstructions.SetActive(true);
            StartInstructions(electricInstructions);
            instructionsPanel.SetActive(true);
            Debug.Log("Instructions started for electric level.");
        }
        if (GameManager.Instance.GMLevel == 41)
        {
            instructionsPanel.SetActive(true);
            level41Instructions.SetActive(true);
            StartInstructions(level41Instructions);
            Debug.Log("Instructions started for level 41.");
        }
        if (GameManager.Instance.GMLevel == 81)
        {
            instructionsPanel.SetActive(true);
            level81Instructions.SetActive(true);
            StartInstructions(level81Instructions);
            Debug.Log("Instructions started for level 81.");
        }
        if (GameManager.Instance.GMLevel == 121)
        {
            instructionsPanel.SetActive(true);
            level121Instructions.SetActive(true);
            StartInstructions(level121Instructions);
            Debug.Log("Instructions started for level 121.");
        }
        if (GameManager.Instance.GMLevel == 161)
        {
            instructionsPanel.SetActive(true);
            level161Instructions.SetActive(true);
            StartInstructions(level161Instructions);
            Debug.Log("Instructions started for level 161.");
        }
    }

    public void StartInstructions(GameObject instructions)
    {
        instructionsList.Clear();
        foreach (Transform child in instructions.transform)
        {
            instructionsList.Add(child.gameObject);
        }
        currentInstructionIndex = 0;
        instructionsList[currentInstructionIndex].SetActive(true);
        ShowCurrentInstruction(instructions);
    }

    public void ShowCurrentInstruction(GameObject instructions)
    {

        instructionsPanel.SetActive(true);
        Time.timeScale = 0f; // Pause the game
    }

    public void ContinueInstructions()
    {
        HapticFeedback.Trigger();
        AudioManager.Instance.PlaySFX(SFXType.buttonClick, 1f);
        //when clicked deactivate curent instructions (list item currentInstructionIndex) and activate the next on the list
        if (currentInstructionIndex < instructionsList.Count)
        {
            instructionsList[currentInstructionIndex].SetActive(false);
            currentInstructionIndex++;

            if (currentInstructionIndex < instructionsList.Count)
            {
                currentInstructionIndex = currentInstructionIndex++;
                instructionsList[currentInstructionIndex].SetActive(true);

            }
            else
            {
                instructionsPanel.SetActive(false);
                Time.timeScale = 1f;
            }
        }
    }
}
