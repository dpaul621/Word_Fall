using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    void Start()
    {
        AudioManager.Instance.PlaySFX(SFXType.largeExplosion, 0.7f); 
    }
}
