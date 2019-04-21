using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeFooterCollision : MonoBehaviour {

    private SpriteRenderer _mySpriteRenderer;
    [SerializeField]
    private int _mySortingOrderFront = 1;
    [SerializeField]
    private int _mySortingOrderBack = -1;

    private int _mySortingOrder = 0;
    
    private void Start()
    {
        _mySpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        _mySortingOrder = _mySpriteRenderer.sortingOrder;
        _mySortingOrderFront = _mySpriteRenderer.sortingOrder;

        //Debug.Log(_mySortingOrder);
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Buldozer"))
    //    {
    //        //_mySpriteRenderer.sortingOrder = _mySortingOrderBack;
    //        collision.gameObject.GetComponent<SpriteRenderer>().sortingOrder = _mySortingOrder + 1;
    //        Debug.Log("Moj sorting order je: " + collision.gameObject.GetComponent<SpriteRenderer>().sortingOrder);
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Buldozer"))
    //    {
    //        //_mySpriteRenderer.sortingOrder = _mySortingOrderFront;
    //        collision.gameObject.GetComponent<SpriteRenderer>().sortingOrder = _mySortingOrder + 1;
    //        Debug.Log("Moj sorting order je: " + collision.gameObject.GetComponent<SpriteRenderer>().sortingOrder);
    //    }
    //}
}