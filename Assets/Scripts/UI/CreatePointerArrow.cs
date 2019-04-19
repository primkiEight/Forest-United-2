using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePointerArrow : MonoBehaviour {

    public GameObject PointerCanvas;
    public GameObject PointerArrowPrefab;

    private void Awake()
    {
        GameObject PointerArrowClone = Instantiate(PointerArrowPrefab);
        PointerArrowClone.transform.SetParent(PointerCanvas.transform, false);
        PointerArrowClone.gameObject.GetComponent<ScreenArrowPointer>().SetTheHomeTarget(transform);
    }

}
