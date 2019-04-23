using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerRain : Power {

    public override void BreakBuldozer(Buldozer buldozer)
    {
        if (buldozer)
            buldozer.SetBuldozingSpeed(BuldozingBreakDuration);
    }
}
