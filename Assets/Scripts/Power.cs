using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Power : MonoBehaviour {

    public float BuldozingBreak;

    public void BreakBuldozer(Buldozer buldozer)
    {
        buldozer.SetBuldozingBreak(BuldozingBreak);
    }

    public void DestroyBuldozer(Buldozer buldozer)
    {
        buldozer.Death();
    }
}
