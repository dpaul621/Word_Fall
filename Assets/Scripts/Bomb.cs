using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    //this is a bomb script. 
    public Animator bombAnimator; // Animator to control the bomb's animation
    //circle collider
    public CircleCollider2D circleCollider; // CircleCollider2D to detect collisions
    public float delay = 5f;

    void Start()
    {
        if (bombAnimator == null)
        {
            //bombAnimator = GetComponent<Animator>(); // Get the Animator component if not assigned
        }

        if (circleCollider == null)
        {
            circleCollider = GetComponent<CircleCollider2D>(); // Get the CircleCollider2D component if not assigned
            circleCollider.enabled = false; // Disable the collider initially
        }

    }
    // This function can be called to trigger the bomb
    void OnEnable()
    {

    }
    public void TriggerBomb()
    {
        if (circleCollider != null)
        {
            circleCollider.enabled = true;
            Debug.Log("Bomb triggered! Target object activated.");
        }
        else
        {
            Debug.LogWarning("Target object is not assigned." + gameObject.name);
        }
    }
    
    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(delay); // Wait for the specified delay
        Destroy(gameObject); // Destroy the bomb object
    }
}
