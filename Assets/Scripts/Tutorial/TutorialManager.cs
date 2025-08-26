using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    // --- Singleton so gameplay code can call us easily ---
    public static TutorialManager Instance { get; private set; }

    [Header("Setup")]
    public List<TutorialStep> steps = new List<TutorialStep>();
    public TMP_Text instructionLabel;
    public Button continueButton;
    public RectTransform arrowRect;        // arrow Image on your Overlay canvas
    public Canvas overlayCanvas;           // Overlay canvas that contains the arrow
    public Camera worldCamera;             // usually Camera.main
    public GameObject tutorialPanel; // optional, to deactivate the panel when done
    public LetterSpawnScript spawnScript;
    public bool isTutorialActive = true; // toggle to enable/disable tutorial

    // state
    int index = -1;
    Button hookedUIButton;
    Coroutine runningDelay;

    void Awake()
    {
        Instance = this;
        if (worldCamera == null) worldCamera = Camera.main;
        if (continueButton != null) continueButton.onClick.AddListener(OnContinueClicked);
        if (arrowRect != null) arrowRect.gameObject.SetActive(false);
    }

    void OnEnable()  { NextStep(); }
    void OnDisable()
    {
        if (continueButton != null) continueButton.onClick.RemoveListener(OnContinueClicked);
        UnhookUIButton();
    }

    void Update()
    {
        if (index < 0 || index >= steps.Count) return;
        PositionArrow(steps[index]);
    }

    // ---- Public API: gameplay calls this when a world object (letter) is tapped ----
    public void NotifyWorldTap(Transform tapped)
    {
        if (index < 0 || index >= steps.Count) return;
        var step = steps[index];

        if (step.completeType != TutorialCompleteType.GameEvent) return;
        if (step.targetType != TutorialTargetType.WorldTarget) return;

        // Require the specific target to be tapped (or allow any if none set)
        if (step.worldTarget == null || tapped == step.worldTarget)
        {
            AdvanceFromExternal();
        }
    }

    // ---- Flow control ----
    void NextStep()
    {
        // cleanup previous step
        UnhookUIButton();
        if (runningDelay != null) { StopCoroutine(runningDelay); runningDelay = null; }

        index++;
        if (index >= steps.Count) { FinishTutorial(); return; }

        var step = steps[index];

        if (instructionLabel) instructionLabel.text = step.instructionText ?? "";

        if (arrowRect) arrowRect.gameObject.SetActive(true);

        // Continue button visible only when needed
        if (continueButton)
            continueButton.gameObject.SetActive(step.completeType == TutorialCompleteType.ContinueButton);

        // Wire completion modes
        switch (step.completeType)
        {
            case TutorialCompleteType.UIButtonClick:
                if (step.uiButtonToClick != null)
                {
                    hookedUIButton = step.uiButtonToClick;
                    hookedUIButton.onClick.AddListener(AdvanceFromExternal);
                }
                break;

            case TutorialCompleteType.Delay:
                runningDelay = StartCoroutine(AdvanceAfterDelay(step.delaySeconds));
                break;

            // GameEvent: we do nothing here; gameplay will call NotifyWorldTap()
            case TutorialCompleteType.GameEvent:
            default:
                break;
        }
    }

    void OnContinueClicked() => AdvanceFromExternal();

    void AdvanceFromExternal()
    {

        UnhookUIButton();
        NextStep();
    }

    IEnumerator AdvanceAfterDelay(float s)
    {
        yield return new WaitForSeconds(s);
        AdvanceFromExternal();
    }

    void UnhookUIButton()
    {
        if (hookedUIButton != null)
        {
            hookedUIButton.onClick.RemoveListener(AdvanceFromExternal);
            hookedUIButton = null;
        }
    }

    void FinishTutorial()
    {
        if (arrowRect) arrowRect.gameObject.SetActive(false);
        if (continueButton) continueButton.gameObject.SetActive(false);
        //deactivate the tutorial panel
        if (tutorialPanel != null) tutorialPanel.SetActive(false);
        //start letter spawn Coroutine
        isTutorialActive = false;
        spawnScript.StartSpawnCoroutine();
        // optionally: gameObject.SetActive(false);
    }

    // ---- Arrow placement (Canvas overlay) ----
    void PositionArrow(TutorialStep step)
    {
        if (arrowRect == null || overlayCanvas == null) return;

        Vector2 screenPos;

        if (step.targetType == TutorialTargetType.UITarget && step.uiTarget != null)
        {
            screenPos = RectTransformUtility.WorldToScreenPoint(null, step.uiTarget.position);
        }
        else if (step.targetType == TutorialTargetType.WorldTarget && step.worldTarget != null)
        {
            var cam = worldCamera != null ? worldCamera : Camera.main;
            screenPos = cam.WorldToScreenPoint(step.worldTarget.position);
        }
        else
        {
            screenPos = step.screenPoint;
        }

        screenPos += step.pixelOffset;

        RectTransform canvasRect = overlayCanvas.transform as RectTransform;
        // For Screen Space Overlay, pass null as camera; for others, pass the canvas camera
        var camForCanvas = overlayCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : overlayCanvas.worldCamera;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, camForCanvas, out var local);
        arrowRect.anchoredPosition = local;
    }
}
