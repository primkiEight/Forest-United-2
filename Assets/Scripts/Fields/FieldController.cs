using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldController : MonoBehaviour {

    [SerializeField]
    private FieldForest SelectedField = null;
    private GameObject AnimalLimbo;

    public FieldForest GetSelectedField
    {
        get
        {
            return SelectedField;
        }
    }

    public void Awake()
    {
        AnimalLimbo = new GameObject("Animal Limbo");
        AnimalLimbo.SetActive(false);
    }

    public Transform GetAnimalLimboTransform()
    {
        return AnimalLimbo.transform;
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
