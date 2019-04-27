using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fog : MonoBehaviour {

    [Header("Min and Max Local Scaling")]
    public Vector2 LocalScalingXMinNMax = Vector2.one;
    public Vector2 LocalScalingYMinNMax = Vector2.one;

    private SpriteRenderer _childSpriteRenderer;
    private Transform _myTransform;
    private Animator _myAC;

    private LevelManager _theLevelManager;

    private void Awake()
    {
        _myTransform = transform;
        _myAC = GetComponent<Animator>();
        _childSpriteRenderer = transform.GetComponentInChildren<SpriteRenderer>();

        //_mySpriteRenderer.sortingOrder = (int)-(transform.position.y * 100);
        
        //float RandomScaleX = Random.Range(LocalScalingXMinNMax.x, LocalScalingXMinNMax.y);
        //float RandomScaleY = Random.Range(LocalScalingYMinNMax.x, LocalScalingYMinNMax.y);
        //
        //int orientation = 1;
        //if (_myTransform.localPosition.x < 0)
        //    orientation = -1;
        //
        //foreach (Transform child in transform)
        //{
        //    child.transform.localScale = new Vector3(RandomScaleX * orientation, RandomScaleY, child.transform.localScale.z);
        //}
    }

    private void Start()
    {
        _theLevelManager = LevelManager.Instance;
        List<Sprite> levelFogSpriteList = _theLevelManager.LevelData.LevelFogSpriteList;

        float RandomScaleX = Random.Range(LocalScalingXMinNMax.x, LocalScalingXMinNMax.y);
        float RandomScaleY = Random.Range(LocalScalingYMinNMax.x, LocalScalingYMinNMax.y);

        int orientation = 1;
        if (_myTransform.localPosition.x < 0)
            orientation = -1;

        foreach (Transform child in transform)
        {
            int ranIndex = Random.Range(0, levelFogSpriteList.Count);
            Sprite sprite = levelFogSpriteList[ranIndex];
            child.GetComponent<SpriteRenderer>().sprite = sprite;

            child.localScale = new Vector3(RandomScaleX * orientation, RandomScaleY, child.transform.localScale.z);
        }
    }

    public void AnimateFog()
    {
        if(_myTransform.transform.position.x >= 0)
            _myAC.SetTrigger("FogDisappearsRight");
        else if((_myTransform.transform.position.x < 0))
            _myAC.SetTrigger("FogDisappearsLeft");
    }
}
