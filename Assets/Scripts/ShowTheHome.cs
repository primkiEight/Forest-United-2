using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowTheHome : MonoBehaviour {

    public Transform TreeBottom;
    private bool _isHidden = true;

    private void OnMouseEnter()
    {
        if (_isHidden)
        {
            _isHidden = false;
            TreeBottom.gameObject.SetActive(true);
        }
    }

    private void OnMouseExit()
    {
        if (!_isHidden)
        {
            _isHidden = true;
            TreeBottom.gameObject.SetActive(false);
        }
    }

}
