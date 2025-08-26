using System.Collections;
using UnityEngine;

public class StompGuy : MonoBehaviour
{
    [Header("Animator & Sprite")]
    public Animator stompGuyAnimator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Death Loop Settings")]
    [SerializeField] private string deathStateName = "DeathStomp"; // state name
    [SerializeField] private int deathLoopsToPlay = 4;
    private int deathLoopsDone = 0;
    private bool deathStarted = false;   // <---- NEW

    [Header("Other")]
    public float lowRangeStompTimer = 10f;
    public float highRangeStompTimer = 15f;
    LetterSpawnScript letterSpawnScript;
    public bool keepStomping = true;
    Word word;

    void Awake()
    {
        if (!stompGuyAnimator) stompGuyAnimator = GetComponent<Animator>();
        if (!spriteRenderer) spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        word = FindObjectOfType<Word>();
        if (GameManager.Instance.levelPercentage == 0.25f) { lowRangeStompTimer = 20f; highRangeStompTimer = 25f; }
        if (GameManager.Instance.levelPercentage == 0.50f) { lowRangeStompTimer = 15f; highRangeStompTimer = 20f; }
        if (GameManager.Instance.levelPercentage == 0.75f) { lowRangeStompTimer = 10f; highRangeStompTimer = 15f; }
        if (GameManager.Instance.levelPercentage == 1.00f) { lowRangeStompTimer = 10f; highRangeStompTimer = 15f; }
        letterSpawnScript = FindObjectOfType<LetterSpawnScript>();
        StartCoroutine(Stomp());
    }

    void Update()
    {
        // Start death ONCE
        if (word.isLevelComplete && !deathStarted)
        {
            deathStarted = true;          // <---- prevent re-Play every frame
            deathLoopsDone = 0;
            spriteRenderer.flipX = false;
//            Debug.Log("StompGuy is dead!");
            StartCoroutine(FlashLightRedWhiteLoop());
            // play first loop from t=0 (string is fine)
            stompGuyAnimator.Play(deathStateName, 0, 0f);
        }
    }
    private IEnumerator FlashLightRedWhiteLoop(float halfPeriod = 0.15f)
    {
        var l = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
        if (!l) yield break;

        while (true)
        {
            for (float t = 0f; t < 1f; t += Time.deltaTime / halfPeriod) { l.color = Color.Lerp(Color.white, Color.red, t); yield return null; }
            for (float t = 0f; t < 1f; t += Time.deltaTime / halfPeriod) { l.color = Color.Lerp(Color.red, Color.white, t); yield return null; }
        }
    }



    // Animation Event at END of DeathStomp clip must call this
    public void OnDeathLoopEnd()
    {
        deathLoopsDone++;

        if (deathLoopsDone < deathLoopsToPlay)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
//            Debug.Log($"Death loop {deathLoopsDone} of {deathLoopsToPlay}");
            stompGuyAnimator.Play(deathStateName, 0, 0f); // restart same state
        }
        else
        {
            StartCoroutine(BeginFall());          // (or do nothing yet)
        }
    }

    IEnumerator BeginFall()
    {
        // add/enable a Rigidbody2D if needed
        var rb = GetComponent<Rigidbody2D>();
        if (!rb) rb = gameObject.AddComponent<Rigidbody2D>();

        rb.simulated = true;
        rb.gravityScale = 1f;      // how fast to fall
        rb.freezeRotation = false; // allow spinning

        rb.velocity = new Vector2(0f, 5f);  // small pop up
        rb.angularVelocity = 320f;          // spin speed (deg/sec)
        yield return new WaitForSeconds(0.5f);
        AudioManager.Instance.PlaySFX(SFXType.fallingBossSound, 0.2f);
    }

    public void StompNoise() { AudioManager.Instance.PlaySFX(SFXType.bossStomps, 1f); ScreenShake.Instance.Shake(0.2f, 0.1f);}

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

    public void StompGuyPicksUpFoot() { AudioManager.Instance.PlaySFX(SFXType.bossPicksUpFoot, 1f); }
    public void StompSpawnsLettersInAnimator()
    {
        ScreenShake.Instance.Shake(0.2f, 0.1f);
        letterSpawnScript.StompSpawn();
        AudioManager.Instance.PlaySFX(SFXType.bossStomps, 1f);
    }
}
