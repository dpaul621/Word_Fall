using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    public ParticleSystem clickParticles;
    public GameObject submitParticlesObject;
    public GameObject outlineObject;
    public float flashDuration = 0.3f;
    public bool selected = false;
    public GameObject selectedEffect;
    public GameObject selectedEffectPrefab;
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
        _rigidbody2D.velocity = new Vector2(0, letterSpawnScript.letterMovementSpeed);
        GameObject particleObject = GameObject.Find("Particles_Click");
        if (particleObject != null)
        {
            clickParticles = particleObject.GetComponent<ParticleSystem>();
        }
        Instantiate(selectedEffectPrefab, transform.position, Quaternion.identity, transform);
    }
    public void SubmitParticlesFunction()
    {
        if (submitParticlesObject != null)
        {
            GameObject fx = Instantiate(submitParticlesObject, transform.position, Quaternion.identity);
            ParticleSystem ps = fx.GetComponent<ParticleSystem>();
            if (ps != null)
                ps.Play();

            Destroy(fx, 2f); // cleanup after 2 seconds
        }
    }
    private void Update()
    {
        CheckIfLetterAboveAndBelow();
        TouchUpdate();
        SelectedeffectActivator();
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
                //if the letter is on the bottom grid or on a letter on the grid, then set the rigidbody to static
                _rigidbody2D.bodyType = RigidbodyType2D.Static;
            }
        }

        //check if hit collider above and below are not null
        if (hitAbove.collider != null && hitBelow.collider != null)
        {
            if (hitAbove.collider.CompareTag("LetterOnGrid") || (hitAbove.collider.CompareTag("FloatingLetter") && hitBelow.collider.CompareTag("LetterOnGrid")) || hitBelow.collider.CompareTag("BottomGrid"))
            {
                _rigidbody2D.bodyType = RigidbodyType2D.Static;
                //start coroutine to make the letter unusable
                StartCoroutine(LetterUnusable());
                gameObject.tag = "LetterOnGrid";
            }
        }

        if (hitBelow.collider == null && _rigidbody2D.bodyType == RigidbodyType2D.Static)
        {
            _rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            _rigidbody2D.velocity = new Vector2(0, letterSpawnScript.letterMovementSpeed);
        }

        if (hitAbove.collider == null && _rigidbody2D.bodyType == RigidbodyType2D.Static)
        {
            //if there is no letter above, then this letter is a floating letter
            StartCoroutine(LetterUsableAgain());
        }
    }
    private void TouchUpdate()
    {
    #if UNITY_EDITOR
        // Only check mouse input in the editor
        if (Input.GetMouseButtonDown(0))
        {
            HandleTouchInput(Input.mousePosition);
            return;
        }
    #else
        // Only check touch input on device
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                HandleTouchInput(touch.position);
            }
        }
    #endif
    }
    private void HandleTouchInput(Vector2 screenPosition)
    {
        // Convert screen position to world position
        Camera camera = Camera.main;
        if (camera == null) return;
        
        Vector3 worldPosition = camera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, camera.nearClipPlane));
        
        // For 2D games, use this raycast
        RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);
        
        // Check if this specific object was hit
        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            GameObject hitObject = hit.collider.gameObject;
            LetterSelected(hitObject);
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
            isInterrupted = true;
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                Debug.Log("changed to red or the bool ");
                selected = true;
                //spriteRenderer.color = Color.red;
                //on select, active te the selected effect

            }
            char firstLetter = gameObject.name[0];
            if (!wordScript.letterObjects.Contains(gameObject))
            {
                inputFieldText.text += firstLetter.ToString();
            }
            wordScript.AddLetter(gameObject);
            if (clickParticles != null)
            {
                clickParticles.transform.position = transform.position;
                clickParticles.Play();
            }
            StartCoroutine(FlashCoroutine());
        }
        else if (objectClicked != null && wordScript.letterObjects.Contains(objectClicked))
        {
            //wordScript.letterObjects.Remove(objectClicked);
        }
    }
    private IEnumerator LetterUnusable()
    {
        //over the next 5 seconds, change the color of the letter to black
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
            spriteRenderer.color = endColor; // Ensure the final color is set
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
    IEnumerator FlashCoroutine()
    {
        GameObject flash = Instantiate(outlineObject, transform.position, Quaternion.identity); // Adjust based on your flash duration
        SpriteRenderer outline = flash.GetComponent<SpriteRenderer>();
        Color c = outline.GetComponent<SpriteRenderer>().color;
        c.a = 1f;
        outline.color = c;
        float elapsed = 0f;
        while (elapsed < flashDuration)
        {
            c.a = Mathf.Lerp(1f, 0f, elapsed / flashDuration);
            outline.color = c;
            elapsed += Time.deltaTime;
            yield return null;
        }
        Destroy(flash, 0.44f);
    }
}