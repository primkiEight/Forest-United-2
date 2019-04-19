using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//https://www.youtube.com/watch?v=VHFJgQraVUs
//https://www.youtube.com/watch?v=dHzeHh-3bp4

public class ScreenArrowPointer : MonoBehaviour {

    public Camera UICamera;
    //public Transform TargetTransform;
    public float BorderSize = 50f;

    private Vector3 _targetPosition;
    private RectTransform _pointerRectTransform;

    //private void Awake()
    //{
    //    _targetPosition = TargetTransform.position;
    //    //_pointerRectTransform = transform.GetComponent<RectTransform>();
    //    
    //}

    public void SetTheHomeTarget(Transform homeTarget)
    {
        _pointerRectTransform = gameObject.GetComponent<RectTransform>();
        _targetPosition = homeTarget.position;
    }

    private void LateUpdate()
    {
        if (_pointerRectTransform)
        {

            Vector3 toPosition = _targetPosition;
            Vector3 fromPosition = Camera.main.transform.position;
            fromPosition.z = 0.0f;

            Vector3 direction = (toPosition - fromPosition).normalized;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            _pointerRectTransform.localEulerAngles = new Vector3(0, 0, angle);

            Vector3 targetPositionScreenPoint = Camera.main.WorldToScreenPoint(_targetPosition);

            Debug.Log(targetPositionScreenPoint);

            bool isOffScreen = targetPositionScreenPoint.x <= BorderSize || targetPositionScreenPoint.x >= Screen.width - BorderSize || targetPositionScreenPoint.y <= BorderSize || targetPositionScreenPoint.y >= Screen.height - BorderSize;

            //Debug.Log(isOffScreen + " " + targetPositionScreenPoint);

            if (isOffScreen)
            {

                if (_pointerRectTransform.gameObject.GetComponent<RawImage>().enabled == false)
                    _pointerRectTransform.gameObject.GetComponent<RawImage>().enabled = true;

                Vector3 cappedTargetScreenPosition = targetPositionScreenPoint;

                if (cappedTargetScreenPosition.x <= BorderSize) cappedTargetScreenPosition.x = BorderSize;
                if (cappedTargetScreenPosition.x >= Screen.width - BorderSize) cappedTargetScreenPosition.x = Screen.width - BorderSize;
                if (cappedTargetScreenPosition.y <= BorderSize) cappedTargetScreenPosition.y = BorderSize;
                if (cappedTargetScreenPosition.y >= Screen.height - BorderSize) cappedTargetScreenPosition.y = Screen.height - BorderSize;

                //Debug.Log(cappedTargetScreenPosition);

                Vector3 pointerWorldPosition = UICamera.ScreenToWorldPoint(cappedTargetScreenPosition);
                _pointerRectTransform.position = pointerWorldPosition;
                //_pointerRectTransform.localPosition = pointerWorldPosition;
                _pointerRectTransform.localPosition = new Vector3(_pointerRectTransform.localPosition.x, _pointerRectTransform.localPosition.y, 0.0f);
            }
            else
            {
                if (_pointerRectTransform.gameObject.GetComponent<RawImage>().enabled == true)
                    _pointerRectTransform.gameObject.GetComponent<RawImage>().enabled = false;
            }
        }

    }

}
