using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Animal : MonoBehaviour {

    public Sprite MySprite;

    public float DiggingSpeed;

    [Header("Animal Powers")]
    public Power MidPowerPrefab;
    public Power SuperPowerPrefab;
    
    private Animator _myAnimator;

    [Header("Audio")]
    public AudioClip AudioDig;
    public AudioClip AudioActive;
    public AudioClip AudioCasting;
    public AudioClip AudioEnter;
    public AudioClip AudioExit;

    [HideInInspector]
    public bool IsCasting = false;

    private void Awake()
    {
        if(GetComponent<Animator>() != null)
            _myAnimator = GetComponent<Animator>();

        if (GetComponent<SpriteRenderer>() != null)
            GetComponent<SpriteRenderer>().sprite = MySprite;
    }

    public virtual void Submerge()
    {
        AnimateExit();
        Destroy(gameObject, _myAnimator.GetCurrentAnimatorStateInfo(0).length);
    }

    public virtual void AnimateIdle()
    {
        IsCasting = false;
        _myAnimator.SetBool("IsActive", false);
        //_myAnimator.SetBool("IsCasting", false);
    }

    public virtual void AnimateNotCastingAnymore()
    {
        IsCasting = false;
        _myAnimator.SetBool("IsCasting", false);
        _myAnimator.SetBool("IsActive", false);
    }

    public virtual void AnimateActive()
    {
        IsCasting = false;
        _myAnimator.SetBool("IsCasting", false);
        _myAnimator.SetBool("IsActive", true);
    }

    public virtual void AnimateCasting()
    {
        IsCasting = true;
        _myAnimator.SetBool("IsActive", false);
        _myAnimator.SetBool("IsCasting", true);
    }

    public virtual void AnimateExit()
    {
        _myAnimator.SetBool("IsSubmerge", true);
    }
}
