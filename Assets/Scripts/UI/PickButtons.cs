using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickButtons : MonoBehaviour {

    public Transform ShowObject;
    public Sprite ButtonOpen;
    public Sprite ButtonClose;
    
    public void OnClick()
    {
        if (!ShowObject.gameObject.activeSelf)
        {
            ShowObject.gameObject.SetActive(true);
            gameObject.GetComponent<Image>().sprite = ButtonClose;
        } else if (ShowObject.gameObject.activeSelf) {
            ShowObject.gameObject.SetActive(false);
            gameObject.GetComponent<Image>().sprite = ButtonOpen;
        }   
    }
}
