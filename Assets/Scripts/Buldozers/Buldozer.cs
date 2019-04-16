using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Buldozer : MonoBehaviour {

    private enum _direction
    {
        Up,
        Right,
        Down,
        Left
    }

    private Transform _myTransform;

    //[HideInInspector]
    public Vector2Int MyMatrixPosition;
    private Transform _myStartingPosition;
    private Field _thisField = null;
    private Field _nextField = null;
    private Transform _myNextPosition;
    private Vector2Int _myFinalDestination;

    private Trees _treesOnThisField = null;

    private List<Vector2Int> _forestHomesPositionsList = new List<Vector2Int>();
    private int _distanceX;
    private int _distanceY;
    private List<_direction> _myMovingPatternList = new List<_direction>();

    public Sprite MySprite;

    [SerializeField]
    //Disables/Enables setting a new field to move to
    private bool _breakA = false;
    [SerializeField]
    //Desables/Enables Update's move and setting a new field to move to
    private bool _breakB = false;
    [SerializeField]
    //Enables/Disables moving from the current field
    private bool _breakC = true;

    private bool _isMoving = false;
    public float MovingSpeed;
    private float _buldozingBreakTemp;
    private float _buldozingBreakPerma = 1.0f;

    public float BuldozingDuration;
    public float WaitOnTheEmptyField;

    private Animator _myAnimator;

    public AudioClip AudioBuldoze;
    public AudioClip AudioMove;
    public AudioClip AudioDeath;

    private LevelManager _theLevelManager;
    
    public void Awake()
    {
        _myTransform = transform;

        _myStartingPosition = _myTransform;
        //MyPosition = new Vector2Int ((int)_myStartingPosition.x, (int)_myStartingPosition.y);

        if (GetComponent<Animator>() != null)
            _myAnimator = GetComponent<Animator>();

        if (GetComponent<SpriteRenderer>() != null)
            GetComponent<SpriteRenderer>().sprite = MySprite;        
    }

    private void Start()
    {
        _theLevelManager = LevelManager.Instance;

        _forestHomesPositionsList = _theLevelManager.LevelHomePositionsList;

        _thisField = _theLevelManager._levelFieldMatrix[MyMatrixPosition.x, MyMatrixPosition.y];

        _breakA = false;
        _breakB = false;
        _breakC = true;



        StartMyEngines();

    }

    public void StartMyEngines()
    {
        if (_forestHomesPositionsList != null)
        {
            _myFinalDestination = GetNearestHomePosition();

            SetMyMovingPattern();
        }

        //_thisField = _theLevelManager._levelFieldMatrix[MyMatrixPosition.x, MyMatrixPosition.y];

        _buldozingBreakTemp = _buldozingBreakPerma;

        StartMoving();
    }

    public Vector2Int GetNearestHomePosition()
    {
        Vector2Int theNearestHomePosition = _forestHomesPositionsList[0];

        float distance = Vector2.Distance(MyMatrixPosition, theNearestHomePosition);

        for (int i = 1; i < _forestHomesPositionsList.Count; i++)
        {
            if (Vector2.Distance(MyMatrixPosition, _forestHomesPositionsList[i]) < distance)
                theNearestHomePosition = _forestHomesPositionsList[i];
        }    

        return theNearestHomePosition;
    }

    private void SetMyMovingPattern()
    {
        _distanceX = MyMatrixPosition.x - _myFinalDestination.x;
        _distanceY = MyMatrixPosition.y - _myFinalDestination.y;

        //Negative X
        if (_distanceX < 0)
        {
            //Negative Y
            if (_distanceY > 0)
            {
                for (int x = 0; x < Mathf.Abs(_distanceX); x++)
                {
                    _myMovingPatternList.Add(_direction.Right);
                }
                for (int y = 0; y < Mathf.Abs(_distanceY); y++)
                {
                    _myMovingPatternList.Add(_direction.Down);
                }
            }            
            //Positive Y
            else if (_distanceY < 0)
            {
                for (int x = 0; x < Mathf.Abs(_distanceX); x++)
                {
                    _myMovingPatternList.Add(_direction.Right);                    
                }
                for (int y = 0; y < Mathf.Abs(_distanceY); y++)
                {
                    _myMovingPatternList.Add(_direction.Up);
                }
            }
            //Zero on Y
            else if (_distanceY == 0)
            {
                for (int x = 0; x < Mathf.Abs(_distanceX); x++)
                {
                    _myMovingPatternList.Add(_direction.Right);                    
                }
            }
        }
        //Zero on X
        else if (_distanceX == 0)
        {
            //Negative Y
            if (_distanceY > 0)
            {
                for (int y = 0; y < Mathf.Abs(_distanceY); y++)
                {
                    _myMovingPatternList.Add(_direction.Down);
                }
            }
            //Positive Y
            else if (_distanceY < 0)
            {
                for (int y = 0; y < Mathf.Abs(_distanceY); y++)
                {
                    _myMovingPatternList.Add(_direction.Up);
                }
            }
        }
        //Positive X
        else if (_distanceX > 0)
        {
            //Negative Y
            if (_distanceY > 0)
            {
                for (int x = 0; x < Mathf.Abs(_distanceX); x++)
                {
                    _myMovingPatternList.Add(_direction.Left);                    
                }
                for (int y = 0; y < Mathf.Abs(_distanceY); y++)
                {
                    _myMovingPatternList.Add(_direction.Down);
                }
            }
            //Positive Y
            else if (_distanceY < 0)
            {
                for (int x = 0; x < Mathf.Abs(_distanceX); x++)
                {
                    _myMovingPatternList.Add(_direction.Left);
                }
                for (int y = 0; y < Mathf.Abs(_distanceY); y++)
                {
                    _myMovingPatternList.Add(_direction.Up);
                }
            }
            //Zero on Y
            else if (_distanceY == 0)
            {
                for (int x = 0; x < Mathf.Abs(_distanceX); x++)
                {
                    _myMovingPatternList.Add(_direction.Left);
                }
            }
        }
    }

    public void TriggerMoving(bool trigger)
    {
        _buldozingBreakTemp = _buldozingBreakPerma;
        _isMoving = trigger;
    }

    public virtual void StartMoving()
    {
        //Animiraj kretnju

        //Odaberi na random sljedeću lokaciju iz liste svojih next lokacija
        if (_myMovingPatternList.Count != 0)
        {
            int randomIndex = Random.Range(0, _myMovingPatternList.Count);
            _direction nextDirection = _myMovingPatternList[randomIndex];

            if (nextDirection == _direction.Up)
            {
                _nextField = _theLevelManager._levelFieldMatrix[MyMatrixPosition.x, MyMatrixPosition.y + 1];
            }
            else if (nextDirection == _direction.Right)
            {
                _nextField = _theLevelManager._levelFieldMatrix[MyMatrixPosition.x + 1, MyMatrixPosition.y];
                _myTransform.localScale = new Vector3(-1, 1, 1);
            }
            else if (nextDirection == _direction.Down)
            {
                _nextField = _theLevelManager._levelFieldMatrix[MyMatrixPosition.x, MyMatrixPosition.y - 1];
            }
            else if (nextDirection == _direction.Left)
            {
                _nextField = _theLevelManager._levelFieldMatrix[MyMatrixPosition.x - 1, MyMatrixPosition.y];
            }
            
            //Provjeri je li buldozer na toj lokaciji, ako je, pričekaj na ovom polju
            //al ne Mario na način da provjeravaš ima li objekta kao child:
            //if (_nextField.BuldozerPosition.GetComponentInChildren<Buldozer>() != null)
            //nego tako da provjeriš vrijednost buldozeronmyfield tog polja!
            if (_nextField.BuldozerOnMyField != null)
                {
                //_nextField.BuldozerPosition.GetComponentInChildren<Buldozer>().Move();
                StartCoroutine(CoWaitOnTheField(WaitOnTheEmptyField));
                return;
            } else
            {
                //_theLevelManager._levelFieldMatrix[MyMatrixPosition.x, MyMatrixPosition.y].SetBuldozerOnMyField(null);
                
                //_myTransform.parent = _theLevelManager.GetBuldozersParent();

                //Postavi varijablu odakle krećeš kao svoj transform
                _myStartingPosition = _myTransform;
                //Postavi varijablu next target iz buldozer pozicije tog susjednog polja iz matrice
                _myNextPosition = _nextField.BuldozerPosition;

                //Izbriši iz liste move patterna direction koji ti je bio odabran
                _myMovingPatternList.Remove(nextDirection);

                //Podešavam zajedničkog parenta dok se mičem
                _myTransform.parent = _theLevelManager.GetBuldozersParent();

                //Oslobađam ovo polje od buldožera
                _theLevelManager._levelFieldMatrix[MyMatrixPosition.x, MyMatrixPosition.y].SetBuldozerOnMyField(null);

                ////OVDJE POSTAVLJAM DA JE BULDOŽER NOVOG POLJA OVAJ BULDOŽER TIK PRIJE NEGO KRENE PREMA NOVOM POLJU
                ////STAVIO SAM TO TAKO KAKO BI IZBJEGAO DA 2 BULDOŽERA BUDU NA ISTOM POLJU
                ////Upitno je hoće li dobro funkcionirati kada stavim moći i životinje
                //Dogodilo mi se da su 2 svejedno išla na isto polje u situaciji kada su se 2 spawnala jedan do drugog i oba krenula na isto polje
                _nextField.SetBuldozerOnMyField(this);

                //OVDJE POKUŠAJ UMJESTO U FIELD APSTRAKTNOJ SKRIPTI
                _nextField.Casting();


                //Postavi bool varijablu koja je triger za Update funkciju za kretanje al prvo resetiraj brzinu na originalnu
                TriggerMoving(true);
            }
        }
    }

    public void SetBuldozingBreak()
    {
        _breakA = true;
        
        //*****************************************************************************************************
        //upitno je trebam li ovu dolje liniju uključiti ili isključiti
        //Također Casting na Field skripti kod postavljanja buldožera
        //jer se taj provjerava kod svakog spawnanja buldožera

        //_breakB = true;
        _breakC = false;

        //_buldozingBreakTemp = _buldozingBreakPower;
        if (_thisField.TreesOnMyField != null)
        {
            Trees treesOnThisField = _thisField.TreesOnMyField.GetComponent<Trees>();
            treesOnThisField.StopBuldozingMe();
        }
    }

    public void SetBuldozingSpeed(float _buldozingBreakSpeed)
    {
        _buldozingBreakTemp = _buldozingBreakSpeed;
    }

    public void ReSetBuldozingBreak()
    {
        //_buldozingBreakTemp = _buldozingBreakPerma;
        _breakA = false;
        _breakB = false;
        _breakC = true;
        if (!_isMoving)
            TriggerMoving(true);
    }

    //Update funkcija pomiče buldozer s trenutne pozicije na next poziciju
    private void Update()
    {
        if (_isMoving && !_breakB)
        {
            _myTransform.position = Vector3.MoveTowards(_myStartingPosition.position, _myNextPosition.position, Time.deltaTime * MovingSpeed * _buldozingBreakTemp);

            float distance = Vector3.Distance(_myTransform.position, _myNextPosition.position);

            if ((distance <= 0.01f) && _breakC)
            {
                if (_breakA)
                    _breakB = true;

                TriggerMoving(false);

                _breakC = false;

                if (_nextField.AnimalInMyHole != null)
                    _nextField.Casting();

                if (_thisField.AnimalInMyHole != null)
                    _thisField.Casting();

                if (!_breakA && !_breakB)
                {
                    _breakC = true;
                    SetNewPosition();
                }   
            }
        }        
    }
        
    //Postavi svoj novi position iz buldozer pozicije ovog polja u matrici
    //Postavi se u buldozer poziciju ovog polja
    //Ako je ovdje šuma, pokreni Attack; ako nije, ponovno pokreni ovaj Move
    private void SetNewPosition()
    {
        _myTransform.position = _myNextPosition.position;
        _myTransform.SetParent(_nextField.BuldozerPosition);

        MyMatrixPosition = _nextField.MyFieldPosition;

        //Stavljam da je ovo novo polje trenutno this field, kako bih mogao dohvatiti životinje ili šumu iz njega
        _thisField = _theLevelManager._levelFieldMatrix[MyMatrixPosition.x, MyMatrixPosition.y];

        _treesOnThisField = _thisField.TreesOnMyField;
        if (_thisField.TreesOnMyField != null)
        {
            StartCoroutine(CoAttackTreesOnThisField(_treesOnThisField));
            return;
        } else
        {
            StartCoroutine(CoWaitOnTheField(WaitOnTheEmptyField));
            return;
        }
    }

    public IEnumerator CoWaitOnTheField(float duration)
    {
        yield return new WaitForSeconds(duration);

        //ReSetBuldozingBreak();

        StartMoving();
    }

    public IEnumerator CoAttackTreesOnThisField(Trees treesOnThisField)
    {
        //Animiraj rezanje
        treesOnThisField.StartBuldozingMe(this);
        yield return new WaitForSeconds(BuldozingDuration);
        StartMoving();
    }

    public virtual void Death()
    {
        //Animiraj smrt
        if (_thisField.TreesOnMyField != null)
        {
            Trees treesOnThisField = _thisField.TreesOnMyField.GetComponent<Trees>();
            //treesOnThisField.StopBuldozingMe(this);
            treesOnThisField.StopBuldozingMe();
        }

        //StopAllCoroutines();
        Destroy(gameObject, 0.5f);
        //Destroy(gameObject);
    }

    public virtual void AnimateMove()
    {

    }

    public virtual void AnimateAttack()
    {

    }

    public virtual void AnimateDeath()
    {

    }
}
