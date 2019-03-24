using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Field : MonoBehaviour {

    [HideInInspector]
    public FieldController FieldController;

    //[HideInInspector]
    public Vector2Int MyFieldPosition;
    
}
