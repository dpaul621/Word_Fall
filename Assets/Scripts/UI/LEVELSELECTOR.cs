using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class LEVELSELECTOR : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject levelPrefab;
    public GameObject grayedOutLevelPrefab;

    [Header("UI")]
    [SerializeField] private ScrollRect scrollRect;        // assign LevelScroll
    [SerializeField] private RectTransform content;        // assign LevelScroll/Viewport/Content
    [SerializeField] private GridLayoutGroup grid;         // assign same Content
    [SerializeField] private Toggle isBetaToggle;

    [Header("Layout")]
    [SerializeField] private int columns = 7;
    [SerializeField] private int totalCap = 200;

    void OnEnable()
    {
        // configure grid once (safe to rerun)
        ConfigureGrid();
        StartCoroutine(CreateLevelButtons());
    }

    void ConfigureGrid()
    {
        if (grid == null) grid = content.GetComponent<GridLayoutGroup>();
        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = columns;
        grid.startCorner = GridLayoutGroup.Corner.UpperLeft;
        grid.childAlignment = TextAnchor.UpperLeft;

        // Make the grid cell match prefab size; spacing = target - cell
        grid.cellSize = new Vector2(150f, 150f);

        // keep your visual rhythm from before:
        grid.spacing = new Vector2(
            20f, 20f
        );
    }

    public IEnumerator CreateLevelButtons()
    {
        yield return new WaitForEndOfFrame();

        int levelFound;
        if (isBetaToggle != null && isBetaToggle.isOn)
        {
            levelFound = totalCap;
        }
        else if (GameManager.Instance.oneDifficultyMode)
        {
            levelFound = GameManager.Instance.GetOneDifficultyModeLevel();
            if (levelFound <= 0) levelFound = 1;
            GameManager.Instance.GMLevel = levelFound;
        }
        else
        {
            if (!LocalSaveManager.HasSaveFile())
                LocalSaveManager.Save(GameManager.Instance.playerProgress);
            else
                GameManager.Instance.playerProgress = LocalSaveManager.Load();

            levelFound = GameManager.Instance.GetLevelForCurrentDifficulty();
            if (levelFound <= 0) levelFound = 1;
        }

        // (re)build
        CreateLevelButtonsForReal(levelFound);

        // layout must be rebuilt before we can scroll precisely
        yield return null;
        Canvas.ForceUpdateCanvases();

        // auto-scroll so the highest unlocked row is centered-ish
        ScrollToLevel(levelFound);
    }

    void CreateLevelButtonsForReal(int levelUsed)
    {
        // clear old
        for (int i = content.childCount - 1; i >= 0; i--)
            Destroy(content.GetChild(i).gameObject);

        // unlocked buttons
        for (int i = 1; i <= levelUsed; i++)
        {
            var go = Instantiate(levelPrefab, content, false);   // parent under Content (no manual positioning!)
            TMP_Text textComp = go.GetComponentInChildren<TMP_Text>();
            textComp.text = i.ToString();

            // If it has 3 digits, shrink the font
            if (textComp.text.Length == 3)
            {
                textComp.fontSize = 75f;
            }
            else
            {
                textComp.fontSize = 90f; // your default
            }

            int level = i;
            go.GetComponent<Button>().onClick.AddListener(() =>
            {
                GameManager.Instance.GMLevel = level;
                HapticFeedback.Trigger();
                AudioManager.Instance.PlaySFX(SFXType.buttonClick, 1f);
                if (level == 1)
                {
                    SceneManager.LoadScene(2);
                }
                else
                {
                    SceneManager.LoadScene(1);
                }
            });
        }

        // locked buttons up to cap
        for (int i = levelUsed + 1; i <= totalCap; i++)
        {
            var go = Instantiate(grayedOutLevelPrefab, content, false);
            go.GetComponentInChildren<TMP_Text>().text = i.ToString();
            var btn = go.GetComponent<Button>();
            if (btn) btn.interactable = false;
            var cg = go.GetComponent<CanvasGroup>() ?? go.AddComponent<CanvasGroup>();
            // cg.alpha = 0.5f; // optional tint
        }
    }

    // centers the row containing level (best-effort) in view
    void ScrollToLevel(int levelIndex1Based)
    {
        levelIndex1Based = Mathf.Clamp(levelIndex1Based, 1, totalCap);

        // rows are zero-based
        int row = (levelIndex1Based - 1) / columns;

        // distance per row = cell height + vertical spacing
        float stepY = grid.cellSize.y + grid.spacing.y;
        float targetY = row * stepY;

        float contentHeight = content.rect.height;
        float viewportHeight = scrollRect.viewport.rect.height;
        float maxScrollPixels = Mathf.Max(0, contentHeight - viewportHeight);

        // aim a bit above the row so it’s more centered
        float desired = Mathf.Clamp(targetY - viewportHeight * 0.35f, 0, maxScrollPixels);

        // ScrollRect vertical normalized: 1 = top, 0 = bottom
        float normalized = (maxScrollPixels <= 0f) ? 1f : 1f - (desired / maxScrollPixels);
        scrollRect.normalizedPosition = new Vector2(scrollRect.normalizedPosition.x, normalized);
    }
}
