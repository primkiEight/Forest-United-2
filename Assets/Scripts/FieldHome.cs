using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldHome : Field {



    public override void SetBuldozerOnMyField(Buldozer buldozerOnMyField)
    {
        BuldozerOnMyField = buldozerOnMyField;

        if (BuldozerOnMyField != null)
        {
            //Pokreni GameOver
            Debug.Log("GameOver");
        } else if (BuldozerOnMyField == null)
        {
            //Ne bi se nikada trebalo dogoditi            
        }
    }
}
