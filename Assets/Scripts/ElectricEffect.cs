using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricEffect : MonoBehaviour
{
    GameObject parentLetter;
    public bool isDead = false;

    void Start()
    {
        parentLetter = transform.parent.gameObject;
    }
    //find all game objects with the tag "ElectricEffect" 
    void Update()
    {
        //find all game objects with the tag "ElectricEffect"
        /*GameObject[] electricEffects = GameObject.FindGameObjectsWithTag("ElectricEffect");
        foreach(GameObject effect in electricEffects)
        {
            //if the effect is not the parent letter, destroy it
            parentLetter.GetComponent<Letter>().TriggerElectricEffect();
        }*/
    }

    public void TriggerElectricEffect()
    {
        if (parentLetter != null)
        {
            Letter letterScript = parentLetter.GetComponent<Letter>();
            if (letterScript != null)
            {
                letterScript.TriggerElectricEffect();
            }
            else
            {
                Debug.LogWarning("Letter script not found on parent letter.");
            }
        }
        else
        {
            Debug.LogWarning("Parent letter is null, cannot trigger electric effect.");
        }
    }

    public void TurnOffParentSpriteRenderer()
    {
        isDead = true;
        if (parentLetter != null)
        {
            SpriteRenderer spriteRenderer = parentLetter.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = false;
            }
            else
            {
                Debug.LogWarning("SpriteRenderer not found on parent letter.");
            }
        }
        else
        {
            Debug.LogWarning("Parent letter is null, cannot turn off sprite renderer.");
        }
    }

    public void DestroyParentLetter()
    {
        if (parentLetter != null)
        {
            Destroy(parentLetter);
            Word wordScript = FindObjectOfType<Word>();
            if (wordScript != null)
            {
                wordScript.lettersCleared += 0.5f; // Increment letters cleared by
            }
        }
        else
        {
            Debug.LogWarning("Parent letter is null, cannot destroy effect.");
        }
    }
    
    public void ElectricBonusScore()
    {
        GameObject bonus = Resources.Load<GameObject>("ElectricBonusScore");
        Instantiate(bonus, transform.position, Quaternion.identity);
    }

}
