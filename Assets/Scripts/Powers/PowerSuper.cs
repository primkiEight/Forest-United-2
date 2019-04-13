using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSuper : Power {

    private void Awake()
    {
        BuldozingBreak = 1.0f;
    }

    public override void BreakBuldozer(Buldozer buldozer)
    {
        if (buldozer)
            buldozer.Death();
    }
}
