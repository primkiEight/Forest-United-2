using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Field : MonoBehaviour {

    [HideInInspector]
    public FieldController FieldController;
    [HideInInspector]
    public Vector2Int MyFieldPosition;

    [Header("Field Positions / Transforms")]
    public Transform BuldozerPosition;
    
    [HideInInspector]
    public Animal AnimalInMyHole;
    [HideInInspector]
    public Power PowerOnMyField;
    [HideInInspector]
    public Buldozer BuldozerOnMyField;
    [HideInInspector]
    public Trees TreesOnMyField;
    [HideInInspector]
    public FogHolder FogOnMyField;

    [HideInInspector]
    public LevelManager _theLevelManager;

    [HideInInspector]
    public AudioSource MyAudioSource;

    public virtual void SetBuldozerOnMyField(Buldozer buldozerOnMyField)
    {
        BuldozerOnMyField = buldozerOnMyField;
    }

    public virtual void SetMyBackground(SpriteRenderer spriteRenderer, ThemeData themeData)
    {
        int ranIndex = Random.Range(0, themeData.FieldSpritesList.Count);
        spriteRenderer.sprite = themeData.FieldSpritesList[ranIndex];        
    }
    
    public virtual void AnimateHomeEarthquake()
    {

    }

    public virtual void Casting()
    {

    }
    
    public virtual void CheckAnimalsInTheHood()
    {

    }

    public virtual void ReCheckAnimalsInTheHood()
    {

    }

    public virtual void ClearFogFromMyField(float positionX)
    {
        if (FogOnMyField != null)
        {
            FogOnMyField.AnimateFog(positionX);
            Destroy(FogOnMyField.gameObject, 2f);
            FogOnMyField = null;
        }
    }

    private void PlaySoundOneShot(AudioClip clipToPlay)
    {
        MyAudioSource.pitch = Random.Range(0.8f, 1.2f);
        MyAudioSource.PlayOneShot(clipToPlay);
        MyAudioSource.pitch = 1.0f;
    }
}

