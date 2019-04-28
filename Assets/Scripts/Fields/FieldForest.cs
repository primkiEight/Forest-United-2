using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldForest : Field {

    private Animal _tempAnimalClone;

    [Header("FieldForest Position / Transform")]
    public Transform AnimalPosition;
    public Transform HolePosition;
    public Transform PowerPosition;
    public Transform TotemPosition;
    public Transform TreesPosition;
    private SpriteRenderer _mound;
    private float _moundSpeedUp = 1.0f;

    [Header("FieldForest Prefab")]
    public GameObject FirefliesPrefab;

    [HideInInspector]
    public bool IsTotemHere = false;
    private Totem _totem;

    [HideInInspector]
    public bool IsAnimalHere = false;
    private bool IsFieldActive = false;

    [HideInInspector]
    public bool AnimalsInTheHood = false;
    [HideInInspector]
    public List<Field> FieldsWithAnimalsInTheHoodList = new List<Field> { };

    private BoxCollider2D _myBoxCollider2D;

    private PowerManager _powerManager;
    
    private SpriteRenderer _mySprite;

    private void Awake()
    {
        _myBoxCollider2D = GetComponent<BoxCollider2D>();
        _mySprite = GetComponent<SpriteRenderer>();
        _mound = HolePosition.GetComponent<SpriteRenderer>();
        SetMound(false);
        MyFieldPosition = new Vector2Int((int)transform.position.x, (int)transform.position.y);
    }

    private void Start()
    {
        _theLevelManager = LevelManager.Instance;

        FieldController = _theLevelManager.GetComponent<FieldController>();

        _powerManager = _theLevelManager.PowerManager;

        if (_mySprite)
        {
            SetMyBackground(_mySprite, _theLevelManager.ThemeData);
        }

        FieldsWithAnimalsInTheHoodList.Clear();

        CheckAnimalsInTheHood();

        if (_theLevelManager.LevelData.IncludeFogOfWar && AnimalInMyHole)
            ClearFogByAnimal();
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
        //If there is a buldozer on that field, restore its movement
        //you don't have to do that in the Casting method for when Animal is leaving the field
        if (otherSelectedField.BuldozerOnMyField != null)
            otherSelectedField.BuldozerOnMyField.ReSetBuldozingBreak();
        //Remove the animal from the previous field, animating it
        otherSelectedField.ClearField();
        
        //Check and clear the otherfield's neighbours' lists once the animal is gone from that field

        //For each neightbouring animal, recheck and change the status of the AnimalsInTheHood
        //and call Casting method on them to change/clear their powers
        otherSelectedField.ReCheckAnimalsInTheHood();
        //then clear the neighbouring list from that field once the animal is gone
        otherSelectedField.CheckAnimalsInTheHood();
        //and call the Casting method to clear that field from powers
        otherSelectedField.Casting();

        //Calculate the distance between the fields, and wait for that long for the animal to emarge
        float distance = Vector2.Distance(otherSelectedField.transform.position, transform.position);
        //Deactivate my collider so that the player cannot select me while the animal is getting here
        _myBoxCollider2D.enabled = false;

        //Seting the animation of fireflies where the animal will show up
        GameObject fireflies = Instantiate(FirefliesPrefab, HolePosition.transform.position, Quaternion.identity, HolePosition);

        //Set the boolean that the animal is now here (getting here)
        IsAnimalHere = true;
        //Wait for distance seconds for the animal to emarge here
        //including the speed of the animal

        //Checking if the mound is allread here - if it is, the animal will travel faster
        if (_mound.enabled == true)
        {
            _moundSpeedUp = 0.8f;
        }

        yield return new WaitForSeconds(distance * _tempAnimalClone.DiggingSpeed * _moundSpeedUp);

        //Instantiate the animal (from the limbo) here on this field
        AnimalInMyHole = Instantiate(_tempAnimalClone, AnimalPosition.position, Quaternion.identity, AnimalPosition);

        SetMound(true);

        ClearFogByAnimal();

        //Destroy the animal in the limbo
        Destroy(_tempAnimalClone.gameObject);
        //Animate the animal emarging
        //nije potrebno

        //Destroy the animation of fireflies
        Destroy(fireflies.gameObject);

        //If Totem is here, activate it
        if (IsTotemHere)
            ActivateTotem();

        //Check the reset the neighouring animals list for this field once the animal appears here
        CheckAnimalsInTheHood();
        //then call the Casting method here to check the buldozers once I know my neighbours
        Casting();
        //then tell other neighbours to recheck their neighbouring lists (adding - me)
        //and then call the Casting method on themselves once they now this field has an animal here
        ReCheckAnimalsInTheHood();
        
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
            //Animal animacija ide u stanje animairanja
            if(AnimalInMyHole)
                AnimalInMyHole.AnimateActive();
            if (FieldController.GetSelectedField != null &&
                FieldController.GetSelectedField != this)
                FieldController.GetSelectedField.ChangeActive(false);
            FieldController.SetActiveField(this);
        }
        else
        {
            //Ako postoji animal, Animal animacija ide u stanje mirovanja
            if (AnimalInMyHole)
                AnimalInMyHole.AnimateIdle();
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
        //Buldožer je na polju ali tamo nema životinje, niti ima susjeda, uobičajen poziv
        if (BuldozerOnMyField != null && AnimalInMyHole == null && AnimalsInTheHood == false)
        {
            if (PowerOnMyField)
                Destroy(PowerOnMyField.gameObject);
            //if (BuldozerOnMyField.IsBroken == true)
            //    BuldozerOnMyField.ReSetBuldozingBreak();
            return;
        }
        //Životinja je na polju ali tamo nema buldožera, niti ima susjeda, uobičajen poziv
        else if (BuldozerOnMyField == null && AnimalInMyHole != null && AnimalsInTheHood == false)
        {
            if (PowerOnMyField)
                Destroy(PowerOnMyField.gameObject);
            if(AnimalInMyHole.IsCasting == true)
                AnimalInMyHole.AnimateNotCastingAnymore();
            return;
        }
        //Buldozer je na tvom polju i ti si jedina životinja
        else if(BuldozerOnMyField != null && AnimalInMyHole != null && AnimalsInTheHood == false)
        {
            if (PowerOnMyField)
                Destroy(PowerOnMyField.gameObject);

            //MID POWER KOJI NAPLATIŠ
            bool hasEnoughMana = _powerManager.MagicPoolTake(AnimalInMyHole.MidPowerPrefab.ManaCost);

            if (hasEnoughMana)
            {
                AnimalInMyHole.AnimateCasting();
                PowerOnMyField = Instantiate(AnimalInMyHole.MidPowerPrefab, PowerPosition.position, Quaternion.identity, PowerPosition);
                PowerOnMyField.BreakBuldozer(BuldozerOnMyField);
            } else
            {
                //Animacija se treba izvoditi na power manageru
                return;
            }
        }
        //Buldozer je na tvom polju i imaš susjede
        else if (BuldozerOnMyField != null && AnimalInMyHole != null && AnimalsInTheHood == true)
        {
            if (PowerOnMyField)
                Destroy(PowerOnMyField.gameObject);

            //SUPER POWER KOJI NAPLATIŠ
            bool hasEnoughMana = _powerManager.MagicPoolTake(AnimalInMyHole.SuperPowerPrefab.ManaCost);

            if (hasEnoughMana)
            {
                AnimalInMyHole.AnimateCasting();
                PowerOnMyField = Instantiate(AnimalInMyHole.SuperPowerPrefab, PowerPosition.position, Quaternion.identity, PowerPosition);
                PowerOnMyField.BreakBuldozer(BuldozerOnMyField);
            } else
            {
                //Animacija se treba izvoditi na power manageru
                return;
            }
        }
        //Buldozer nije na tvom polju nego na susjednom na kojem je druga životinja
        else if (BuldozerOnMyField == null && AnimalInMyHole != null && AnimalsInTheHood == true)
        {
            //Čudno izgleda i ne funkcionira sasvim dobro, jer se naplaćuje i ukoliko se s prvim potroši mana
            //onda se tamo magija pokaže, ali ne i ovdje
            //možda da samo probam s animacijom životinje???

            AnimalInMyHole.AnimateCasting();

            /*if (PowerOnMyField)
                Destroy(PowerOnMyField.gameObject);

            //SUPER POWER KOJI NE NAPLATIŠ

            //ČUDNO IZGLEDA - MOŽDA IPAK BOLJE STAVITI NAPLATU, ALI ONDA CIJENA DA JE DUPLO MANJA, ISTO I GORE??
            //PowerOnMyField = Instantiate(AnimalInMyHole.SuperPowerPrefab, PowerPosition.position, Quaternion.identity, PowerPosition);

            bool hasEnoughMana = _powerManager.MagicPoolTake(AnimalInMyHole.SuperPowerPrefab.ManaCost / 2);

            if (hasEnoughMana)
            {
                PowerOnMyField = Instantiate(AnimalInMyHole.SuperPowerPrefab, PowerPosition.position, Quaternion.identity, PowerPosition);
                PowerOnMyField.BreakBuldozer(BuldozerOnMyField);
            }
            else
            {
                //Animacija se treba izvoditi na power manageru
                return;
            }*/
        }
        //Buldozer nije na ovom polju a nije više niti životinja
        else if(BuldozerOnMyField == null && AnimalInMyHole == null)
        {
            //AKO POSTOJI IKAKAV POWER, UNIŠTI GA
            if (PowerOnMyField)
                Destroy(PowerOnMyField.gameObject);
        }
    }

    public bool InstantiateTotemHere(Totem totemPrefab)
    {
        if (!IsTotemHere && !IsAnimalHere)
        {
            _totem = Instantiate(totemPrefab, TotemPosition.position, Quaternion.identity, TotemPosition);
            _totem.SetMyFieldForest(this);
            IsTotemHere = true;
            
            //Stops the do-while loop if false
            return false;
        }

        //Continues the do-while loop if true
        return true;
    }

    public void ActivateTotem()
    {
        if (IsTotemHere)
        {
            _totem.TotemStoneAnimator.SetTrigger("TotemDisappear");
            if (IsAnimalHere)
            {
                //Dodaj bodove na powerbar
                _theLevelManager.PowerManager.MagicPoolAdd(_totem.TotemManaValue);
            }
            //pokreni animaciju
            Destroy(_totem.gameObject, 1.5f);
            IsTotemHere = false;
        }
    }

    private void ClearFogByAnimal()
    {
        if (_theLevelManager.LevelData.IncludeFogOfWar)
        {
            if (_theLevelManager._levelFieldMatrix[MyFieldPosition.x, MyFieldPosition.y].FogOnMyField != null)
            {
                _theLevelManager._levelFieldMatrix[MyFieldPosition.x, MyFieldPosition.y].ClearFogFromMyField(transform.position.x);
            }
            if (_theLevelManager._levelFieldMatrix[MyFieldPosition.x + 1, MyFieldPosition.y].FogOnMyField != null)
            {
                _theLevelManager._levelFieldMatrix[MyFieldPosition.x + 1, MyFieldPosition.y].ClearFogFromMyField(transform.position.x);
            }
            if (_theLevelManager._levelFieldMatrix[MyFieldPosition.x - 1, MyFieldPosition.y].FogOnMyField != null)
            {
                _theLevelManager._levelFieldMatrix[MyFieldPosition.x - 1, MyFieldPosition.y].ClearFogFromMyField(transform.position.x);
            }
            if (_theLevelManager._levelFieldMatrix[MyFieldPosition.x, MyFieldPosition.y + 1].FogOnMyField != null)
            {
                _theLevelManager._levelFieldMatrix[MyFieldPosition.x, MyFieldPosition.y + 1].ClearFogFromMyField(transform.position.x);
            }
            if (_theLevelManager._levelFieldMatrix[MyFieldPosition.x, MyFieldPosition.y - 1].FogOnMyField != null)
            {
                _theLevelManager._levelFieldMatrix[MyFieldPosition.x, MyFieldPosition.y - 1].ClearFogFromMyField(transform.position.x);
            }
        }
    } 

    //PART of Field class (both FieldFOrest and FieldHome have fogs)
    //public override void ClearFogFromMyField(float positionX)
    //{
    //    if(FogOnMyField != null)
    //    {
    //        FogOnMyField.AnimateFog(positionX);
    //        Destroy(FogOnMyField.gameObject, 2f);
    //        FogOnMyField = null;
    //    }
    //}
}
