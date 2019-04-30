using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour {

    private Animator _myAC;
    public float AnimationDuration;
    public Vector2 AnimationLag = Vector2.zero;
    //private bool _isDying = false;

    public AudioClip TreeFalling;
    public AudioClip TreeBurning;

    private SpriteRenderer _mySpriteRenderer;

    private AudioSource _myAudioSource;

    private void Awake()
    {
        _myAC = GetComponent<Animator>();

        _myAudioSource = GetComponent<AudioSource>();

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
        //_isDying = true;
        float animationLag = Random.Range(AnimationLag.x, AnimationLag.y);
        yield return new WaitForSeconds(animationLag);
        PlaySoundOneShot(TreeFalling);
        _myAC.SetTrigger("IsFalling");        
    }

    private IEnumerator CoAnimateMeBurning()
    {
        //_isDying = true;
        float animationLag = Random.Range(AnimationLag.x, AnimationLag.y);
        yield return new WaitForSeconds(animationLag);
        PlaySoundOneShot(TreeBurning);
        _myAC.SetTrigger("IsBurning");        
    }

    private void PlaySoundOneShot(AudioClip clipToPlay)
    {
        _myAudioSource.pitch = Random.Range(0.8f, 1.2f);
        _myAudioSource.PlayOneShot(clipToPlay);
        _myAudioSource.pitch = 1.0f;
    }
}
