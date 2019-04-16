using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShiftMyTransformPosition : MonoBehaviour {

    private Transform _myTransform;
    public Vector2 ShiftRangeX = Vector2.zero;
    public Vector2 ShiftRangeY = Vector2.zero;

    private void Awake()
    {
        _myTransform = transform;

        float ranX = Random.Range(ShiftRangeX.x, ShiftRangeX.y);
        float ranY = Random.Range(ShiftRangeY.x, ShiftRangeY.y);

        _myTransform.position = new Vector3(_myTransform.position.x + ranX, _myTransform.position.y + ranY, _myTransform.position.z);
    }

    private void Start()
    {
        Destroy(this);
    }
}
