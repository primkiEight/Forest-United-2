using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeFooterCollision : MonoBehaviour {

    private SpriteRenderer _mySpriteRenderer;
    [SerializeField]
    private int _mySortingOrderFront = 1;
    [SerializeField]
    private int _mySortingOrderBack = -1;
    public LayerMask OtherLayerMask;

    private void Awake()
    {
        _mySpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        _mySortingOrderFront = _mySpriteRenderer.sortingOrder;
        //_mySortingOrderBack = _mySortingOrderFront + 1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == OtherLayerMask)
        {
            _mySpriteRenderer.sortingOrder = _mySortingOrderBack;
            //Debug.Log("Dotaknuo sam drvo preko layera!");
        }

        if (collision.CompareTag("Buldozer"))
        {
            _mySpriteRenderer.sortingOrder = _mySortingOrderBack;
            //Debug.Log("Dotaknuo sam drvo preko taga!");
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == OtherLayerMask)
        {
            _mySpriteRenderer.sortingOrder = _mySortingOrderFront;
            //Debug.Log("Maknuo sam se s drveta preko layera!");
        }

        if (collision.CompareTag("Buldozer"))
        {
            _mySpriteRenderer.sortingOrder = _mySortingOrderFront;
            //Debug.Log("Maknuo sam se s drveta preko taga!");
        }
    }
}