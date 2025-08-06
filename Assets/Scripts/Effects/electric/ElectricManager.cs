using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ElectricManager : MonoBehaviour
{
    bool isChainDestructionTriggered = false;
    float delay = 0.2f; 

    void Update()
    {
        //find all game objects with the tag "ElectricEffect"
        GameObject[] electricEffects = GameObject.FindGameObjectsWithTag("ElectricEffect");
        foreach (GameObject effect in electricEffects)
        {
            if (effect.GetComponent<ElectricEffect>().isDead)
            {
                isChainDestructionTriggered = true;
            }
        }

        if (isChainDestructionTriggered)
        {
            TriggerChainDestruction();
            isChainDestructionTriggered = false; // Reset the trigger after processing
        }
    }

    void TriggerChainDestruction()
    {
        GameObject[] electricEffects = GameObject.FindGameObjectsWithTag("ElectricEffect");
        foreach (GameObject effect in electricEffects)
        {

        }
        for(int i = 0; i < electricEffects.Length; i++)
        {
            StartCoroutine(DelayedDestructionElectric(delay * i, electricEffects[i]));
        }
    }

    IEnumerator DelayedDestructionElectric(float delayTime, GameObject effectLetter)
    {
        yield return new WaitForSeconds(delayTime);
        if (effectLetter == null)
        {

            yield break; // Exit the coroutine if the effect letter is null
        }
        else
        {
            effectLetter.GetComponent<ElectricEffect>().TriggerElectricEffect();
        }

    }
}
