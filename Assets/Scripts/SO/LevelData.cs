using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level Data", menuName = "Forest United/Level Data", order = 1)]
public class LevelData : ScriptableObject {

    [Header("Level Size (min = 6)")]
    public int Xmax;
    public int Ymax;
    [HideInInspector]
    public int Xmin = 6;
    [HideInInspector]
    public int Ymin = 6;

    [Header("Level Spawning Areas [0.10, 0.35, 0.65, 0.90]")]
    [Range(0.1f, 0.35f)]
    public float CircleForAnimalsMin = 0.1f;
    [Range(0.1f, 0.35f)]
    public float CircleForHomeMin = 0.35f;
    [Range(0.65f, 0.9f)]
    public float CircleForHomeMax = 0.65f;
    [Range(0.65f, 0.9f)]
    public float CircleForAnimalsMax = 0.9f;

    [Header("Buldozer Options")]
    [Range(1f,1.5f)]
    public float BuldozerMoveSpeedModifier = 1f;
    public Vector2 BuldozerSpawnerMinNMax;
    public float TimeForTheFirstBuldozerSpawn;
    public int NoOfBuldozersToSpawn;

    [Header("Cloud Options")]
    public bool IncludeClouds;
    public Vector2 CloudMoveSpeed;
    public Vector2 CloudSpawnerMinNMax;
    public float TimeForTheFirstCloudSpawn;

    [Header("Animal Options")]
    //[Range(1, 2)]
    public Vector2 AnimalMoveSpeed;

    [Header("Power Options (pts/min)")]
    [Range(1f, 100f)]
    public float ManaFillUpSpeed = 100f;

    [Header("Totem Options (app in sec)")]
    public float TimeForTheFirstTotemSpawn;
    public Vector2 TotemSpawnerMinNMax;

    [Header("Field Prefab")]
    public FieldForest FieldForestPrefab;

    [Header("Fog")]
    public bool IncludeFogOfWar;
    public FogHolder FogHolderPrefab;

    //[Header("Field Sprites")]
    //public List<Sprite> FieldSpritesList = new List<Sprite> { };
    //
    [Header("Number and types of Homes")]
    public List<FieldHome> LevelHomeList= new List<FieldHome> {};
    [Header("Number and types of Animals")]
    public List<Animal> LevelAnimalsList = new List<Animal> { };
    [Header("Number and types of Buldozers")]
    public List<Buldozer> LevelBuldozersList = new List<Buldozer> { };
    [Header("Types of Forest Formations")]
    public List<Trees> LevelForestsList = new List<Trees> { };
    [Header("Type of Clouds")]
    public List<Cloud> LevelCloudsList = new List<Cloud> { };
    //[Header("Type of Fogs")]
    //public List<Sprite> LevelFogSpriteList = new List<Sprite> { };
}
