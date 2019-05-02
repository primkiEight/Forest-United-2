using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trees : MonoBehaviour {

    private bool _cancelDestroy = false;
    private AudioSource _fieldAudioSource;
    public AudioClip TreeFalling;
    public AudioClip TreeBurning;

    public void SetAudioSource(AudioSource audioSource)
    {
        _fieldAudioSource = audioSource;
    }

    public void StartBuldozingMe(Buldozer theBuldozer)
    {
        if(theBuldozer != null)
        {
            //Pokreni Korutinu za uništenje
            //Zaustavi animaciju idle i pokreni animaciju uništavanja
            StartCoroutine(CoBuldozingMe(theBuldozer));
        }
    }

    public void StopBuldozingMe()
    {
        StopCoroutine("CoBuldozingMe");
        _cancelDestroy = true;
    }

    private IEnumerator CoBuldozingMe(Buldozer theBuldozer)
    {
        yield return new WaitForSeconds(theBuldozer.BuldozingDuration - 0.2f);

        if (!_cancelDestroy)
        {
            Tree[] allChildren = GetComponentsInChildren<Tree>();

            foreach (Tree child in allChildren)
            {
                child.AnimateAndDestroy(theBuldozer);
            }

            if (_fieldAudioSource != null && theBuldozer is BuldozerGrr)
                _fieldAudioSource.PlayOneShot(TreeFalling);
            if (_fieldAudioSource != null && theBuldozer is BuldozerFire)
                _fieldAudioSource.PlayOneShot(TreeBurning);

            Destroy(gameObject, 4.5f);
        }

        _cancelDestroy = false;
    }
}
