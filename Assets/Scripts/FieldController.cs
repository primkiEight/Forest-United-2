using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldController : MonoBehaviour {

    [SerializeField]
    private FieldForest SelectedField = null;

    public FieldForest GetSelectedField
    {
        get
        {
            return SelectedField;
        }
    }

    //Called by mouseclick from the Field
    public void SetActiveField(FieldForest selectedField)
    {
        if (SelectedField == selectedField)
            SelectedField = null;
        else
            SelectedField = selectedField;
    }

}
