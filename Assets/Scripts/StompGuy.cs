using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StompGuy : MonoBehaviour
{
    //timed script that makes guy stomp every x seconds, and causes a stomp spawn in the letter spawn script
    public Animator stompGuyAnimator;
    public float lowRangeStompTimer = 10f;
    public float highRangeStompTimer = 15f;
    public float totalLevels = 40f;
    LetterSpawnScript letterSpawnScript;
    void Start()
    {
        if(GameManager.Instance.GMLevel / totalLevels == 0.25f)
        {
            lowRangeStompTimer = 20f;
            highRangeStompTimer = 25f;
        }
        if(GameManager.Instance.GMLevel / totalLevels == 0.50f)
        {
            lowRangeStompTimer = 15f;
            highRangeStompTimer = 20f;
        }
        if(GameManager.Instance.GMLevel / totalLevels == 0.75f)
        {
            lowRangeStompTimer = 10f;
            highRangeStompTimer = 15f;
        }
        if(GameManager.Instance.GMLevel / totalLevels == 1.00f)
        {
            lowRangeStompTimer = 10f;
            highRangeStompTimer = 15f;
        }
        letterSpawnScript = FindObjectOfType<LetterSpawnScript>();
        StartCoroutine(Stomp());
    }

    IEnumerator Stomp()
    {
        while (true)
        {
            stompGuyAnimator.SetTrigger("Stomp");
            
            //flip x scale
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            float randomStompInterval = Random.Range(lowRangeStompTimer, highRangeStompTimer);
            yield return new WaitForSeconds(randomStompInterval);
        }
    }

    public void StompSpawnsLettersInAnimator()
    {
        //add screen shake
        ScreenShake.Instance.Shake(0.2f, 0.1f); // (duration, magnitude)
        letterSpawnScript.StompSpawn();
    }
}
