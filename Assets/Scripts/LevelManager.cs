using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    [Header("This Level Prefabs")]
    public LevelData LevelData;

    [Header("Theme Prefabs")]
    public ThemeData ThemeData;

    //private Transform _myTransform;
    public Field[,] _levelFieldMatrix;

    private float _circleForAnimalsMin = 0.1f;
    private float _circleForHomeMin = 0.35f;
    private float _circleForHomeMax = 0.65f;
    private float _circleForAnimalsMax = 0.9f;

    [Header("UI ArrowPointers Data")]
    public Canvas PointerArrowsCanvas;
    public Camera UICamera;
    public ScreenArrowPointer PointerArrowPrefab;

    //[SerializeField]
    private List<Vector2Int> _circleHomeList = new List<Vector2Int> { };
    [HideInInspector]
    public List<Vector2Int> LevelHomePositionsList = new List<Vector2Int> { };
    //[SerializeField]
    private List<Vector2Int> _circleAnimalsList = new List<Vector2Int> { };
    //[SerializeField]
    private List<FieldForest> _circleBoundaryForestList = new List<FieldForest> { };
    public List<FieldForest> GetBoundaryForestList
    {
        get
        {
            return _circleBoundaryForestList;
        }
    }

    private BuldozerSpawner _buldozerSpawner;
    private GameObject _movingBuldozerParent;

    private CloudSpawner _cloudSpawner;
    private GameObject _movingCloudsParent;

    private TotemSpawner _totemSpawner;
    private List<FieldForest> _fieldForestList = new List<FieldForest> { };

    [HideInInspector]
    public PowerManager PowerManager;

    private GameManager _theGameManager;

    //Singleton
    private static LevelManager _instance;
    public static LevelManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (LevelManager)FindObjectOfType(typeof(LevelManager));

                if (_instance == null)
                    Debug.Log("An instance of InputManager doesn't exist!");
                //Sad ćemo napisati poruku da ne postoji
                //i da ga netko treba postaviti na scenu.
                //To nije konačno rješenje; konačno rješenje bi bilo
                //napraviti GameObject, dodati mu ovu skriptu, postaviti vrijednosti varijabli i vratimo taj objekt.
            }
            return _instance;
        }
    }

    private void Awake()
    {
        //Singleton
        if (Instance != this)
            Destroy(gameObject);

        //_myTransform = transform;        
        
        if (LevelData.Xmax < LevelData.Xmin)
            LevelData.Xmax = LevelData.Xmin;
        if (LevelData.Ymax < LevelData.Ymin)
            LevelData.Ymax = LevelData.Ymin;

        _circleForAnimalsMin = LevelData.CircleForAnimalsMin;
        _circleForHomeMin = LevelData.CircleForHomeMin;
        _circleForHomeMax = LevelData.CircleForHomeMax;
        _circleForAnimalsMax = LevelData.CircleForAnimalsMax;

        _movingBuldozerParent = new GameObject("MovingBuldozersParent");
        _movingBuldozerParent.transform.SetParent(transform);
        _buldozerSpawner = GetComponent<BuldozerSpawner>();

        _movingCloudsParent = new GameObject("CloudsParent");
        _movingCloudsParent.transform.SetParent(transform);
        _cloudSpawner = GetComponent<CloudSpawner>();

        _totemSpawner = GetComponent<TotemSpawner>();

        PowerManager = GetComponent<PowerManager>();

        CreateLevelFields();
    }

    private void Start()
    {
        _theGameManager = GameManager.Instance;

        int musicLibraryIndex = Random.Range(0, ThemeData.MusicLibraryList.Count);
        _theGameManager.PlayBackgroundMusic(ThemeData.MusicLibraryList[musicLibraryIndex]);

        _theGameManager.BuldozerCountSet(LevelData.NoOfBuldozersToSpawn);
    }

    public Transform GetBuldozersParent()
    {
        return _movingBuldozerParent.transform;
    }

    public Transform GetCloudsParent()
    {
        return _movingCloudsParent.transform;
    }
    
    private void CreateLevelFields()
    {
        _levelFieldMatrix = new Field[LevelData.Xmax +2, LevelData.Ymax +2];

        /*
        for (int i = LevelData.Xmax; i <= 19; i++)
        {
            int a = 1;
            int A = i;
            int b = (int)Mathf.Floor(i * 0.1f) + 1;
            int B = (int)Mathf.Round(i * 0.9f);
            int c = (int)Mathf.Floor(i * 0.35f) + 1;
            int C = (int)Mathf.Round(i * 0.65f);
            
            Debug.Log(i);
            Debug.Log(a + " " + A);
            Debug.Log(b + " " + B);
            Debug.Log(c + " " + C);
            Debug.Log("************************");
        }*/

        //CircleForHome
        int CircleForHomeXmin = (int)Mathf.Floor(LevelData.Xmax * _circleForHomeMin) + 1;
        int CircleForHomeXmax = (int)Mathf.Round(LevelData.Xmax * _circleForHomeMax);
        int CircleForHomeYmin = (int)Mathf.Floor(LevelData.Ymax * _circleForHomeMin) + 1;
        int CircleForHomeYmax = (int)Mathf.Round(LevelData.Ymax * _circleForHomeMax);

        //CircleNotForAnimals
        int CircleForAnimalsXmin = (int)Mathf.Floor(LevelData.Xmax * _circleForAnimalsMin) + 1;
        int CircleForAnimalsXmax = (int)Mathf.Round(LevelData.Xmax * _circleForAnimalsMax);
        int CircleForAnimalsYmin = (int)Mathf.Floor(LevelData.Ymax * _circleForAnimalsMin) + 1;
        int CircleForAnimalsYmax = (int)Mathf.Round(LevelData.Ymax * _circleForAnimalsMax);

        //Set Homes' and Animals' Domains
        for (int x = 1; x <= LevelData.Xmax; x++)
        {
            for (int y = 1; y <= LevelData.Ymax; y++)
            {
                //Domain for Homes
                if ((x >= CircleForHomeXmin) &&
                    (x <= CircleForHomeXmax) &&
                    (y >= CircleForHomeYmin) &&
                    (y <= CircleForHomeYmax))
                {
                    _circleHomeList.Add(new Vector2Int(x, y));
                }

                //Domain for Animals
                if ((x > CircleForAnimalsXmin) &&
                    (x <= CircleForAnimalsXmax) &&
                    (y > CircleForAnimalsYmin) &&
                    (y <= CircleForAnimalsYmax))
                {
                    _circleAnimalsList.Add(new Vector2Int(x, y));
                }
            }
        }

        //Build Forest
        GameObject ForestParent = new GameObject("ForestParent");
        ForestParent.transform.parent = transform;

        for (int x = 1; x <= LevelData.Xmax; x++)
        {
            for (int y = 1; y <= LevelData.Ymax; y++)
            {
                FieldForest ForestClone = Instantiate(LevelData.FieldForestPrefab, new Vector3(x, y, 0.0f), Quaternion.identity, ForestParent.transform);

                int ForestsListIndex = Random.Range(0, LevelData.LevelForestsList.Count);
                ForestClone.TreesOnMyField = Instantiate(LevelData.LevelForestsList[ForestsListIndex], ForestClone.TreesPosition.position, Quaternion.identity, ForestClone.TreesPosition);
                ForestClone.TreesOnMyField.SetAudioSource(ForestClone.MyAudioSource);

                _levelFieldMatrix[x, y] = ForestClone;

                //Set Fog of War on Forest fields
                if (LevelData.IncludeFogOfWar)
                {
                    ForestClone.FogOnMyField = Instantiate(LevelData.FogHolderPrefab, ForestClone.transform.position, Quaternion.identity, ForestClone.transform);
                }
            }
        }

        //Create Clouds
        if (LevelData.IncludeClouds)
            _cloudSpawner.StartSpawning();

        //Set Homes
        GameObject HomeParent = new GameObject("HomeParent");
        HomeParent.transform.parent = transform;

        if (LevelData.LevelHomeList.Count != 0)
        {
            for (int i = 0; i < LevelData.LevelHomeList.Count; i++)
            {
                if (_circleHomeList.Count == 0)
                    return;

                int ranIndex = Random.Range(0, _circleHomeList.Count);

                Vector2Int ranHomePosition = _circleHomeList[ranIndex];

                Destroy(_levelFieldMatrix[ranHomePosition.x, ranHomePosition.y].gameObject);
                _levelFieldMatrix[ranHomePosition.x, ranHomePosition.y] = null;

                FieldHome HomeClone = Instantiate(LevelData.LevelHomeList[i], new Vector3(ranHomePosition.x, ranHomePosition.y, 0.0f), Quaternion.identity, HomeParent.transform);

                HomeClone.MyFieldPosition = new Vector2Int(ranHomePosition.x, ranHomePosition.y);

                //Set UI ArrowPointers for Homes
                InstantiateUIPointerArrow(HomeClone.transform);
                
                //Adding the home positions to the list, for the buldozers
                LevelHomePositionsList.Add(HomeClone.MyFieldPosition);

                //Set Fog of War on Home fields
                if (LevelData.IncludeFogOfWar)
                {
                    HomeClone.FogOnMyField = Instantiate(LevelData.FogHolderPrefab, HomeClone.transform.position, Quaternion.identity, HomeClone.transform);
                }

                _levelFieldMatrix[ranHomePosition.x, ranHomePosition.y] = HomeClone;

                _circleAnimalsList.Remove(ranHomePosition);

                _circleHomeList.RemoveAt(ranIndex);
            }
        }

        _circleHomeList.Clear();
        
        //Set Animals
        if (LevelData.LevelAnimalsList.Count != 0)
        {
            for (int i = 0; i < LevelData.LevelAnimalsList.Count; i++)
            {
                if (_circleAnimalsList.Count == 0)
                    return;

                int ranIndex = Random.Range(0, _circleAnimalsList.Count);

                Vector2Int ranAnimalPosition = _circleAnimalsList[ranIndex];

                FieldForest thisForestField = _levelFieldMatrix[ranAnimalPosition.x, ranAnimalPosition.y].GetComponent<FieldForest>();

                Animal animalInTheHole = Instantiate(LevelData.LevelAnimalsList[i], thisForestField.AnimalPosition.position, Quaternion.identity, thisForestField.AnimalPosition);

                thisForestField.SetMound(true);

                _levelFieldMatrix[ranAnimalPosition.x, ranAnimalPosition.y].GetComponent<FieldForest>().AnimalInMyHole = animalInTheHole;

                _levelFieldMatrix[ranAnimalPosition.x, ranAnimalPosition.y].GetComponent<FieldForest>().IsAnimalHere = true;
                
                _circleAnimalsList.RemoveAt(ranIndex);
            }
        }

        _circleAnimalsList.Clear();

        //Set Totem Spawning Fields
        //Can not be a part of the Build Forest, since it is before setting of Homes,
        //and totems can not be spawned where homes are
        for (int x = 1; x <= LevelData.Xmax; x++)
        {
            for (int y = 1; y <= LevelData.Ymax; y++)
            {
                if (_levelFieldMatrix[x, y] is FieldForest)
                    _fieldForestList.Add((FieldForest)_levelFieldMatrix[x, y]);
            }
        }

        //Set Boundaries
        GameObject BoundaryParent = new GameObject("BoundaryParent");
        BoundaryParent.transform.parent = transform;

        for (int x = 0; x <= LevelData.Xmax + 1; x++)
        {
            for (int y = 0; y <= LevelData.Ymax + 1; y++)
            {
                if (_levelFieldMatrix[x,y] == null)
                {
                    FieldForest ForestClone = Instantiate(LevelData.FieldForestPrefab, new Vector3(x, y, 0.0f), Quaternion.identity, BoundaryParent.transform);

                    _levelFieldMatrix[x, y] = ForestClone;

                    Vector2Int boundaryPosition = new Vector2Int(x, y);
                    ForestClone.MyFieldPosition = boundaryPosition;
                    ForestClone.GetComponent<BoxCollider2D>().enabled = false;
                    Destroy(ForestClone.HolePosition.gameObject);
                    _circleBoundaryForestList.Add(ForestClone);
                }
            }
        }

        _buldozerSpawner.StartSpawning();

        _totemSpawner.StartSpawning(_fieldForestList);
    }

    private void InstantiateUIPointerArrow(Transform fieldHome)
    {
        ScreenArrowPointer PointerArrowClone = Instantiate(PointerArrowPrefab, PointerArrowsCanvas.transform);
        PointerArrowClone.SetTheHomeTargetAndUICamera(fieldHome, UICamera);
    }
}
