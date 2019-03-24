using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Animal : MonoBehaviour {

    protected Field[,] _LevelFields;
    public virtual void Initialize(Field[,] levelFields)
    {
        _LevelFields = levelFields;
    }

    public Sprite MySprite;

    public float DiggingSpeed;

    public float PowerDuration;
    public GameObject PowerPrefab;

    private Animator _myAnimator;

    public AudioClip AudioDig;
    public AudioClip AudioActive;
    public AudioClip AudioCasting;
    public AudioClip AudioEnter;
    public AudioClip AudioExit;

    private void Awake()
    {
        if(GetComponent<Animator>() != null)
            _myAnimator = GetComponent<Animator>();

        if (GetComponent<SpriteRenderer>() != null)
            GetComponent<SpriteRenderer>().sprite = MySprite;
    }

    public virtual void Move(float diggingSpeed)
    {
        StartCoroutine("Digging", diggingSpeed);
    }

    private IEnumerator Digging(float diggingSpeed)
    {
        AnimateExit();
        yield return new WaitForSeconds(diggingSpeed);
        AnimateEnter();
    }

    public virtual void Attack()
    {
        
    }

    public virtual void AnimateIdle()
    {

    }

    public virtual void AnimateActive()
    {

    }

    public virtual void AnimateCasting()
    {

    }

    public virtual void AnimateEnter()
    {

    }

    public virtual void AnimateExit()
    {

    }
}
