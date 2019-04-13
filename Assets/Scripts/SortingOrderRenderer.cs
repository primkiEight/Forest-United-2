using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingOrderRenderer : MonoBehaviour {

    [SerializeField]
    private int _sortingOrderBase = 5000;
    [SerializeField]
    private int _offset = 0;
    [SerializeField]
    private bool _runOnlyOnce = false;

    private float _timer;
    private float _timerMax = 0.1f;

    private SpriteRenderer _mySpriteRenderer;

    private void Awake()
    {
        _mySpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    private void LateUpdate()
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0f)
        {
            _mySpriteRenderer.sortingOrder = (int)(_sortingOrderBase - transform.position.y - _offset);
            if (_runOnlyOnce)
                Destroy(this);
        }
    }
}
