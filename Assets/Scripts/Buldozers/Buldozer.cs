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

    //[HideInInspector]
    public Vector2Int MyMatrixPosition;
    private Transform _myStartingPosition;
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

    public float BuldozingDuration;
    
    private Animator _myAnimator;

    public AudioClip AudioBuldoze;
    public AudioClip AudioMove;
    public AudioClip AudioDeath;

    private LevelManager _theLevelManager;
    
    public void Awake()
    {
        _myStartingPosition = transform;
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

            ///PROBLEM S OUT-OF-RANGE ELEMENTIMA MATRICE!!!
            
            /*if (nextDirection == _direction.Down)
            {
                _nextField = _theLevelManager._levelFieldMatrix[MyMatrixPosition.x, MyMatrixPosition.y - 1];
            }
            else if (nextDirection == _direction.Right)
            {
                _nextField = _theLevelManager._levelFieldMatrix[MyMatrixPosition.x, MyMatrixPosition.y - 1];
            }
            else if (nextDirection == _direction.Down)
            {
                _nextField = _theLevelManager._levelFieldMatrix[MyMatrixPosition.x, MyMatrixPosition.y - 1];
            }
            else if (nextDirection == _direction.Left)
            {
                _nextField = _theLevelManager._levelFieldMatrix[MyMatrixPosition.x, MyMatrixPosition.y - 1];
            }

            //Provjeri je li buldozer na toj lokaciji, ako je, pokreni njegov Move pa nastavi dalje
            if (_nextField.BuldozerPosition.GetComponentInChildren<Buldozer>() != null)
            {
                _nextField.BuldozerPosition.GetComponentInChildren<Buldozer>().Move();
            }

            //Provjeri je li home na toj lokaciji, ako je, svakako idi tamo
            //Provjeri je li šuma na toj lokaciji, ako je, svakako idi tamo

            //////////////////////////////Otkvači se s ove pozicije u matrici prema podatku o MyPosition: isprazni buldozer poziciju na ovoj lokaciji

            _theLevelManager._levelFieldMatrix[MyMatrixPosition.x, MyMatrixPosition.y].BuldozerOnMyField = null;
            transform.parent = _theLevelManager.GetBuldozersParent();
            //Postavi varijablu odakle krećeš kao svoj transform
            _myStartingPosition = transform;
            //Postavi varijablu next target iz buldozer pozicije tog susjednog polja iz matrice
            _myNextPosition = _nextField.BuldozerPosition;

            //Izbriši iz liste move patterna direction koji ti je bio odabran
            _myMovingPatternList.Remove(nextDirection);
            //Postavi bool varijablu koja je triger za Update funkciju
            TriggerMoving(true);

            //Update funkcija pomiče buldozer s trenutne pozicije na next poziciju
            //Kada je distance od transforma buldozera vrlo blizu target poziciji (>=0.01)
            //postavi transform buldozera točno prema transformu finalne pozicije
            //i stavi bool za Move u false.

            //Postavi svoj novi position iz buldozer pozicije ovog polja u matrici
            //Postavi se u buldozer poziciju ovog polja
            //Ako je ovdje šuma, pokreni Attack; ako nije, ponovno pokreni ovaj Move
            */
        }
    }

    private void Update()
    {
        if (_isMoving)
        {
            Move();
        }        
    }

    private void Move()
    {
        transform.position = Vector3.MoveTowards(_myStartingPosition.position, _myNextPosition.position, Time.deltaTime * MovingSpeed);

        if (Vector3.Distance(transform.position, _myNextPosition.position) <= 0.01)
        {
            transform.position = _myNextPosition.position;
            SetNewPosition();
            TriggerMoving(false);
        }
    }

    private void SetNewPosition()
    {
        _nextField.BuldozerOnMyField = this;
        transform.parent = _nextField.BuldozerPosition;
        MyMatrixPosition = _nextField.MyFieldPosition;

        StartMoving();
    }

    public virtual void Attack()
    {
        //Animiraj razaranje
    }

    public virtual void Death()
    {
        //Animiraj smrt
        Destroy(gameObject, 0.5f);
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
