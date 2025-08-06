using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    void Start()
    {
        AudioManager.Instance.PlaySFX(SFXType.SmallExplosionPop, 0.3f); 
    }
}
