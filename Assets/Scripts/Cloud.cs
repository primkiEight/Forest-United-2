using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour {

    private bool _isMoving = false;
    private float _speed;
    private int _destroyPositionX;
    private Vector3 _destroyPosition = Vector3.zero;
    private Transform _myTransform;
    private int _startingLayerIndex;
    
    private void Awake()
    {
        _myTransform = transform;

        float ranSize = Random.Range(0.6f, 1.2f);

        Vector3 newSize = Vector3.one * ranSize;

        _myTransform.localScale = newSize;

        _startingLayerIndex = gameObject.layer;
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

    public void SetLayerMask(string layerName)
    {
        gameObject.layer = LayerMask.NameToLayer(layerName);        
    }

    public void CreateShadow(string shadowLayerName, Vector3 shadowOffset, string shadowSortingLayer)
    {
        GameObject cloudShadow = Instantiate(gameObject, transform.position + shadowOffset, Quaternion.identity, transform);

        foreach (Transform child in cloudShadow.transform)
        {
            Destroy(child.gameObject);
        }

        CompositeCollider2D shadowCollider = cloudShadow.GetComponent<CompositeCollider2D>();
        if(shadowCollider)
            Destroy(shadowCollider);

        Rigidbody2D shadowRigidBody2D = cloudShadow.GetComponent<Rigidbody2D>();
        if (shadowRigidBody2D)
            Destroy(shadowRigidBody2D);
        
        cloudShadow.layer = LayerMask.NameToLayer(shadowLayerName);

        int goLayerIndexChange = gameObject.layer;

        if (goLayerIndexChange != _startingLayerIndex)
            cloudShadow.transform.localScale = Vector3.one * 0.6f;
        else
            cloudShadow.transform.localScale = Vector3.one * 0.8f;

        SpriteRenderer sprite = cloudShadow.GetComponent<SpriteRenderer>();

        if (sprite)
        {
            Color spriteColor = sprite.color;
            spriteColor = Color.black;
            float ranAlpha = Random.Range(0.1f, 0.5f);
            spriteColor.a = ranAlpha;
            sprite.color = spriteColor;

            sprite.sortingLayerName = shadowSortingLayer;
        }

        
    }

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
