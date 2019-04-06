using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Field : MonoBehaviour {

    //protected Field[,] _LevelFields;
    //public virtual void Initialize(Field[,] levelFields)
    //{
    //    _LevelFields = levelFields;
    //}


    [HideInInspector]
    public FieldController FieldController;

    //[HideInInspector]
    public Vector2Int MyFieldPosition;

    public Animal AnimalInMyHole;
    public Transform AnimalPosition;
    public Transform PowerPosition;
    public Power PowerOnMyField;

    public Transform BuldozerPosition;
    //[HideInInspector]
    public Buldozer BuldozerOnMyField;
    //[HideInInspector]
    public Transform TreesPosition;
    //[HideInInspector]
    public Trees TreesOnMyField;

    

    public virtual void SetBuldozerOnMyField(Buldozer buldozerOnMyField)
    {
        BuldozerOnMyField = buldozerOnMyField;
        Casting();
    }

    public virtual void Casting()
    {

    }

    //public bool CheckThisFieldForAnimal()
    //{
    //    if (AnimalInMyHole != null)
    //        return true;
    //    else
    //        return false;
    //}
    //
    //public bool CheckThisFieldForBuldozer()
    //{
    //    if (BuldozerOnMyField != null)
    //        return true;
    //    else
    //        return false;
    //}

    public virtual void CheckAnimalsInTheHood()
    {

    }

    public virtual void ReCheckAnimalsInTheHood()
    {

    }

    

}

