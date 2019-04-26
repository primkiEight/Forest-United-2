﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Field : MonoBehaviour {

    [HideInInspector]
    public FieldController FieldController;
    [HideInInspector]
    public Vector2Int MyFieldPosition;

    [Header("Instantiate Positions / Transforms")]
    public Transform AnimalPosition;
    public Transform PowerPosition;
    public Transform BuldozerPosition;
    public Transform TreesPosition;

    //[HideInInspector]
    public Animal AnimalInMyHole;
    //[HideInInspector]
    public Power PowerOnMyField;
    //[HideInInspector]
    public Buldozer BuldozerOnMyField;
    [HideInInspector]
    public Trees TreesOnMyField;

    [HideInInspector]
    public LevelManager _theLevelManager;

    public virtual void SetBuldozerOnMyField(Buldozer buldozerOnMyField)
    {
        BuldozerOnMyField = buldozerOnMyField;
    }

    public virtual void SetMyBackground(SpriteRenderer spriteRenderer, ThemeData themeData)
    {
        int ranIndex = Random.Range(0, themeData.FieldSpritesList.Count);
        spriteRenderer.sprite = themeData.FieldSpritesList[ranIndex];
        //spriteRenderer.color = InactiveColor;
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

    

}

