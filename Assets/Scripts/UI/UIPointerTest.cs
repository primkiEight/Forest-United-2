using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPointerTest : MonoBehaviour {

    public Transform MyHome;
    public Transform MainCamera;

    public float speed = 5f;

    private RectTransform _myRectTransform;
    private RectTransform _myParentRectTransform;

    [SerializeField]
    private float XminPos;
    [SerializeField]
    private float XmaxPos;
    [SerializeField]
    private float YminPos;
    [SerializeField]
    private float YmaxPos;

    [SerializeField]
    private float XminAnchor;
    [SerializeField]
    private float XmaxAnchor;
    [SerializeField]
    private float YminAnchor;
    [SerializeField]
    private float YmaxAnchor;

    private void Awake()
    {
        _myRectTransform = GetComponent<RectTransform>();
        _myParentRectTransform = _myRectTransform.parent.GetComponent<RectTransform>();

        XminAnchor = _myParentRectTransform.anchorMin.x;
        XmaxAnchor = _myParentRectTransform.anchorMax.x;
        YminAnchor = _myParentRectTransform.anchorMin.y;
        YmaxAnchor = _myParentRectTransform.anchorMax.y;

        XminPos = -Screen.width/2 + (Screen.width * XminAnchor);
        XmaxPos = Screen.width/2 - (Screen.width * (1 - XmaxAnchor));
        YminPos = -Screen.height/2 + (Screen.height * YminAnchor);
        YmaxPos = Screen.height/2 - (Screen.height * (1 - YmaxAnchor));

        //ChangePosition();
    }

    public void ChangePosition()
    {
        Debug.Log(_myRectTransform);

        Vector2 newPosition = new Vector2(XminPos, YminPos);

        _myRectTransform.localPosition = newPosition;
    }

    public void LateUpdate()
    {
        //Vector3 difference = MainCamera.position - MyHome.position;
        //
        ////_myRectTransform.position = difference;
        //
        ////Vector3 rotacija = Vector3.RotateTowards(_myRectTransform.position, MyHome.position, 1, 1);
        //
        //_myRectTransform.position = Vector3.RotateTowards(_myRectTransform.position, MyHome.position, 1, 1);
        //
        //
        //Debug.Log(difference);
        //

        Vector2 direction = MyHome.position - MainCamera.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        _myRectTransform.rotation = Quaternion.Slerp(_myRectTransform.rotation, rotation, speed * Time.deltaTime);



        Vector3 rectPosition = _myRectTransform.localPosition;

        if (direction.x <=0)
        {
            if (direction.y <= 0)
            {
                float min;
                float max;

                

                rectPosition.x = XminPos;
                rectPosition.y = YmaxPos;
            }else if (direction.y > 0)
            {
                rectPosition.x = XminPos;
                rectPosition.y = YminPos;
            }
        }
        else if (direction.x > 0)
        {
            if (direction.y <= 0)
            {
                rectPosition.x = XmaxPos;
                rectPosition.y = YminPos;
            }
            else if (direction.y > 0)
            {
                rectPosition.x = XmaxPos;
                rectPosition.y = YmaxPos;
            }
        }

        rectPosition.x = Mathf.Clamp(rectPosition.x, XminPos, XmaxPos);
        rectPosition.y = Mathf.Clamp(rectPosition.y, YminPos, YmaxPos);

        _myRectTransform.localPosition = rectPosition;



    }

}
