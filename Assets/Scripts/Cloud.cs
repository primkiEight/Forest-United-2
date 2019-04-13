using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour {

    private bool _isMoving = false;
    private float _speed;
    private int _destroyPositionX;
    private Vector3 _destroyPosition = Vector3.zero;
    private Transform _myTransform;
    //public List<LayerMask> LayerMasksList = new List<LayerMask> { };

    private void Awake()
    {
        _myTransform = transform;

        float ranSize = Random.Range(0.8f, 1.2f);

        Vector3 newSize = Vector3.one * ranSize;

        _myTransform.localScale = newSize;

    }

    public void SetCloudSpeed(float speed)
    {
        _speed = speed;
    }

    public void SetDestroyPosition (int destroyPosition)
    {
        _destroyPositionX = destroyPosition;
        _destroyPosition = new Vector3(destroyPosition, _myTransform.position.y, _myTransform.position.z);
    }

    //public void SetLayerMask()
    //{
    //    if(LayerMasksList.Count > 1)
    //    {
    //        int ranIndex = Random.Range(0, LayerMasksList.Count);
    //        Debug.Log(LayerMasksList[ranIndex]);
    //        Debug.Log(gameObject.layer);
    //        /////OVAKO NE RADI gameObject.layer = LayerMasksList[ranIndex];
    //    }
    //}

    public void StartMoving()
    {
        _isMoving = true;
    }

    private void Update()
    {
        if (_isMoving)
        {
            _myTransform.position = Vector3.MoveTowards(_myTransform.position, _destroyPosition, Time.deltaTime * _speed);

            if (_myTransform.position.x <= _destroyPositionX + 1)
                Destroy(gameObject);
        }
    }
}
