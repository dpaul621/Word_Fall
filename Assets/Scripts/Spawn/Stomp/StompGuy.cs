using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StompGuy : MonoBehaviour
{
    public Animator stompGuyAnimator;
    public float lowRangeStompTimer = 10f;
    public float highRangeStompTimer = 15f;
    LetterSpawnScript letterSpawnScript;
    public bool keepStomping = true;
    void Start()
    {
        if(GameManager.Instance.levelPercentage == 0.25f)
        {
            lowRangeStompTimer = 20f;
            highRangeStompTimer = 25f;
        }
        if(GameManager.Instance.levelPercentage == 0.50f)
        {
            lowRangeStompTimer = 15f;
            highRangeStompTimer = 20f;
        }
        if(GameManager.Instance.levelPercentage == 0.75f)
        {
            lowRangeStompTimer = 10f;
            highRangeStompTimer = 15f;
        }
        if(GameManager.Instance.levelPercentage == 1.00f)
        {
            lowRangeStompTimer = 10f;
            highRangeStompTimer = 15f;
        }
        letterSpawnScript = FindObjectOfType<LetterSpawnScript>();
        StartCoroutine(Stomp());
    }

    IEnumerator Stomp()
    {
        while (keepStomping)
        {
            stompGuyAnimator.SetTrigger("Stomp");
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            float randomStompInterval = Random.Range(lowRangeStompTimer, highRangeStompTimer);
            yield return new WaitForSeconds(randomStompInterval);
        }
    }
    public void StompGuyPicksUpFoot()
    {
        AudioManager.Instance.PlaySFX(SFXType.bossPicksUpFoot, 1f);
    }

    public void StompSpawnsLettersInAnimator()
    {
        //add screen shake
        ScreenShake.Instance.Shake(0.2f, 0.1f); // (duration, magnitude)
        letterSpawnScript.StompSpawn();
        AudioManager.Instance.PlaySFX(SFXType.bossStomps, 1f);
    }
}
