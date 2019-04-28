using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowTheHome : MonoBehaviour {

    public Transform TreeBottom;
    private bool _isHidden = true;

    private bool _canShowMyHome = false;

    //If Fog of War is Included, the bottom of the home stays hidden while the fog is on the field
    public void CanShowMyHome(bool canShowMyHome)
    {
        _canShowMyHome = canShowMyHome;
    }

    private void OnMouseEnter()
    {
        if (_isHidden && _canShowMyHome)
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
