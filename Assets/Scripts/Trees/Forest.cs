﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forest : MonoBehaviour {

    public void StartBuldozingMe(Buldozer theBuldozer)
    {
        if (theBuldozer != null)
        {
            //Pokreni Korutinu za uništenje
            //Zaustavi animaciju idle i pokreni animaciju uništavanja
            StartCoroutine(CoBuldozingMe(theBuldozer));
        }

    }

    //public void StopBuldozingMe(Buldozer theBuldozer)
    //{
    //    if(theBuldozer != null)
    //    {
    //        //Zaustavi istu korutinu za uništenje
    //        //Zaustavi animaciju uništavanja i pokreni idle animaciju
    //        StopCoroutine(CoBuldozingMe(theBuldozer));
    //    }
    //
    //    //Ako ubijem buldozera, i prije smrti se pokrene ova korutina nakon čega je buldozer odmah uništen,
    //    //javi mi da pokušavam dohvatiti nepostojeći objekt (tamo u Buldozer skripti, a ne ovdje makar je ovdje problem)
    //    //zato na ovaj način zaustavljam sve koorutine
    //    StopAllCoroutines();
    //}

    public void StopBuldozingMe()
    {
        //StopAllCoroutines();
        StopCoroutine("CoBuldozingMe");
    }

    private IEnumerator CoBuldozingMe(Buldozer theBuldozer)
    {
        yield return new WaitForSeconds(theBuldozer.BuldozingDuration - 0.2f);
        //if(theBuldozer != null)
        //    theBuldozer.StartMoving();
        Destroy(gameObject);
    }
}
