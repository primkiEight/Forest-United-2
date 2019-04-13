using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuldozerFooter : MonoBehaviour {

    private Renderer _mySpriteRenderer;
    private int _mySortingOrder;
    private int _mySortingOrderExtra;
    public LayerMask OtherLayerMask;

    private void Awake()
    {
        _mySpriteRenderer = gameObject.GetComponent<Renderer>();
        _mySortingOrder = _mySpriteRenderer.sortingOrder;
        _mySortingOrderExtra += _mySortingOrder;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == OtherLayerMask.value)
        {
            _mySpriteRenderer.sortingOrder = _mySortingOrderExtra;
            Debug.Log("Dotaknuo sam drvo!");
        }

        if (collision.CompareTag("Tree"))
        {
            _mySpriteRenderer.sortingOrder = _mySortingOrderExtra;
            Debug.Log("Dotaknuo sam drvo!");
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == OtherLayerMask.value)
        {
            _mySpriteRenderer.sortingOrder = _mySortingOrder;
            Debug.Log("Maknuo sam se s drveta!");
        }

        if (collision.CompareTag("Tree"))
        {
            _mySpriteRenderer.sortingOrder = _mySortingOrder;
            Debug.Log("Maknuo sam se s drveta!");
        }
    }
}
