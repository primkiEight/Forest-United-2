using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogClear : MonoBehaviour {

    public void ClearFog()
    {
        StartCoroutine(CoClearFog());        
    }

    private IEnumerator CoClearFog()
    {
        float timer = Random.Range(0.2f, 0.4f);

        yield return new WaitForSeconds(timer);

        Fog[] allChildren = GetComponentsInChildren<Fog>();

        foreach (Fog child in allChildren)
        {
            child.AnimateFog();
        }

        Destroy(gameObject);
    }
}
