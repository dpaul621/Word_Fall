using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instructions : MonoBehaviour
{
    // this will be used only when gminstance level = 1
    // when it is active the game will be paused.
    //when you hit the continue button, it will go through the lsit of instructions one at a time, activating one and deactiviting the current, until we are on the last list item. then it will stop pausing hte game and deactivate the entire deal.
    public GameObject instructionsPanel;
    public List<GameObject> instructionsList;
    public int currentInstructionIndex = 0;

    public GameObject level1Instructions;
    public GameObject bombInstructions;
    public GameObject electricInstructions;

    void Start()
    {
        if (GameManager.Instance.GMLevel == 1)
        {
            instructionsPanel.SetActive(true);
            level1Instructions.SetActive(true);
            StartInstructions(level1Instructions);
            Debug.Log("Instructions started for level 1.");
        }
        if (GameManager.Instance.GMLevel == 11)
        {
            bombInstructions.SetActive(true);
            StartInstructions(bombInstructions);
            instructionsPanel.SetActive(true);
            Debug.Log("Instructions started for bomb level.");
        }
        if (GameManager.Instance.GMLevel == 30)
        {
            electricInstructions.SetActive(true);
            StartInstructions(electricInstructions);
            instructionsPanel.SetActive(true);
            Debug.Log("Instructions started for electric level.");
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
