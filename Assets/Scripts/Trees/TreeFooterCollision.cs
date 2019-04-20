using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeFooterCollision : MonoBehaviour {

    private SpriteRenderer _mySpriteRenderer;
    [SerializeField]
    private int _mySortingOrderFront = 1;
    [SerializeField]
    private int _mySortingOrderBack = -1;
    
    private void Awake()
    {
        _mySpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        _mySortingOrderFront = _mySpriteRenderer.sortingOrder;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Buldozer"))
        {
            _mySpriteRenderer.sortingOrder = _mySortingOrderBack;            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Buldozer"))
        {
            _mySpriteRenderer.sortingOrder = _mySortingOrderFront;        
        }
    }
}