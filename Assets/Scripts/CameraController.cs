using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        //Level's hight is larger or equal than it's width
        if (_levelHight >= _levelWidth)
        {
            //Zoom out to the boundaries of the level:
            _ortographicZOOMmax = _levelWidth * Screen.height / Screen.width * 0.5f;
            
            //Zoom out only up to 2 times the minimum zoom
            if (_ortographicZOOMmax >= OrtographicZOOMmin * 2)
                _ortographicZOOMmax = OrtographicZOOMmin * 2;

        }
        //Level's width is larger than it's width
        else
        {
            //Zoom out to the boundaries of the level:
            _ortographicZOOMmax = _levelHight / 2;
            
            //Zoom out only up to 2 times the minimum zoom
            if (_ortographicZOOMmax >= OrtographicZOOMmin * 2)
                _ortographicZOOMmax = OrtographicZOOMmin * 2;
        }
    }
    
    private void Update()
    {
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
        if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f)
        {
            if(_mainCamera.orthographicSize < _ortographicZOOMmax)
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
    }
}
