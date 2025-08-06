using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LettersHaveRollingRainbowEffect : MonoBehaviour
{
    public TextMeshProUGUI text;
    public float speed = 0.1f; 
    Color[] colors = new Color[]
    {
        new Color(0.95f, 0.30f, 0.40f),  // Rose Red
        new Color(0.98f, 0.62f, 0.25f),  // Coral Orange
        new Color(1.00f, 0.85f, 0.40f),  // Warm Yellow
        new Color(0.45f, 0.90f, 0.45f),  // Mint Green
        new Color(0.30f, 0.70f, 0.95f),  // Sky Blue
        new Color(0.60f, 0.40f, 0.90f),  // Soft Violet
        new Color(0.90f, 0.50f, 0.80f)   // Orchid Pink
    };

    void OnEnable()
    {
        text = GetComponent<TextMeshProUGUI>();
        StartCoroutine(RollingRainbowFadeWave());   
    }
    private IEnumerator RollingRainbowFadeWave()
    {
        Color[] colors = new Color[]
        {
            new Color32(255, 65, 54, 255),    // Bright Red
            new Color32(255, 133, 27, 255),   // Vivid Orange
            new Color32(255, 239, 78, 255),   // Bright Yellow
            new Color32(97, 209, 79, 255),    // Bright Green
            new Color32(0, 158, 255, 255),    // Sky Blue
            new Color32(34, 32, 217, 255),    // Deep Blue
            new Color32(199, 48, 246, 255)    // Hot Magenta
        };
        float fadeDuration = speed;

        text.ForceMeshUpdate();
        TMP_TextInfo textInfo = text.textInfo;
        int characterCount = textInfo.characterCount;

        int colorIndexOffset = 0;

        while (true)
        {
            text.ForceMeshUpdate();
            textInfo = text.textInfo;

            float t = 0f;

            while (t < 1f)
            {
                t += Time.deltaTime / fadeDuration;

                for (int i = 0; i < characterCount; i++)
                {
                    if (!textInfo.characterInfo[i].isVisible) continue;

                    int meshIndex = textInfo.characterInfo[i].materialReferenceIndex;
                    int vertexIndex = textInfo.characterInfo[i].vertexIndex;

                    Color startColor = colors[(colorIndexOffset + i) % colors.Length];
                    Color endColor = colors[(colorIndexOffset + i + 1) % colors.Length];

                    Color lerped = Color.Lerp(startColor, endColor, t);

                    Color32[] vertexColors = textInfo.meshInfo[meshIndex].colors32;
                    vertexColors[vertexIndex + 0] = lerped;
                    vertexColors[vertexIndex + 1] = lerped;
                    vertexColors[vertexIndex + 2] = lerped;
                    vertexColors[vertexIndex + 3] = lerped;
                }

                // Apply colors to mesh
                for (int m = 0; m < textInfo.meshInfo.Length; m++)
                {
                    textInfo.meshInfo[m].mesh.colors32 = textInfo.meshInfo[m].colors32;
                    text.UpdateGeometry(textInfo.meshInfo[m].mesh, m);
                }

                yield return null;
            }

            colorIndexOffset = (colorIndexOffset + 1) % colors.Length;
        }
    }


        /*private IEnumerator RollingRainbowEffect()
        {
            Color[] colors = new Color[] { Color.red, Color.yellow, Color.green, Color.cyan, Color.blue, Color.magenta };
            int currentColorIndex = 0;

            while (true)
            {
                text.color = colors[currentColorIndex];
                currentColorIndex = (currentColorIndex + 1) % colors.Length;
                yield return new WaitForSeconds(speed);
            }
        }*/
    }
