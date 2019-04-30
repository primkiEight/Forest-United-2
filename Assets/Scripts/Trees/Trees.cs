using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trees : MonoBehaviour {

    private bool _cancelDestroy = false;

    public void StartBuldozingMe(Buldozer theBuldozer)
    {
        if(theBuldozer != null)
        {
            //Pokreni Korutinu za uništenje
            //Zaustavi animaciju idle i pokreni animaciju uništavanja
            StartCoroutine(CoBuldozingMe(theBuldozer));
        }
    }

    public void StopBuldozingMe()
    {
        StopCoroutine("CoBuldozingMe");
        _cancelDestroy = true;
    }

    private IEnumerator CoBuldozingMe(Buldozer theBuldozer)
    {
        yield return new WaitForSeconds(theBuldozer.BuldozingDuration - 0.2f);

        if (!_cancelDestroy)
        {
            Tree[] allChildren = GetComponentsInChildren<Tree>();

            foreach (Tree child in allChildren)
            {
                child.AnimateAndDestroy(theBuldozer);
            }

            Destroy(gameObject);
        }

        _cancelDestroy = false;
    }
}
