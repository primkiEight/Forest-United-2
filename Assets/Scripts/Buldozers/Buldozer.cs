using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Buldozer : MonoBehaviour {

    protected Field[,] _LevelFields;
    public virtual void Initialize(Field[,] levelFields)
    {
        _LevelFields = levelFields;
    }

    private Vector2 _myStartingPosition;
    //[HideInInspector]
    public Vector2Int MyPosition;
    private Vector2Int _myNextPosition;
    private Vector2Int _myFinalDestination;

    private List<Vector2Int> _forestHomesPositionsList = new List<Vector2Int>();
    private int _distanceX;
    private int _distanceY;
    private List<int> _myMovingPatternList = new List<int>();

    public Sprite MySprite;

    public float MovingSpeed;

    public float BuldozingDuration;
    
    private Animator _myAnimator;

    public AudioClip AudioBuldoze;
    public AudioClip AudioMove;
    public AudioClip AudioDeath;

    private LevelManager _theLevelManager;
    
    public void Awake()
    {
        _myStartingPosition = transform.position;
        MyPosition = new Vector2Int ((int)_myStartingPosition.x, (int)_myStartingPosition.y);

        if (GetComponent<Animator>() != null)
            _myAnimator = GetComponent<Animator>();

        if (GetComponent<SpriteRenderer>() != null)
            GetComponent<SpriteRenderer>().sprite = MySprite;

        _theLevelManager = LevelManager.Instance;
    }

    public void Start()
    {
        _forestHomesPositionsList = _theLevelManager.LevelHomePositionsList;
        if (_forestHomesPositionsList != null)
        {
            _myFinalDestination = GetNearestHomePosition();
        }

        SetMyMovingPattern();
    }

    public Vector2Int GetNearestHomePosition()
    {
        Vector2Int theNearestHomePosition = _forestHomesPositionsList[0];

        float distance = Vector2.Distance(_myStartingPosition, theNearestHomePosition);

        for (int i = 1; i < _forestHomesPositionsList.Count; i++)
        {
            if (Vector2.Distance(_myStartingPosition, _forestHomesPositionsList[i]) < distance)
                theNearestHomePosition = _forestHomesPositionsList[i];
        }    

        return theNearestHomePosition;
    }

    private void SetMyMovingPattern()
    {
        _distanceX = MyPosition.x - _myFinalDestination.x;
        _distanceY = MyPosition.y - _myFinalDestination.y;

        int posX;
        int posY;
        int negX;
        int negY;
    }

    public virtual void Move()
    {
        //Animiraj kretnju
        
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
