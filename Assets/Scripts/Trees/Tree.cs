using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour {

    private Animator _myAC;
    public float AnimationDuration;
    public Vector2 AnimationLag = Vector2.zero;
    
    private SpriteRenderer _mySpriteRenderer;

    private void Awake()
    {
        _myAC = GetComponent<Animator>();

        _mySpriteRenderer = GetComponent<SpriteRenderer>();

        _mySpriteRenderer.sortingOrder = (int) -(transform.position.y * 100);
    }

    public void AnimateAndDestroy(Buldozer buldozer)
    {
        Transform newParent = transform.parent;
        transform.SetParent(newParent.parent);
        if (buldozer is BuldozerGrr)
            StartCoroutine(CoAnimateMeFalling());
        if (buldozer is BuldozerFire)
            StartCoroutine(CoAnimateMeBurning());
    }

    private IEnumerator CoAnimateMeFalling()
    {
        float animationLag = Random.Range(AnimationLag.x, AnimationLag.y);
        yield return new WaitForSeconds(animationLag);
        _myAC.SetTrigger("IsFalling");        
    }

    private IEnumerator CoAnimateMeBurning()
    {
        float animationLag = Random.Range(AnimationLag.x, AnimationLag.y);
        yield return new WaitForSeconds(animationLag);
        _myAC.SetTrigger("IsBurning");        
    }
}
