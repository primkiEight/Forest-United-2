﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Power : MonoBehaviour {

    [Header("Duration Of Buldozer SlowDown (sec)")]
    public float BuldozingBreakDuration;
    [Range(0f, 100f)]
    public float ManaCost;

    public AudioClip AudioPower;

    public virtual void BreakBuldozer(Buldozer buldozer)
    {
        
    }
}
