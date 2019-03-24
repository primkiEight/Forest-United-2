using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldForest : Field {

    //private FieldController FieldController;

    //[HideInInspector]
    //public Vector2Int MyFieldPosition;
    //[HideInInspector]
    public Animal AnimalInMyHole;
    //[HideInInspector]
    public Buldozer BuldozerOnMyField;
    //[HideInInspector]
    public Trees TreesOnMyField;

    public Transform HolePosition;
    public Transform AnimalPosition;
    public Transform TreesPosition;
    public Transform BuldozerPosition;

    //[SerializeField]
    public bool IsAnimalHere = false;
    [SerializeField]
    private bool IsFieldActive = false;

    //TEST
    public Color ActiveColor;
    public Color InactiveColor;
    private SpriteRenderer mySprite;

    private void Awake()
    {
        FieldController = FindObjectOfType<FieldController>();
        mySprite = GetComponent<SpriteRenderer>();
        mySprite.color = InactiveColor;
        MyFieldPosition = new Vector2Int((int)transform.position.x, (int)transform.position.y);
    }

    private void OnMouseUp()
    {
        //If I am already active and you click me, then deactivate me,
        //and tell FieldController I am no longer the selected field
        if (IsFieldActive)
        {
            ChangeActive(false);
            return;
        }
        //Else, if I am not active, check the state of the readyness to be active
        else
        {
            //If Animal is here (the field is ready to be activated and used)
            //Active this field and tell the FieldController I am the active one, select me
            if (IsAnimalHere)
            {
                ChangeActive(true);
            }
            //Else if an animal is not here
            else
            {
                FieldForest otherSelectedField = FieldController.GetSelectedField;

                //If there is an active field in the FieldController (that is not me), then clear that field,
                //move the animal there and set that field IsAnimalHere to true and IsFieldActive of that field to false
                if (otherSelectedField != null)
                {
                    //OVDJE IDE ANIMACIJA I TRAJANJE DOK ŽIVOTINJA DOĐE NA NOVO POLJE

                    Animal tempAnimal = otherSelectedField.AnimalInMyHole;

                    AnimalInMyHole = tempAnimal;
                    otherSelectedField.ClearField();
                    SetField();
                }
                //If there is no active field at all, return
                else
                {
                    //Play that funky music white boy
                    return;
                }
            }
        }
    }

    private void ClearField()
    {
        AnimalInMyHole = null;
        IsAnimalHere = false;
        ChangeActive(false);
    }

    private void SetField()
    {
        IsAnimalHere = true;
        ChangeActive(false);
    }

    public void ChangeActive(bool setState)
    {
        IsFieldActive = setState;
        if (IsFieldActive)
        {
            mySprite.color = ActiveColor;
            //Animal animacija ide u stanje animairanja
            if (FieldController.GetSelectedField != null &&
                FieldController.GetSelectedField != this)
                FieldController.GetSelectedField.ChangeActive(false);
            FieldController.SetActiveField(this);
        }
        else
        {
            mySprite.color = InactiveColor;
            //Ako postoji animal, Animal animacija ide u stanje mirovanja
            FieldController.SetActiveField(null);
        }
    }
}
