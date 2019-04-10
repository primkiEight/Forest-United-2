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
    private float _levelWidth;
    private float _levelHight;
    private float _screenRatio;
    private float _xBorder;
    private float _yBorder;
    
    private void Awake()
    {
        _myTransform = transform;
    }

    private void Start()
    {
        _theLevelManager = LevelManager.Instance;

        //PANNING AND CAMERA BORDERS

        _panLimit = new Vector2(_theLevelManager.LevelData.Xmax, _theLevelManager.LevelData.Ymax);

        //ZOOOM with MouseWheel limits

        Camera.main.orthographicSize = OrtographicZOOMmin;

        _screenRatio = (float)Camera.main.pixelWidth / (float)Camera.main.pixelHeight;
        _yBorder = Camera.main.orthographicSize;
        _xBorder = _yBorder * _screenRatio;

        _levelWidth = _theLevelManager.LevelData.Xmax;
        _levelHight = _theLevelManager.LevelData.Ymax;

        Camera.main.orthographicSize = OrtographicZOOMmin;

        //Level's hight is larger or equal than it's width
        if (_levelHight >= _levelWidth)
        {
            _ortographicZOOMmax = _levelWidth * Screen.height / Screen.width * 0.5f;
        }
        //Level's width is larger than it's width
        else
        {
            _ortographicZOOMmax = _levelHight / 2;
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
            if(Camera.main.orthographicSize < _ortographicZOOMmax)
                Camera.main.orthographicSize += ZoomStep;
            else if (Camera.main.orthographicSize >= _ortographicZOOMmax)
                Camera.main.orthographicSize = _ortographicZOOMmax;

            _yBorder = Camera.main.orthographicSize;
            _xBorder = _yBorder * _screenRatio;

        } else if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f)
        {
            if (Camera.main.orthographicSize > OrtographicZOOMmin)
                Camera.main.orthographicSize -= ZoomStep;
            else if (Camera.main.orthographicSize <= OrtographicZOOMmin)
                Camera.main.orthographicSize = OrtographicZOOMmin;

            _yBorder = Camera.main.orthographicSize;
            _xBorder = _yBorder * _screenRatio;
            
        }
    }
}
