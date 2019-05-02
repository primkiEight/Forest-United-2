using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingOrderRenderer : MonoBehaviour {

    private SpriteRenderer _mySpriteRenderer;

    private void Awake()
    {
        _mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        _mySpriteRenderer.sortingOrder = (int)-(transform.position.y * 100);
    }
}
