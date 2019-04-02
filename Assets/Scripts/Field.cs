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

    public Transform BuldozerPosition;
    //[HideInInspector]
    public Buldozer BuldozerOnMyField;
    //[HideInInspector]
    public Transform TreesPosition;
    //[HideInInspector]
    public Trees TreesOnMyField;
}
