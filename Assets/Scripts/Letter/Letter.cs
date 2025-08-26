using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Rendering.Universal;

public class Letter : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    public TMP_InputField inputFieldText;
    public Word wordScript;
    public LetterSpawnScript letterSpawnScript;
    public bool letterIsUsable = true;
    public float rayDistance;
    public float rayCastOffsetSoItDoesntHitItself = 0.5f;
    public bool isInterrupted = false;
    public bool selected = false;
    public GameObject selectedEffect;
    public GameObject selectedEffectPrefab;
    public GameObject ElectricEffect;
    public GameObject ElectricEffectPrefab;
    public Animator ElectricAnimator;
    public float chanceOfElectricEffect = 0.0f;
    public bool isElectric = false;
    public bool letterIsBomb = false;
    public GameObject BombPrefab;
    public Animator animator;
    public float bombDelay = 0.15f;
    public Pause pauseScript;
    public float spawnXLocation = 0f;
    public bool destroyedByExplosion = false;
    public GameObject letterFlashWhenClickedPrefab;
    public GameObject scoreAddedImage;
    public GameObject bombBonusPrefab;
    public GameObject electricBonusPrefab;
    public bool isVowel;

    public void Setup(LetterData data)
    {
        isVowel = data.isVowel;
    }
    private void Awake()
    {
        GameObject inputObj = GameObject.FindGameObjectWithTag("TextInputField");
        if (inputObj != null)
        {
            inputFieldText = inputObj.GetComponent<TMP_InputField>();
        }
        else
        {
            Debug.LogError("No GameObject with tag 'TextInputField' found!");
        }
        GameObject wordObj = GameObject.FindGameObjectWithTag("Word");
        if (wordObj != null)
        {
            wordScript = wordObj.GetComponent<Word>();
        }
        else
        {
            Debug.LogError("No GameObject with tag 'Word' found!");
        }
        _rigidbody2D = GetComponent<Rigidbody2D>();

        letterSpawnScript = FindObjectOfType<LetterSpawnScript>();

    }
    private void Start()
    {
        pauseScript = GameObject.FindGameObjectWithTag("PauseButton").GetComponent<Pause>();
        if (TutorialManager.Instance == null || TutorialManager.Instance.isTutorialActive == false)
        {
            //Debug.Log("Setting initial velocity for letter." + "tutman istut active" + (TutorialManager.Instance.isTutorialActive));
            _rigidbody2D.velocity = new Vector2(0, letterSpawnScript.currentLetterMovementSpeed);
        }
        else
        {
            Debug.Log("TutorialManager is active, not setting initial velocity.");
        }
        Instantiate(selectedEffectPrefab, transform.position, Quaternion.identity, transform);
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found on the letter object.");
        }
        char firstLetter = gameObject.name[0];
        animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>($"Animations/{firstLetter}");
        animator.SetBool("isSizzling", false);
        if (letterIsBomb == true)
        {
            animator.SetBool("isSizzling", false);
            StartCoroutine(WaitAndSetSizzling());
        }
        else
        {
            if (isElectric)
            {
                ElectricEffect = Instantiate(ElectricEffectPrefab, transform.position, Quaternion.identity, transform);
                ElectricAnimator = ElectricEffect.GetComponent<Animator>();
            }
        }
        StartCoroutine(TurnOffLights());
    }
    IEnumerator TurnOffLights()
    {
        yield return new WaitForEndOfFrame();
        LightController lightController = FindObjectOfType<LightController>();
        GameObject lightTest = transform.Find("light test").gameObject;
        if (lightController.dayNightReference <= 0.35f)
        {
            lightTest.SetActive(false);
        }

    }
    public void TriggerElectricEffect()
    {
        if (ElectricAnimator != null)
        {
            ElectricAnimator.SetTrigger("ElectricExplosion");
            Destroy(selectedEffect);
        }
        else
        {
            Debug.LogWarning("ElectricAnimator is null, cannot trigger electric effect.");
        }
    }
    IEnumerator WaitAndSetSizzling()
    {
        yield return new WaitForEndOfFrame();
        float now = Time.time;
        float nextSecond = Mathf.Ceil(now);
        yield return new WaitForSeconds(nextSecond - now);
        animator.SetBool("isSizzling", true);
    }
    void DeactivateLightIfElectric()
    {
        if (ElectricEffect != null)
        {
            Transform lightTest = gameObject.transform.Find("light test");
            if (lightTest != null)
            {
                lightTest.gameObject.SetActive(false);
            }
        }
    }
    private void Update()
    {
        DeactivateLightIfElectric();
        CheckIfLetterAboveAndBelow();
        if (pauseScript.isPaused == false)
        {
            TouchUpdate();
        }
        SelectedeffectActivator();
        CorrectToAllowedXValues();
    }

    void CorrectToAllowedXValues()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);

        float rawX = Mathf.Round(transform.position.x * 2f) / 2f;

        if (Mathf.Approximately(rawX, 0f))
        {
            rawX = (transform.position.x < 0) ? -0.5f : 0.5f;
        }
        else if (Mathf.Approximately(rawX % 1f, 0f))
        {
            rawX += (rawX < 0) ? -0.5f : 0.5f;
        }

        rawX = Mathf.Clamp(rawX, -2.5f, 2.5f);

        spawnXLocation = rawX;
        transform.position = new Vector3(spawnXLocation, transform.position.y, transform.position.z);

    }
    void CheckIfLetterAboveAndBelow()
    {
        Vector2 rayOriginAbove = new Vector2(transform.position.x, transform.position.y + rayCastOffsetSoItDoesntHitItself);
        Vector2 rayOriginBelow = new Vector2(transform.position.x, transform.position.y - rayCastOffsetSoItDoesntHitItself);
        Vector2 rayDirection = Vector2.up;

        Debug.DrawRay(rayOriginAbove, rayDirection * rayDistance, Color.green);
        Debug.DrawRay(rayOriginBelow, Vector2.down * rayDistance, Color.red);

        RaycastHit2D hitAbove = Physics2D.Raycast(rayOriginAbove, rayDirection, rayDistance);
        RaycastHit2D hitBelow = Physics2D.Raycast(rayOriginBelow, Vector2.down, rayDistance);

        if (hitBelow.collider != null)
        {
            if (hitBelow.collider.CompareTag("BottomGrid") || hitBelow.collider.CompareTag("LetterOnGrid") && _rigidbody2D.bodyType == RigidbodyType2D.Dynamic)
            {
                _rigidbody2D.bodyType = RigidbodyType2D.Static;
            }
        }
        if (hitAbove.collider != null && hitBelow.collider != null)
        {
            if (hitAbove.collider.CompareTag("LetterOnGrid") || (hitAbove.collider.CompareTag("FloatingLetter") && hitBelow.collider.CompareTag("LetterOnGrid")) || hitBelow.collider.CompareTag("BottomGrid"))
            {
                _rigidbody2D.bodyType = RigidbodyType2D.Static;
                StartCoroutine(LetterUnusable());
                gameObject.tag = "LetterOnGrid";
            }
        }

        if (hitBelow.collider == null && _rigidbody2D.bodyType == RigidbodyType2D.Static)
        {
            _rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            _rigidbody2D.velocity = new Vector2(0, letterSpawnScript.currentLetterMovementSpeed);
        }

        if (hitAbove.collider == null && _rigidbody2D.bodyType == RigidbodyType2D.Static)
        {
            StartCoroutine(LetterUsableAgain());
        }
    }
    private void TouchUpdate()
    {
        if (Time.timeScale == 0f) return;
        // After confirming this tap hit a letter tile



    #if UNITY_IOS || UNITY_ANDROID
        // Mobile: use touch
        if (Input.touchCount > 0)
        {
            var t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Began)
                HandleTouchInput(t.position);
        }
    #else
        // Editor + Standalone (macOS/Windows/Linux): use mouse
        if (Input.GetMouseButtonDown(0))
            HandleTouchInput(Input.mousePosition);
    #endif
    }
    private void HandleTouchInput(Vector2 screenPosition)
    {
        var cam = Camera.main;
        if (!cam) return;

        // Convert screen -> world (for 2D, z can be 0)
        Vector3 world = cam.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, 0f));
        Vector2 p2D = new Vector2(world.x, world.y);

        // Hit test a Collider2D at that point
        var hit = Physics2D.OverlapPoint(p2D);
        if (hit == null) return;

        // If this script is on the letter object you care about:
        if (hit.transform == transform)  // or: if (hit.GetComponentInParent<Letter>() == GetComponent<Letter>())
        {
            // your existing selection logic
            LetterSelected(hit.gameObject);

            // tell the tutorial manager this specific world object was tapped
            TutorialManager.Instance?.NotifyWorldTap(hit.transform);
        }
    }
    void SelectedeffectActivator()
    {
        selectedEffect = transform.Find("Letter Effect(Clone)")?.gameObject;
        if (selected)
        {
            if (selectedEffect != null)
            {
                selectedEffect.SetActive(true);
            }
        }
        if (!selected)
        {
            if (selectedEffect != null)
            {
                selectedEffect.SetActive(false);
            }
        }

    }
    void LetterSelected(GameObject objectClicked)
    {
        if (letterIsUsable)
        {
            if (!selected)
            {
                
                CreateLetterFlash();
            }
            else
            {
                AudioManager.Instance.PlaySFX(SFXType.letterDeselect, 1f);
            }
            HapticFeedback.Trigger();
            isInterrupted = true;
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                selected = true;
            }
            char firstLetter = gameObject.name[0];
            if (!wordScript.letterObjects.Contains(gameObject))
            {
                inputFieldText.text += firstLetter.ToString();
            }
            wordScript.AddLetter(gameObject);
        }
    }
    void CreateLetterFlash()
    {
        if (letterFlashWhenClickedPrefab != null)
        {
            GameObject flash = Instantiate(letterFlashWhenClickedPrefab, transform.position, Quaternion.identity);
            flash.transform.SetParent(transform);
        }
        else
        {
            Debug.LogWarning("Letter flash prefab is not assigned.");
        }
    }
    private IEnumerator LetterUnusable()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && spriteRenderer.color == Color.white)
        {
            isInterrupted = false;
            Color startColor = spriteRenderer.color;
            Color endColor = Color.black;
            float duration = 5f;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                if (isInterrupted)
                    yield break;

                spriteRenderer.color = Color.Lerp(startColor, endColor, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            spriteRenderer.color = endColor;
            yield return new WaitForSeconds(5f);
            letterIsUsable = false;
        }
    }
    IEnumerator LetterUsableAgain()
    {
        letterIsUsable = true;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && spriteRenderer.color == Color.black)
        {
            isInterrupted = false;
            Color startColor = spriteRenderer.color;
            Color endColor = Color.white;
            float duration = 5f;
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                if (isInterrupted)
                    yield break;

                spriteRenderer.color = Color.Lerp(startColor, endColor, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            spriteRenderer.color = endColor;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Explosion"))
        {
            if (isElectric)
            {
                Destroy(ElectricEffect);
            }
            destroyedByExplosion = true;
            if (destroyedByExplosion == true)
            {
                if (!wordScript.letterObjects.Contains(gameObject))
                {
                    wordScript.lettersCleared += 0.7f;
                    Instantiate(bombBonusPrefab, transform.position, Quaternion.identity);
                }
            }

            animator.SetTrigger("smallDeath");
            selectedEffect = transform.Find("Letter Effect(Clone)")?.gameObject;
            if (selectedEffect != null)
            {
                Destroy(selectedEffect);
            }

        }
    }
    public void TriggerBomb()
    {
        if (BombPrefab != null)
        {
            StartCoroutine(BombAfterASecond());
            if (animator != null)
            {
                animator.SetTrigger("Explosion");
            }
            selectedEffect = transform.Find("Letter Effect(Clone)")?.gameObject;
            if (selectedEffect != null)
            {
                Destroy(selectedEffect);
            }
        }
    }
    IEnumerator BombAfterASecond()
    {
        yield return new WaitForSeconds(bombDelay);
        ScreenShake.Instance.Shake(0.175f, 0.025f);
        GameObject bomb = Instantiate(BombPrefab, transform.position, Quaternion.identity, transform);
        Destroy(bomb, 0.1f);
    }
    public void DestroyMe()
    {
        Destroy(gameObject);
    }
    public void ElectricDestruction()
    {
        Debug.Log("Electric destruction triggered for: " + gameObject.name);
        wordScript.lettersCleared += 0.5f;
        Destroy(gameObject);
    }
    public void Death()
    {
        animator.SetTrigger("smallDeath");
        AudioManager.Instance.PlaySFX(SFXType.largeExplosion, 0.1f); 
    }
}