using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//https://youtu.be/mKLp-2iseDc (Angle for rotation)
//https://www.youtube.com/watch?v=VHFJgQraVUs (UI Camera)
//https://www.youtube.com/watch?v=dHzeHh-3bp4 (Arrows)

public class ScreenArrowPointer : MonoBehaviour {

    private Camera _UICamera;
    public float BorderSize = 50f;

    private Vector3 _targetPosition;
    private RectTransform _pointerRectTransform;
    
    public void SetTheHomeTargetAndUICamera(Transform homeTarget, Camera UICamera)
    {
        _pointerRectTransform = gameObject.GetComponent<RectTransform>();
        _targetPosition = homeTarget.position;
        _UICamera = UICamera;
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

            bool isOffScreen = targetPositionScreenPoint.x <= BorderSize || targetPositionScreenPoint.x >= Screen.width - BorderSize || targetPositionScreenPoint.y <= BorderSize || targetPositionScreenPoint.y >= Screen.height - BorderSize;
            
            if (isOffScreen)
            {

                if (_pointerRectTransform.gameObject.GetComponent<RawImage>().enabled == false)
                    _pointerRectTransform.gameObject.GetComponent<RawImage>().enabled = true;

                Vector3 cappedTargetScreenPosition = targetPositionScreenPoint;

                if (cappedTargetScreenPosition.x <= BorderSize) cappedTargetScreenPosition.x = BorderSize;
                if (cappedTargetScreenPosition.x >= Screen.width - BorderSize) cappedTargetScreenPosition.x = Screen.width - BorderSize;
                if (cappedTargetScreenPosition.y <= BorderSize) cappedTargetScreenPosition.y = BorderSize;
                if (cappedTargetScreenPosition.y >= Screen.height - BorderSize) cappedTargetScreenPosition.y = Screen.height - BorderSize;

                Vector3 pointerWorldPosition = _UICamera.ScreenToWorldPoint(cappedTargetScreenPosition);
                _pointerRectTransform.position = pointerWorldPosition;
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
