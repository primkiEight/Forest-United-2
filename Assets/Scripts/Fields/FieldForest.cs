using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldForest : Field {

    private Animal _tempAnimalClone;

    //public Camera MainCamera;

    [Header("Hole Position / Transform")]
    public Transform HolePosition;
    private SpriteRenderer _mound;
    private float _moundSpeedUp = 1.0f;
    
    //[SerializeField]
    public bool IsAnimalHere = false;
    //[SerializeField]
    private bool IsFieldActive = false;

    [HideInInspector]
    public LevelManager _theLevelManager;
    [HideInInspector]
    public bool AnimalsInTheHood = false;
    [HideInInspector]
    public List<Field> FieldsWithAnimalsInTheHoodList = new List<Field> { };

    private BoxCollider2D _myBoxCollider2D;
    
    //TEST
    public Color ActiveColor;
    public Color InactiveColor;
    private SpriteRenderer _mySprite;

    private Vector3 origin = Vector3.zero;
    private Vector3 direction = Vector3.zero;

    private void Awake()
    {
        FieldController = FindObjectOfType<FieldController>();
        _myBoxCollider2D = GetComponent<BoxCollider2D>();
        _mySprite = GetComponent<SpriteRenderer>();
        _mound = HolePosition.GetComponent<SpriteRenderer>();
        SetMound(false);
        MyFieldPosition = new Vector2Int((int)transform.position.x, (int)transform.position.y);
    }

    private void Start()
    {
        _theLevelManager = LevelManager.Instance;

        if (_mySprite)
        {
            int ranIndex = Random.Range(0, _theLevelManager.ThemeData.FieldSpritesList.Count);
            _mySprite.sprite = _theLevelManager.ThemeData.FieldSpritesList[ranIndex];
            _mySprite.color = InactiveColor;
        }

        FieldsWithAnimalsInTheHoodList.Clear();

        CheckAnimalsInTheHood();
    }

    /*
    private void Update()
    {
        if (Input.GetMouseButtonUp(0)) {
        //Vector2 origin = Vector2.zero;
        ////Vector2 direction = Vector2.zero;

        //origin = Camera.main.transform.position;
        origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //direction = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //direction = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = new Vector3(origin.x, origin.y, 20f);

        //Definirati preko inspektora
        LayerMask mask = LayerMask.GetMask("Fields");
        //RaycastHit2D hit = Physics2D.Raycast(origin, direction, 50.0f, mask);
        RaycastHit hit;

        bool isHit = Physics.Raycast(origin, Vector3.forward * 30, out hit, Mathf.Infinity, mask);

        if (isHit)
        {
            Debug.Log("Hit");
            CheckMouseClick();
            return;
        }

        }
    }
    */

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(origin, Vector3.forward*30);
    }

    private void OnMouseUp()
    {
        if (Input.GetMouseButtonUp(0))
        {
            CheckMouseClick();
        }
    }

    public void CheckMouseClick()
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
                        StartCoroutine(CoEmargeAnimalInMyHole(otherSelectedField));
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

    public void SetMound(bool visible)
    {
        _mound.enabled = visible;
    }

    private IEnumerator CoEmargeAnimalInMyHole(FieldForest otherSelectedField)
    {
        //Instantiate the animal temporarily in the AnimalLimbo unvisible/deactivated gameobject
        _tempAnimalClone = Instantiate(otherSelectedField.AnimalInMyHole, AnimalPosition.position, Quaternion.identity, FieldController.GetAnimalLimboTransform());
        //Remove the animal from the previous field, animating it
        otherSelectedField.ClearField();
        //If there is a buldozer on that field, restore its speed
        if(otherSelectedField.BuldozerOnMyField != null)
            otherSelectedField.BuldozerOnMyField.ReSetBuldozingBreak();
            
        //Check and clear the otherfield's neighbours' lists once the animal is gone from that field

            otherSelectedField.ReCheckAnimalsInTheHood();
        otherSelectedField.CheckAnimalsInTheHood();
        otherSelectedField.Casting();

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

        if (_mound.enabled == true)
        {
            _moundSpeedUp = 0.8f;
        }

        yield return new WaitForSeconds(distance * _tempAnimalClone.DiggingSpeed * _moundSpeedUp);

        //Instantiate the animal (from the limbo) here on this field
        AnimalInMyHole = Instantiate(_tempAnimalClone, AnimalPosition.position, Quaternion.identity, AnimalPosition);

        SetMound(true);

        //Destroy the animal in the limbo
        _tempAnimalClone.Submerge();
        //Animate the animal emarging
        AnimalInMyHole.Emerge();

        //Provjeriti i očistiti listu susjednih životinja mog polja sada kada je ovdje životinja postavljena
        CheckAnimalsInTheHood();
        ReCheckAnimalsInTheHood();
        Casting();

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
            _mySprite.color = ActiveColor;
            //Animal animacija ide u stanje animairanja
            if (FieldController.GetSelectedField != null &&
                FieldController.GetSelectedField != this)
                FieldController.GetSelectedField.ChangeActive(false);
            FieldController.SetActiveField(this);
        }
        else
        {
            _mySprite.color = InactiveColor;
            //Ako postoji animal, Animal animacija ide u stanje mirovanja
            FieldController.SetActiveField(null);
        }
    }

    //Checks the animals in the neighbour fileds and creates the list of those fields
    public override void CheckAnimalsInTheHood()
    {
        FieldsWithAnimalsInTheHoodList.Clear();

        if (AnimalInMyHole == null)
        {
            FieldsWithAnimalsInTheHoodList.Clear();
            AnimalsInTheHood = false;
        }
        else if (AnimalInMyHole != null)
        {
            if (_theLevelManager._levelFieldMatrix[MyFieldPosition.x + 1, MyFieldPosition.y].AnimalInMyHole != null)
            {
                FieldsWithAnimalsInTheHoodList.Add(_theLevelManager._levelFieldMatrix[MyFieldPosition.x + 1, MyFieldPosition.y]);
            }
            if (_theLevelManager._levelFieldMatrix[MyFieldPosition.x - 1, MyFieldPosition.y].AnimalInMyHole != null)
            {
                FieldsWithAnimalsInTheHoodList.Add(_theLevelManager._levelFieldMatrix[MyFieldPosition.x - 1, MyFieldPosition.y]);
            }
            if (_theLevelManager._levelFieldMatrix[MyFieldPosition.x, MyFieldPosition.y + 1].AnimalInMyHole != null)
            {
                FieldsWithAnimalsInTheHoodList.Add(_theLevelManager._levelFieldMatrix[MyFieldPosition.x, MyFieldPosition.y + 1]);
            }
            if (_theLevelManager._levelFieldMatrix[MyFieldPosition.x, MyFieldPosition.y - 1].AnimalInMyHole != null)
            {
                FieldsWithAnimalsInTheHoodList.Add(_theLevelManager._levelFieldMatrix[MyFieldPosition.x, MyFieldPosition.y - 1]);
            }

            if(FieldsWithAnimalsInTheHoodList.Count != 0)
            {
                AnimalsInTheHood = true;
            } else
            {
                AnimalsInTheHood = false;
            }
        }
    }

    //Checks the neighbour fields once the animals have shifted
    public override void ReCheckAnimalsInTheHood()
    {
        if (FieldsWithAnimalsInTheHoodList.Count != 0)
        {
            for (int i = 0; i < FieldsWithAnimalsInTheHoodList.Count; i++)
            {
                FieldsWithAnimalsInTheHoodList[i].CheckAnimalsInTheHood();
                FieldsWithAnimalsInTheHoodList[i].Casting();
            }
        } else
        {
            AnimalsInTheHood = false;
        }
    }
    
    public override void Casting()
    {
        if (PowerOnMyField)
            Destroy(PowerOnMyField.gameObject);

        if(BuldozerOnMyField == null && AnimalsInTheHood == true && AnimalInMyHole == true)
        {
            CastSuperPower();
        }
        else if (BuldozerOnMyField != null && AnimalsInTheHood == true  && AnimalInMyHole != null)
        {
            CastSuperPower();
        } else if (BuldozerOnMyField != null && AnimalsInTheHood == false && AnimalInMyHole != null)
        {
            CastMidPower();
        } else if (BuldozerOnMyField != null && AnimalInMyHole == null)
        {
            if (PowerOnMyField)
                Destroy(PowerOnMyField);
            BuldozerOnMyField.ReSetBuldozingBreak();
        }
    }
    
    public void CastMidPower()
    {
        if(PowerOnMyField)
            Destroy(PowerOnMyField.gameObject);

        PowerOnMyField = Instantiate(AnimalInMyHole.MidPowerPrefab, PowerPosition.position, Quaternion.identity, PowerPosition);
        PowerOnMyField.BreakBuldozer(BuldozerOnMyField);
 
    }

    public void CastSuperPower()
    {
        if (PowerOnMyField)
            Destroy(PowerOnMyField.gameObject);

        PowerOnMyField = Instantiate(AnimalInMyHole.SuperPowerPrefab, PowerPosition.position, Quaternion.identity, PowerPosition);
        PowerOnMyField.BreakBuldozer(BuldozerOnMyField);

    }
}
