using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapHolderController : MonoBehaviour {

    RectTransform _myRectTransform;

	void Awake () {

        _myRectTransform = GetComponent<RectTransform>();

        float _screenRatio = (float)Screen.width / (float)Screen.height;

		if(_myRectTransform)
            _myRectTransform.localScale = new Vector3(_myRectTransform.localScale.x * _screenRatio, _myRectTransform.localScale.y, _myRectTransform.localScale.z);

    }
	
}
