﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour {

    private Animator _myAC;
    public float AnimationDuration;
    public Vector2 AnimationLag = Vector2.zero;
    private bool _isFalling = false;

    private void Awake()
    {
        _myAC = GetComponent<Animator>();        
    }

    public void AnimateAndDestroy()
    {
        Transform newParent = transform.parent;
        transform.SetParent(newParent.parent);
        StartCoroutine(CoAnimateMe());
    }

    private IEnumerator CoAnimateMe()
    {
        _isFalling = true;
        float animationLag = Random.Range(AnimationLag.x, AnimationLag.y);
        yield return new WaitForSeconds(animationLag);
        _myAC.SetBool("IsFalling", _isFalling);

        Destroy(gameObject, AnimationDuration);
        //StartCoroutine(CoDestroyMe());
    }

    //private IEnumerator CoDestroyMe()
    //{
    //    yield return new WaitForSeconds(AnimationDuration);
    //    Destroy(gameObject);
    //}
}