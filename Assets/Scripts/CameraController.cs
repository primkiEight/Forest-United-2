using System.Collections;
using System.Collections.Generic;
using UnityEngine;


static class PublicVariable
{
    public static bool ISPANNING = false;
}

public class CameraController : MonoBehaviour {

    public float PanSpeed = 1.0f;
    public float PanBorderThickness = 10.0f;
    private Vector2 _panLimit;

    private Transform _myTransform;
    private LevelManager _theLevelManager;

    public float OrtographicZOOMmin = 1.5f;
    public float ZoomStep = 0.2f;
    private float _ortographicZOOMmax;
    private float _secondCameraOrtographicProportion;
    private float _thirdCameraOrtographicProportion;
    private float _levelWidth;
    private float _levelHight;
    private float _screenRatio;
    private float _xBorder;
    private float _yBorder;

    private Camera _mainCamera;
    public Camera SecondCamera;
    public Camera ThirdCamera;



    private bool _isPanning = false;
    private bool _isZooming = false;
   
    private Vector3 _startingPositionCamera;
    private Vector3 _startingPositionTouch;
    private Vector2 _movingDirection;



    private void Awake()
    {
        _myTransform = transform;
        _mainCamera = gameObject.GetComponent<Camera>();

        _secondCameraOrtographicProportion = SecondCamera.orthographicSize / _mainCamera.orthographicSize;
        _thirdCameraOrtographicProportion =  ThirdCamera.orthographicSize / _mainCamera.orthographicSize;
    }

    private void Start()
    {
        _theLevelManager = LevelManager.Instance;

        _levelWidth = _theLevelManager.LevelData.Xmax;
        _levelHight = _theLevelManager.LevelData.Ymax;

        _screenRatio = (float)_mainCamera.pixelWidth / (float)_mainCamera.pixelHeight;

        //_screenRatio = Screen.width / Screen.height;
        //_levelRatio = (float)_levelWidth / (float)_levelHight;

        //PANNING AND CAMERA BORDERS
        _panLimit = new Vector2(_theLevelManager.LevelData.Xmax, _theLevelManager.LevelData.Ymax);

        //ZOOOM with MouseWheel limits

        _mainCamera.orthographicSize = OrtographicZOOMmin;
        SecondCamera.orthographicSize = OrtographicZOOMmin * _secondCameraOrtographicProportion;
        ThirdCamera.orthographicSize = OrtographicZOOMmin * _thirdCameraOrtographicProportion;
        
        _myTransform.position = new Vector3((float) ((_panLimit.x + 1) / 2), (float) ((_panLimit.y + 1) / 2), _myTransform.position.z);

        _yBorder = _mainCamera.orthographicSize;
        _xBorder = _yBorder * _screenRatio;

        //////Level's hight is larger or equal than it's width
        ////if (_levelHight >= _levelWidth)
        ////{
        ////    //Zoom out to the boundaries of the level:
        ////    _ortographicZOOMmax = _levelWidth * Screen.height / Screen.width * 0.5f;
        ////    
        ////    //Zoom out only up to 2 times the minimum zoom
        ////    if (_ortographicZOOMmax >= OrtographicZOOMmin * 2)
        ////        _ortographicZOOMmax = OrtographicZOOMmin * 2;
        ////
        ////}
        //////Level's width is larger than it's width
        ////else
        ////{
        ////    //Zoom out to the boundaries of the level:
        ////    _ortographicZOOMmax = _levelHight / 2;
        ////    
        ////    //Zoom out only up to 2 times the minimum zoom
        ////    if (_ortographicZOOMmax >= OrtographicZOOMmin * 2)
        ////        _ortographicZOOMmax = OrtographicZOOMmin * 2;
        ////}

        _ortographicZOOMmax = Mathf.Min(OrtographicZOOMmin * 2, _levelWidth * (1 / _screenRatio) * 0.5f);
    }
    
    private void Update()
    {
#if UNITY_STANDALONE || UNITY_WEBGL || UNITY_EDITOR
        MousePan();
        MouseZoom();
#endif

#if UNITY_ANDROID
        TouchPanAndZoom();
#endif

        /*
        //PANNING AND CAMERA BORDERS
        Vector3 pos = transform.position;

        if (Input.GetAxisRaw("Vertical") > 0.0f || Input.mousePosition.y >= (Screen.height - PanBorderThickness))
            pos.y += PanSpeed * Time.deltaTime;
        else if (Input.GetAxisRaw("Vertical") < 0.0f || Input.mousePosition.y <= PanBorderThickness)
            pos.y -= PanSpeed * Time.deltaTime;

        if (Input.GetAxisRaw("Horizontal") > 0.0f || Input.mousePosition.x >= (Screen.width - PanBorderThickness))
            pos.x += PanSpeed * Time.deltaTime;
        else if (Input.GetAxisRaw("Horizontal") < 0.0f || Input.mousePosition.x <= PanBorderThickness)
            pos.x -= PanSpeed * Time.deltaTime;

        pos.x = Mathf.Clamp(pos.x, _xBorder + 0.5f, _panLimit.x - _xBorder + 0.5f);
        pos.y = Mathf.Clamp(pos.y, _yBorder + 0.5f, _panLimit.y - _yBorder + 0.5f);

        _myTransform.position = pos;

        //ZOOOM with MouseWheel limits

        /*
        if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f)
        {
            ZoomStep = -Input.GetAxisRaw("Mouse ScrollWheel");
            if (_mainCamera.orthographicSize < _ortographicZOOMmax)
            {
                _mainCamera.orthographicSize += ZoomStep;
                SecondCamera.orthographicSize += ZoomStep * _secondCameraOrtographicProportion;
                ThirdCamera.orthographicSize += ZoomStep * _thirdCameraOrtographicProportion;
            }                
            else if (_mainCamera.orthographicSize >= _ortographicZOOMmax)
            {
                _mainCamera.orthographicSize = _ortographicZOOMmax;
                SecondCamera.orthographicSize = _ortographicZOOMmax * _secondCameraOrtographicProportion;
                ThirdCamera.orthographicSize = _ortographicZOOMmax * _thirdCameraOrtographicProportion;
            }
                
            _yBorder = _mainCamera.orthographicSize;
            _xBorder = _yBorder * _screenRatio;

        } else if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f)
        {
            ZoomStep = Input.GetAxisRaw("Mouse ScrollWheel");
            if (_mainCamera.orthographicSize > OrtographicZOOMmin)
            {
                _mainCamera.orthographicSize -= ZoomStep;
                SecondCamera.orthographicSize -= ZoomStep * _secondCameraOrtographicProportion;
                ThirdCamera.orthographicSize -= ZoomStep * _thirdCameraOrtographicProportion;
            }                
            else if (_mainCamera.orthographicSize <= OrtographicZOOMmin)
            {
                _mainCamera.orthographicSize = OrtographicZOOMmin;
                SecondCamera.orthographicSize = OrtographicZOOMmin * _secondCameraOrtographicProportion;
                ThirdCamera.orthographicSize = OrtographicZOOMmin * _thirdCameraOrtographicProportion;
            }   

            _yBorder = _mainCamera.orthographicSize;
            _xBorder = _yBorder * _screenRatio;            
        }

        if(Input.GetAxisRaw("Mouse ScrollWheel") != 0)
        {
            ZoomStep = Input.GetAxisRaw("Mouse ScrollWheel");

            _mainCamera.orthographicSize -= ZoomStep;
            SecondCamera.orthographicSize -= ZoomStep * _secondCameraOrtographicProportion;
            ThirdCamera.orthographicSize -= ZoomStep * _thirdCameraOrtographicProportion;

            _mainCamera.orthographicSize = Mathf.Clamp(_mainCamera.orthographicSize, OrtographicZOOMmin, _ortographicZOOMmax);
            SecondCamera.orthographicSize = Mathf.Clamp(SecondCamera.orthographicSize, OrtographicZOOMmin * _secondCameraOrtographicProportion, _ortographicZOOMmax * _secondCameraOrtographicProportion);
            ThirdCamera.orthographicSize = Mathf.Clamp(ThirdCamera.orthographicSize, OrtographicZOOMmin * _thirdCameraOrtographicProportion, _ortographicZOOMmax * _thirdCameraOrtographicProportion);

            _yBorder = _mainCamera.orthographicSize;
            _xBorder = _yBorder * _screenRatio;
        }*/
    }

    //PANNING AND CAMERA BORDERS
    private void MousePan()
    {
        Vector3 pos = transform.position;

        if (Input.GetAxisRaw("Vertical") > 0.0f || Input.mousePosition.y >= (Screen.height - PanBorderThickness))
            pos.y += PanSpeed * Time.deltaTime;
        else if (Input.GetAxisRaw("Vertical") < 0.0f || Input.mousePosition.y <= PanBorderThickness)
            pos.y -= PanSpeed * Time.deltaTime;

        if (Input.GetAxisRaw("Horizontal") > 0.0f || Input.mousePosition.x >= (Screen.width - PanBorderThickness))
            pos.x += PanSpeed * Time.deltaTime;
        else if (Input.GetAxisRaw("Horizontal") < 0.0f || Input.mousePosition.x <= PanBorderThickness)
            pos.x -= PanSpeed * Time.deltaTime;

        pos.x = Mathf.Clamp(pos.x, _xBorder + 0.5f, _panLimit.x - _xBorder + 0.5f);
        pos.y = Mathf.Clamp(pos.y, _yBorder + 0.5f, _panLimit.y - _yBorder + 0.5f);

        _myTransform.position = pos;
    }

    //ZOOOM with MouseWheel limits
    private void MouseZoom()
    {
        if (Input.GetAxisRaw("Mouse ScrollWheel") != 0)
        {
            ZoomStep = Input.GetAxisRaw("Mouse ScrollWheel");

            _mainCamera.orthographicSize -= ZoomStep;
            SecondCamera.orthographicSize -= ZoomStep * _secondCameraOrtographicProportion;
            ThirdCamera.orthographicSize -= ZoomStep * _thirdCameraOrtographicProportion;

            _mainCamera.orthographicSize = Mathf.Clamp(_mainCamera.orthographicSize, OrtographicZOOMmin, _ortographicZOOMmax);
            SecondCamera.orthographicSize = Mathf.Clamp(SecondCamera.orthographicSize, OrtographicZOOMmin * _secondCameraOrtographicProportion, _ortographicZOOMmax * _secondCameraOrtographicProportion);
            ThirdCamera.orthographicSize = Mathf.Clamp(ThirdCamera.orthographicSize, OrtographicZOOMmin * _thirdCameraOrtographicProportion, _ortographicZOOMmax * _thirdCameraOrtographicProportion);

            _yBorder = _mainCamera.orthographicSize;
            _xBorder = _yBorder * _screenRatio;
        }
    }

    private void TouchPanAndZoom()
    {
        if (Input.touchCount != 0)
        {
            Touch touchZero = Input.GetTouch(0);

            if (Input.touchCount == 1 && !_isZooming)
            {
                if (touchZero.phase == TouchPhase.Began)
                {
                    //_isPanning = true;
                    _startingPositionCamera = _myTransform.position;
                    _startingPositionTouch = WorldPoint(touchZero.position);
                }
                else if (touchZero.phase == TouchPhase.Moved)
                {
                    _isPanning = true;
                    PublicVariable.ISPANNING = true;

                    _movingDirection = (Vector2)WorldPoint(touchZero.position) - (Vector2)_startingPositionTouch;

                    Vector3 newPosition = _startingPositionCamera - (Vector3)_movingDirection;

                    newPosition.x = Mathf.Clamp(newPosition.x, _xBorder + 0.5f, _panLimit.x - _xBorder + 0.5f);
                    newPosition.y = Mathf.Clamp(newPosition.y, _yBorder + 0.5f, _panLimit.y - _yBorder + 0.5f);

                    _yBorder = _mainCamera.orthographicSize;
                    _xBorder = _yBorder * _screenRatio;

                    _myTransform.position = Vector3.Lerp(_startingPositionCamera, newPosition, 0.9f);
                }
                else if (touchZero.phase == TouchPhase.Ended || touchZero.phase == TouchPhase.Canceled)
                {
                    if (_isPanning)
                    {
                        _isPanning = false;
                        PublicVariable.ISPANNING = false;
                        return;
                    } else
                    {
                        //Raycast na Field
                        ////RaycastHit2D hit = Physics2D.Raycast(_myTransform.position, WorldPoint(touchZero.position));
                        ////
                        ////if (hit.collider.gameObject.GetComponent<FieldForest>())
                        ////{
                        ////    hit.collider.gameObject.GetComponent<FieldForest>().CheckMouseClick();
                        ////    Debug.Log("hit from " + _myTransform.position + " to " + WorldPoint(touchZero.position) + " hitting: " + hit.collider.gameObject.transform.position);
                        ////}
                    }
                }

                _startingPositionCamera = _myTransform.position;
            }
            else if (Input.touchCount == 2)
            {
                _isZooming = true;
                Touch touchOne = Input.GetTouch(1);

                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

                float difference = currentMagnitude - prevMagnitude;

                _mainCamera.orthographicSize = _mainCamera.orthographicSize - difference * 0.01f;
                _mainCamera.orthographicSize = Mathf.Clamp(_mainCamera.orthographicSize, OrtographicZOOMmin, _ortographicZOOMmax);
                SecondCamera.orthographicSize = SecondCamera.orthographicSize - difference * 0.01f;
                SecondCamera.orthographicSize = Mathf.Clamp(SecondCamera.orthographicSize, OrtographicZOOMmin * _secondCameraOrtographicProportion, _ortographicZOOMmax * _secondCameraOrtographicProportion);
                ThirdCamera.orthographicSize = ThirdCamera.orthographicSize - difference * 0.01f;
                ThirdCamera.orthographicSize = Mathf.Clamp(ThirdCamera.orthographicSize, OrtographicZOOMmin * _thirdCameraOrtographicProportion, _ortographicZOOMmax * _thirdCameraOrtographicProportion);

                if (touchZero.phase == TouchPhase.Ended || touchOne.phase == TouchPhase.Ended
                    || touchZero.phase == TouchPhase.Canceled || touchOne.phase == TouchPhase.Canceled)
                    _isZooming = false;

                _yBorder = _mainCamera.orthographicSize;
                _xBorder = _yBorder * _screenRatio;

                Vector3 pos = transform.position;

                pos.x = Mathf.Clamp(pos.x, _xBorder + 0.5f, _panLimit.x - _xBorder + 0.5f);
                pos.y = Mathf.Clamp(pos.y, _yBorder + 0.5f, _panLimit.y - _yBorder + 0.5f);

                _myTransform.position = pos;
            }
        }
    }

    private Vector3 WorldPoint(Vector3 position)
    {
        return _mainCamera.ScreenToWorldPoint(position);
    }
}
