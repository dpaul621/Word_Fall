using UnityEngine;
using UnityEngine.UI;

public enum TutorialTargetType { UITarget, WorldTarget, ScreenPoint }
public enum TutorialCompleteType { ContinueButton, UIButtonClick, GameEvent, Delay }

[System.Serializable]                         // << change
public class TutorialStep                     // << no longer ScriptableObject
{
    [TextArea] public string instructionText;

    public TutorialTargetType targetType = TutorialTargetType.UITarget;

    public RectTransform uiTarget;   // for UITarget (scene ok)
    public Transform worldTarget;    // for WorldTarget (scene ok)
    public Vector2 screenPoint;      // for ScreenPoint (pixels)
    public Vector2 pixelOffset;

    public TutorialCompleteType completeType = TutorialCompleteType.ContinueButton;
    public Button uiButtonToClick;   // for UIButtonClick
    public string gameEventName;     // for GameEvent
    public float delaySeconds = 0f;

    public bool blockInputExceptTarget = false;
}

