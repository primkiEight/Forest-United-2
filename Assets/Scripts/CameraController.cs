using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public float PanSpeed = 1.0f;
    public float PanBorderThickness = 10.0f;
    private Vector2 _panLimit;

    private Vector3 _cameraResPixels;
    private Vector3 _cameraResWorld;

    private Vector3 dim;

    private Transform _myTransform;
    private LevelManager _theLevelManager;

    private void Awake()
    {
        _myTransform = transform;
    }

    private void Start()
    {
        _theLevelManager = LevelManager.Instance;

        _panLimit = new Vector2(_theLevelManager.LevelData.Xmax, _theLevelManager.LevelData.Ymax);

        //_myTransform.position = new Vector3((_theLevelManager.LevelData.Xmax + 1.0f) / 2.0f, (_theLevelManager.LevelData.Ymax + 1.0f) / 2.0f, transform.position.z);

        //_myTransform.position = new Vector3 (0, 0, -10.0f);
        
        _cameraResPixels = new Vector3(Screen.width, Screen.height, 0.0f);
        _cameraResWorld = Camera.main.ScreenToWorldPoint(new Vector3(_cameraResPixels.x / 2,
            _cameraResPixels.y / 2,
            0.0f));

        //Debug.Log("Pixels = " + _cameraResPixels);
        //Debug.Log("World = " + _cameraResWorld);
        /*
        Debug.Log("Camera pixelHieght = " + Camera.main.pixelHeight);
        Debug.Log("Camera pixelRect = " + Camera.main.pixelRect);
        Debug.Log("Camera pixelWidth = " + Camera.main.pixelWidth);
        Debug.Log("Camera scaledPixelHeight = " + Camera.main.scaledPixelHeight);
        Debug.Log("Camera scaledPixelWidth = " + Camera.main.scaledPixelWidth);
        Debug.Log("Camera orthographicSize = " + Camera.main.orthographicSize);
        Debug.Log("Camera cameraToWorldMatrix = " + Camera.main.cameraToWorldMatrix);
        Debug.Log("Camera worldToCameraMatrix = " + Camera.main.worldToCameraMatrix);
        Debug.Log("Camera orthographic = " + Camera.main.orthographic);
        Debug.Log("Camera projectionMatrix = " + Camera.main.projectionMatrix);
        Debug.Log("Camera scaledPixelHeight = " + Camera.main.scaledPixelHeight);
        Debug.Log("Camera scaledPixelWidth = " + Camera.main.scaledPixelWidth);
        Debug.Log("Camera worldToCameraMatrix = " + Camera.main.worldToCameraMatrix);
        */
        //float width = (Camera.main.pixelWidth / 2.0f) / Camera.main.orthographicSize;
        //float height = (Camera.main.pixelHeight / 2.0f) / Camera.main.orthographicSize;

        float width = Camera.main.pixelWidth;
        float height = Camera.main.pixelHeight;
        float width2 = Camera.main.scaledPixelWidth;
        float height2 = Camera.main.scaledPixelHeight;

        dim = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.scaledPixelWidth, Camera.main.scaledPixelHeight, 0.0f));

        Debug.Log("width in world units = " + width);
        Debug.Log("height in world units = " + height);
        Debug.Log("height in world units = " + dim);
        Debug.Log("scaled width in world units = " + width2);
        Debug.Log("sclaed height in world units = " + height2);


    }
    
    private void Update()
    {
        Vector3 pos = transform.position;

        if (Input.GetAxisRaw("Vertical") > 0.0f || Input.mousePosition.y >= (Screen.height - PanBorderThickness))
        {
            pos.y += PanSpeed * Time.deltaTime;
        }
        else if (Input.GetAxisRaw("Vertical") < 0.0f || Input.mousePosition.y <= PanBorderThickness)
        {
            pos.y -= PanSpeed * Time.deltaTime;
        }

        if (Input.GetAxisRaw("Horizontal") > 0.0f || Input.mousePosition.x >= (Screen.width - PanBorderThickness))
        {
            pos.x += PanSpeed * Time.deltaTime;
        }
        else if (Input.GetAxisRaw("Horizontal") < 0.0f || Input.mousePosition.x <= PanBorderThickness)
        {
            pos.x -= PanSpeed * Time.deltaTime;
        }

        pos.x = Mathf.Clamp(pos.x, dim.x + 0.5f, _panLimit.x - dim.x + 0.5f);
        pos.y = Mathf.Clamp(pos.y, dim.y + 0.5f, _panLimit.y - dim.y + 0.5f);
        _myTransform.position = pos;
    }
}
