using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fog : MonoBehaviour {

    [Header("Min and Max Local Scaling")]
    public Vector2 LocalScalingXMinNMax = Vector2.one;
    public Vector2 LocalScalingYMinNMax = Vector2.one;

    private SpriteRenderer _mySpriteRenderer;
    private Transform _myTransform;
    private Animator _myParentAC;

    private LevelManager _theLevelManager;

    private void Awake()
    {
        _myTransform = transform;
        _myParentAC = transform.GetComponentInParent<Animator>();
        _mySpriteRenderer = GetComponent<SpriteRenderer>();

        //_mySpriteRenderer.sortingOrder = (int)-(transform.position.y * 100);
        
        float RandomScaleX = Random.Range(LocalScalingXMinNMax.x, LocalScalingXMinNMax.y);
        float RandomScaleY = Random.Range(LocalScalingYMinNMax.x, LocalScalingYMinNMax.y);

        int orientation = 1;
        if (_myTransform.localPosition.x < 0)
            orientation = -1;

        _myTransform.localScale = new Vector3(RandomScaleX * orientation, RandomScaleY, _myTransform.localScale.z);
    }

    private void Start()
    {
        _theLevelManager = LevelManager.Instance;
        List<Sprite> levelFogSpriteList = _theLevelManager.LevelData.LevelFogSpriteList;
        int ranIndex = Random.Range(0, levelFogSpriteList.Count);
        Sprite sprite = levelFogSpriteList[ranIndex];
        _mySpriteRenderer.sprite = sprite;
    }

    private void OnBecameVisible()
    {
        if(_myTransform.parent.transform.position.x >= 0)
            _myParentAC.SetTrigger("FogDisappearsRight");
        else if((_myTransform.parent.transform.position.x < 0))
            _myParentAC.SetTrigger("FogDisappearsLeft");
    }
}
