using UnityEngine;

public class LetterOnGrid : MonoBehaviour
{
    float requiredTime = 3f;
    private float timer = 0f;
    private bool hasTriggered = false;
    private EndScenario manager;
    private Vector2 rayOrigin;
    private Vector2 rayDirection;
    private float rayDistance;

    void Start()
    {
        manager = FindObjectOfType<EndScenario>();

    }

    void FixedUpdate()
    {
        if (hasTriggered) return;
        rayOrigin = new Vector2(transform.position.x - 0.5f, transform.position.y - 0.5f);
        rayDirection = Vector2.right;
        rayDistance = 1f;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance);
        Debug.DrawRay(rayOrigin, rayDirection * rayDistance, Color.red);
        //if my own objects tage = letterOnGrid
        if (gameObject.CompareTag("LetterOnGrid"))
        {
            if (hit.collider != null && hit.collider.CompareTag("EndScenario"))
            {
                timer += Time.fixedDeltaTime;
//                Debug.Log($"[{name}] Under ray for {timer:F2} seconds");

                if (timer >= requiredTime)
                {
                    hasTriggered = true;
                    manager.TriggerEnd();
                }
            }
            else
            {
                timer = 0f;
            }
        }
    }
}

