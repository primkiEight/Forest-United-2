using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trees : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartBuldozingMe(Buldozer theBuldozer)
    {
        if(theBuldozer != null)
        {
            //Pokreni Korutinu za uništenje
            //Zaustavi animaciju idle i pokreni animaciju uništavanja
            StartCoroutine(CoBuldozingMe(theBuldozer));
        }

    }

    public void StopBuldozingMe(Buldozer theBuldozer)
    {
        if(theBuldozer != null)
        {
            //Zaustavi istu korutinu za uništenje
            //Zaustavi animaciju uništavanja i pokreni idle animaciju
            StopCoroutine(CoBuldozingMe(theBuldozer));
        }
    }

    private IEnumerator CoBuldozingMe(Buldozer theBuldozer)
    {
        yield return new WaitForSeconds(theBuldozer.BuldozingDuration);
        theBuldozer.StartMoving();
        Destroy(gameObject);
    }
}
