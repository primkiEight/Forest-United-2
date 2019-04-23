using System.Collections;
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

    

    public virtual void SetBuldozerOnMyField(Buldozer buldozerOnMyField)
    {
        BuldozerOnMyField = buldozerOnMyField;

        //Ovo je upitno treba li ovdje :(
        //if (AnimalInMyHole != null)
        //    Casting();
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

