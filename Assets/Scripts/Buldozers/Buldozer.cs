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

    private _direction nextDirection;

    private Transform _myTransform;
    private SpriteRenderer _mySpriteRenderer;

    private Animator _myAnimator;

    public Sprite MySprite;

    private AudioSource _myAudioSource;
    [Header("Audio")]
    public AudioClip AudioBuldoze;
    public AudioClip AudioMove;
    public AudioClip AudioDeath;

    private LevelManager _theLevelManager;
    private GameManager _theGameManager;

    [HideInInspector]
    public Vector2Int MyMatrixPosition;
    private Transform _myStartingPosition;
    private Field _thisField = null;
    private Field _nextField = null;
    private Transform _myNextPosition;
    private Vector2Int _myFinalDestination;

    private Trees _treesOnThisField = null;

    private List<Vector2Int> _forestHomesPositionsList = new List<Vector2Int>();
    private Field _nearestHome = null;
    private int _distanceX;
    private int _distanceY;
    private List<_direction> _myMovingPatternList = new List<_direction>();

    [HideInInspector]
    public bool ChangeOrderInLayer = true;
    
    private bool _isMoving = false;
    [Header("Movement")]
    [Range(0.1f, 0.5f)]
    public float MovingSpeed = 0.2f;
    private float _levelMovingSpeedModifier;
    private float _buldozingBreakTemp;
    private float _buldozingBreakPerma = 1.0f;
    [Range(4f, 10f)]
    public float BuldozingDuration = 5f;
    [Range(0.0f, 2.0f)]
    public float WaitOnTheEmptyField = 0f;

    [HideInInspector]
    public bool IsBroken = false;
    [HideInInspector]
    public bool IsSlowedDown = false;


    public void Awake()
    {
        _myTransform = transform;

        _myStartingPosition = _myTransform;

        if (GetComponent<Animator>() != null)
            _myAnimator = GetComponent<Animator>();

        if (GetComponent<SpriteRenderer>() != null)
        {
            _mySpriteRenderer = GetComponent<SpriteRenderer>();
            _mySpriteRenderer.sprite = MySprite;
        }

        _myAudioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _theLevelManager = LevelManager.Instance;

        _theGameManager = GameManager.Instance;

        _forestHomesPositionsList = _theLevelManager.LevelHomePositionsList;

        _thisField = _theLevelManager._levelFieldMatrix[MyMatrixPosition.x, MyMatrixPosition.y];

        IsBroken = false;

        _levelMovingSpeedModifier = _theLevelManager.LevelData.BuldozerMoveSpeedModifier;

        MovingSpeed *= _levelMovingSpeedModifier;

        _buldozingBreakTemp = _buldozingBreakPerma;

        StartMyEngines();
    }

    public void StartMyEngines()
    {
        if (_forestHomesPositionsList != null)
        {
            _myFinalDestination = GetNearestHomePosition();

            SetMyMovingPattern();
        }

        TriggerMoving(false);

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

        _nearestHome = _theLevelManager._levelFieldMatrix[theNearestHomePosition.x, theNearestHomePosition.y];

        return theNearestHomePosition;
    }

    private void SetMyMovingPattern()
    {
        _distanceX = MyMatrixPosition.x - _myFinalDestination.x;
        _distanceY = MyMatrixPosition.y - _myFinalDestination.y;

        if (_myMovingPatternList.Count > 0)
            _myMovingPatternList.Clear();

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
        ////?????? //_buldozingBreakTemp = _buldozingBreakPerma;
        _isMoving = trigger;
    }

    public virtual void StartMoving()
    {
        if (!IsBroken) {

            _myAudioSource.PlayOneShot(AudioMove);

            //Odaberi na random sljedeću lokaciju iz liste svojih next lokacija
            if (_myMovingPatternList.Count != 0)
            {
                int randomIndex = Random.Range(0, _myMovingPatternList.Count);
                //_direction nextDirection = _myMovingPatternList[randomIndex];
                nextDirection = _myMovingPatternList[randomIndex];

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
                if (_nextField.BuldozerOnMyField != null)
                {
                    StartCoroutine(CoWaitOnTheField(WaitOnTheEmptyField));
                    return;
                }
                else
                {
                    //Postavi varijablu odakle krećeš kao svoj transform
                    //_myStartingPosition = _myTransform;
                    _myStartingPosition = transform;
                    //Postavi varijablu next target iz buldozer pozicije tog susjednog polja iz matrice
                    _myNextPosition = _nextField.BuldozerPosition;

                    //Podešavam zajedničkog parenta dok se mičem
                    ///_myTransform.parent = _theLevelManager.GetBuldozersParent();

                    //Oslobađam ovo polje od buldožera
                    _theLevelManager._levelFieldMatrix[MyMatrixPosition.x, MyMatrixPosition.y].SetBuldozerOnMyField(null);

                    ////OVDJE POSTAVLJAM DA JE BULDOŽER NOVOG POLJA OVAJ BULDOŽER TIK PRIJE NEGO KRENE PREMA NOVOM POLJU
                    ////STAVIO SAM TO TAKO KAKO BI IZBJEGAO DA 2 BULDOŽERA BUDU NA ISTOM POLJU
                    ////Upitno je hoće li dobro funkcionirati kada stavim moći i životinje
                    //Dogodilo mi se da su 2 svejedno išla na isto polje u situaciji kada su se 2 spawnala jedan do drugog i oba krenula na isto polje
                    _nextField.SetBuldozerOnMyField(this);

                    //OVDJE POKUŠAJ UMJESTO U FIELD APSTRAKTNOJ SKRIPTI
                    _nextField.Casting();

                    //Izbriši iz liste move patterna direction koji ti je bio odabran
                    //_myMovingPatternList.Remove(nextDirection);

                    //Postavi bool varijablu koja je triger za Update funkciju za kretanje al prvo resetiraj brzinu na originalnu
                    //if(!IsBroken)
                    TriggerMoving(true);
                }
            }
        }
    }

    public void SetBuldozingBreak()
    {
        IsBroken = true;

        _myAnimator.SetBool("IsHalted", true);

        if (_thisField.TreesOnMyField != null)
        {
            Trees treesOnThisField = _thisField.TreesOnMyField.GetComponent<Trees>();
            treesOnThisField.StopBuldozingMe();
        }
    }

    public void SetBuldozingSpeed(float buldozingBreakDuration)
    {
        if (_buldozingBreakTemp == _buldozingBreakPerma)
        {
            IsSlowedDown = true;

            _myAnimator.SetBool("IsMovingSlowly", true);

            _buldozingBreakTemp = 0.5f;
            StartCoroutine(BreakBuldozerForAmountOfTimeCo(buldozingBreakDuration));
        }   
    }
    
    private IEnumerator BreakBuldozerForAmountOfTimeCo(float buldozingBreakDuration)
    {
        yield return new WaitForSeconds(buldozingBreakDuration);
        _buldozingBreakTemp = _buldozingBreakPerma;
        IsSlowedDown = false;

        _myAnimator.SetBool("IsMovingSlowly", false);
    }

    public void ReSetBuldozingBreak()
    {
        if(IsSlowedDown && !IsBroken)
        {
            
        } else if (!IsSlowedDown && IsBroken)
        {
            IsBroken = false;
            _myAnimator.SetBool("IsHalted", false);
            StartMyEngines();
        } else if (IsSlowedDown && IsBroken)
        {
            IsBroken = false;
            _myAnimator.SetBool("IsHalted", false);
            StartMyEngines();
        } else
        {

        }
    }
    
    //Update funkcija pomiče buldozer s trenutne pozicije na next poziciju
    private void Update()
    {
        if (_isMoving)
        {
            _myTransform.position = Vector3.MoveTowards(_myStartingPosition.position, _myNextPosition.position, Time.deltaTime * MovingSpeed * _buldozingBreakTemp);

            if(ChangeOrderInLayer)
                _mySpriteRenderer.sortingOrder = (int)(-(_myTransform.position.y * 100));

            float distance = Vector3.Distance(_myTransform.position, _myNextPosition.position);

            if ((distance <= 0.01f))
            {
                _myTransform.position = _myNextPosition.position;
                MyMatrixPosition = _nextField.MyFieldPosition;
                _myTransform.SetParent(_nextField.BuldozerPosition);
                _thisField = _theLevelManager._levelFieldMatrix[MyMatrixPosition.x, MyMatrixPosition.y];

                //_myMovingPatternList.Remove(nextDirection);

                if (IsBroken)
                {
                    TriggerMoving(false);
                } else if (!IsBroken)
                {
                    TriggerMoving(false);

                    _myMovingPatternList.Remove(nextDirection);

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

        StartMoving();
    }

    public IEnumerator CoAttackTreesOnThisField(Trees treesOnThisField)
    {
        //Animiraj rezanje
        treesOnThisField.StartBuldozingMe(this);
        yield return new WaitForSeconds(BuldozingDuration);

        _myAudioSource.PlayOneShot(AudioBuldoze);

        if(_nearestHome != null)
            _nearestHome.AnimateHomeEarthquake();

        StartMoving();
    }

    public virtual void Death()
    {
        _myAudioSource.PlayOneShot(AudioDeath);
        _myAnimator.SetTrigger("IsDestroyed");

        _theGameManager.BuldozerCountReduce();

        if (_thisField.TreesOnMyField != null)
        {
            Trees treesOnThisField = _thisField.TreesOnMyField.GetComponent<Trees>();
            treesOnThisField.StopBuldozingMe();
        }

        //StopAllCoroutines();
        Destroy(gameObject, 0.8f);
        //Destroy(gameObject);
    }
}
