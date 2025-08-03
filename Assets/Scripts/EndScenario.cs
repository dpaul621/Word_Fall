using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScenario : MonoBehaviour
{
    public GameObject endScenarioText;
    public float rayDistance = 1f;
    public float raycastOffsetX = 0.5f;
    public float raycastOffsetY = 0.5f;
    private bool scenarioTriggered = false;

    void FixedUpdate()
    {
        // Reset all LetterOnGrid objects at the beginning of the frame
        /*foreach (LetterOnGrid letter in FindObjectsOfType<LetterOnGrid>())
        {
            letter.isUnderRay = false;
        }

        Vector2 rayOrigin = new Vector2(transform.position.x - raycastOffsetX, transform.position.y - raycastOffsetY);
        Vector2 rayDirection = Vector2.right;

        Debug.DrawRay(rayOrigin, rayDirection * rayDistance, Color.green);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance);

        if (hit.collider != null && hit.collider.CompareTag("LetterOnGrid"))
        {
            LetterOnGrid letter = hit.collider.GetComponent<LetterOnGrid>();
            if (letter != null)
            {
                letter.isUnderRay = true;
            }
        }*/
    }

    public void TriggerEnd()
    {
        if (!scenarioTriggered)
        {
            scenarioTriggered = true;
            StartCoroutine(EndScenarioCoroutine());
        }
    }

    IEnumerator EndScenarioCoroutine()
    {
        endScenarioText.SetActive(true);
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}



/*using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScenario : MonoBehaviour
{
    public GameObject endScenarioText;
    Vector2 rayOrigin;
    Vector2 rayDirection;
    public float rayDistance = 1f;
    RaycastHit2D hit;
    public bool hasExitedCollider = false;
    public float raycastOffsetX = 0.5f;
    public float raycastOffsetY = 0.5f;
    void FixedUpdate()
    {
        //create a ray cast that spans the gameobject, and checks to see if it hits an object with the tag "letterOnGrid", the raycast goes horizontally from the left side of the gameobject to the right side
        rayOrigin = new Vector2(transform.position.x - raycastOffsetX, transform.position.y - raycastOffsetY);
        rayDirection = Vector2.right;
        Debug.DrawRay(rayOrigin, rayDirection * rayDistance, Color.green);
        hit = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance);
        //console log the name of the object hit by the raycast
        if (hit.collider != null && hit.collider.CompareTag("LetterOnGrid"))
        {
            //if the raycast hits an object with the tag "letterOnGrid", then end the scenario
            StartCoroutine(EndScenarioCoroutine());
        }
    }
    IEnumerator EndScenarioCoroutine()
    {
        endScenarioText.SetActive(true);
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}*/