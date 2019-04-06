using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Buldozer : MonoBehaviour {

    //protected Field[,] _LevelFields;
    //public virtual void Initialize(Field[,] levelFields)
    //{
    //    _LevelFields = levelFields;
    //}

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

    private List<Vector2Int> _forestHomesPositionsList = new List<Vector2Int>();
    private int _distanceX;
    private int _distanceY;
    private List<_direction> _myMovingPatternList = new List<_direction>();

    public Sprite MySprite;

    private bool _isMoving = false;
    public float MovingSpeed;
    private float _buldozingBreakTemp;
    private float _buldozingBreakPerma = 1;

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

    public void Start()
    {
        _theLevelManager = LevelManager.Instance;

        _forestHomesPositionsList = _theLevelManager.LevelHomePositionsList;
        if (_forestHomesPositionsList != null)
        {
            _myFinalDestination = GetNearestHomePosition();
        }

        _thisField = _theLevelManager._levelFieldMatrix[MyMatrixPosition.x, MyMatrixPosition.y];

        _buldozingBreakTemp = _buldozingBreakPerma;

        SetMyMovingPattern();

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
            if (_nextField.BuldozerPosition.GetComponentInChildren<Buldozer>() != null)
            {
                //_nextField.BuldozerPosition.GetComponentInChildren<Buldozer>().Move();
                StartCoroutine(CoWaitOnTheField());
                return;
            } else
            {
                _theLevelManager._levelFieldMatrix[MyMatrixPosition.x, MyMatrixPosition.y].SetBuldozerOnMyField(null);
                
                _myTransform.parent = _theLevelManager.GetBuldozersParent();

                //Postavi varijablu odakle krećeš kao svoj transform
                _myStartingPosition = _myTransform;
                //Postavi varijablu next target iz buldozer pozicije tog susjednog polja iz matrice
                _myNextPosition = _nextField.BuldozerPosition;

                //Izbriši iz liste move patterna direction koji ti je bio odabran
                _myMovingPatternList.Remove(nextDirection);

                //Resetiraj brzinu
                //ReSetBuldozingBreak();
                //Postavi bool varijablu koja je triger za Update funkciju
                TriggerMoving(true);
            }
        }
    }

    //Update funkcija pomiče buldozer s trenutne pozicije na next poziciju
    private void Update()
    {
        if (_isMoving)
        {
            Move();
        }        
    }

    public void SetBuldozingBreak(float _buldozingBreakPower)
    {
        _buldozingBreakTemp = _buldozingBreakPower;
    }

    public void ReSetBuldozingBreak()
    {
        _buldozingBreakTemp = _buldozingBreakPerma;
    }

    private void Move()
    {
        _myTransform.position = Vector3.MoveTowards(_myStartingPosition.position, _myNextPosition.position, Time.deltaTime * MovingSpeed * _buldozingBreakTemp);

        //Kada je distance od transforma buldozera vrlo blizu target poziciji (>=0.01)
        //postavi transform buldozera točno prema transformu finalne pozicije
        //i stavi bool za Move u false.
        if (Vector3.Distance(_myTransform.position, _myNextPosition.position) <= 0.01)
        {
            TriggerMoving(false);



            ////OVE DOLJE 3 NAREDBE sam prebacio ovdje (ranije su bile dolje) jer mi destroyanje nije dobro radilo (ne znam radi li sada još uvijek dobro)
            //buldozer se nije uništavao ako bi postavio životinju nešto kasnije na polje
            _myTransform.position = _myNextPosition.position;
            _myTransform.SetParent(_nextField.BuldozerPosition);
            _nextField.SetBuldozerOnMyField(this);




            SetNewPosition();
        }
    }
    
    //Postavi svoj novi position iz buldozer pozicije ovog polja u matrici
    //Postavi se u buldozer poziciju ovog polja
    //Ako je ovdje šuma, pokreni Attack; ako nije, ponovno pokreni ovaj Move
    private void SetNewPosition()
    {
        ////OVE DOLJE 3 NAREDBE sam prebacio gore jer mi destroyanje nije dobro radilo (ne znam radi li sada još uvijek dobro)
        //buldozer se nije uništavao ako bi postavio životinju nešto kasnije na polje
        //_myTransform.position = _myNextPosition.position;
        //_myTransform.SetParent(_nextField.BuldozerPosition);
        //_nextField.SetBuldozerOnMyField(this);

        MyMatrixPosition = _nextField.MyFieldPosition;

        //Stavljam da je ovo novo polje trenutno this field, kako bih mogao dohvatiti životinje ili šumu iz njega
        _thisField = _theLevelManager._levelFieldMatrix[MyMatrixPosition.x, MyMatrixPosition.y];

        //Pokreni attack
        Attack();
    }

    public IEnumerator CoWaitOnTheField()
    {
        yield return new WaitForSeconds(BuldozingDuration);

        ReSetBuldozingBreak();

        StartMoving();
    }


    public virtual void Attack()
    {
        //Animiraj razaranje

        if(_thisField.TreesOnMyField != null)
        {
            Trees treesOnThisField = _thisField.TreesOnMyField.GetComponent<Trees>();
            treesOnThisField.StartBuldozingMe(this);
        } else
        {
            StartCoroutine(CoWaitOnTheField());
        }

        //while(_thisField.TreesOnMyField != null)

        //if()

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

        StopAllCoroutines();
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
