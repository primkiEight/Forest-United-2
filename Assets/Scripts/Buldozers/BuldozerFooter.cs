using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuldozerFooter : MonoBehaviour {

    private SpriteRenderer _mySpriteRenderer;
    [SerializeField]
    private int _mySortingOrder = 0;
    [SerializeField]
    private int _mySortingOrderExtra = 0;
    public LayerMask OtherLayerMask;

    private void Awake()
    {
        _mySpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        _mySortingOrder = _mySpriteRenderer.sortingOrder;
        _mySortingOrderExtra = _mySortingOrder + 1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == OtherLayerMask)
        {
            _mySpriteRenderer.sortingOrder = _mySortingOrderExtra;
            Debug.Log("Dotaknuo sam drvo preko layera!");
        }

        if (collision.CompareTag("Tree"))
        {
            _mySpriteRenderer.sortingOrder = _mySortingOrderExtra;
            Debug.Log("Dotaknuo sam drvo preko taga!");
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == OtherLayerMask)
        {
            _mySpriteRenderer.sortingOrder = _mySortingOrder;
            Debug.Log("Maknuo sam se s drveta preko layera!");
        }

        if (collision.CompareTag("Tree"))
        {
            _mySpriteRenderer.sortingOrder = _mySortingOrder;
            Debug.Log("Maknuo sam se s drveta preko taga!");
        }
    }
}
