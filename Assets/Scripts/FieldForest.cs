using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldForest : Field {

    //[HideInInspector]
    public Animal AnimalInMyHole;
    private Animal _tempAnimalClone;
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

    private BoxCollider2D _myBoxCollider2D;

    //TEST
    public Color ActiveColor;
    public Color InactiveColor;
    private SpriteRenderer mySprite;

    private void Awake()
    {
        FieldController = FindObjectOfType<FieldController>();
        _myBoxCollider2D = GetComponent<BoxCollider2D>();
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
                    //Starting a coroutine for emarging the selected animal
                    StartCoroutine("CoEmargeAnimalInMyHole", otherSelectedField);
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
        AnimalInMyHole.Submerge();
        AnimalInMyHole = null;
        IsAnimalHere = false;
        ChangeActive(false);
    }

    //private void SetField(float distance)
    //{
        ////IsAnimalHere = true;
        //Invoke("EmergeAnimalInMyHole", _tempAnimalClone.DiggingSpeed * distance);
        //AnimalInMyHole = Instantiate(_tempAnimalClone, AnimalPosition.position, Quaternion.identity, AnimalPosition);        
        ////StartCoroutine("CoEmargeAnimalInMyHole");
        ////ChangeActive(false);


        //Napraviti korutinu??
        //Prije yielda, ubiti životinju na starom polju i deselektirati to polje
        //a na novom podesiti sve spremno (ovaj limbo i tako to)
        //ubaciti animaciju, strelicu iznat tog polja s facom životinje koja se treba pojaviti
        //disejblati kolider da player ne može kliktati ovo aktivno polje
        //i nakon yielda pojavi se životinja
        //ugasi se animacija
        //collider ponovno postane aktivan
    //}
    
    private IEnumerator CoEmargeAnimalInMyHole(FieldForest otherSelectedField)
    {
        //Instantiate the animal temporarily in the AnimalLimbo unvisible/deactivated gameobject
        _tempAnimalClone = Instantiate(otherSelectedField.AnimalInMyHole, AnimalPosition.position, Quaternion.identity, FieldController.GetAnimalLimboTransform());
        //Remove the animal from the previous field, animating it
        otherSelectedField.ClearField();
        //Calculate the distance between the fields, and wait for that long for the animal to emarge
        float distance = Vector2.Distance(otherSelectedField.transform.position, transform.position);
        //Deactivate my collider so that the player cannot select me while the animal is getting here
        _myBoxCollider2D.enabled = false;
        //ubaciti animaciju, strelicu iznat tog polja s facom životinje koja se treba pojaviti
        //Set the boolean that the animal is now here (getting here)
        IsAnimalHere = true;
        //Wait for distance seconds for the animal to emarge here
        //including the speed of the animal
        //i dodati provjeru je li rupa već iskopana ili nije
        yield return new WaitForSeconds(distance * _tempAnimalClone.DiggingSpeed);
        //Instantiate the animal (from the limbo) here on this field
        AnimalInMyHole = Instantiate(_tempAnimalClone, AnimalPosition.position, Quaternion.identity, AnimalPosition);
        //Destroy the animal in the limbo
        _tempAnimalClone.Submerge();
        //Animate the animal emarging
        AnimalInMyHole.Emerge();
        //Return the status of the boxcollider to active
        _myBoxCollider2D.enabled = true;
        //izbaciti animaciju, strelicu iznat tog polja s facom životinje koja se treba pojaviti
        //Set this field to not active once the animal is here
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
