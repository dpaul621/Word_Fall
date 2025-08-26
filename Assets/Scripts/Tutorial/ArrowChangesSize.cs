using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowChangesSize : MonoBehaviour
{
    //change the rect for size of this object over time, fluxate between small and large like its inflating and deflating
    RectTransform rt;
    Vector2 originalSize;
    public float sizeChangeSpeed = 1f;
    public float sizeChangeAmount = 10f;

    void Start()
    {
        rt = GetComponent<RectTransform>();
        originalSize = rt.sizeDelta;
    }
//test
    void Update()
    {
        float newSize = originalSize.x + Mathf.Sin(Time.time * sizeChangeSpeed) * sizeChangeAmount;
        rt.sizeDelta = new Vector2(newSize, newSize);
    }
}
