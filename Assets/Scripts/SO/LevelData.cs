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

    [Header("Buldozer Options")]
    //[Range(1,2)]
    public Vector2 BuldozerMoveSpeed;
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

    [Header("Field Prefab")]
    public FieldForest FieldForestPrefab;

    [Header("Field Prefab")]
    public List<Sprite> FieldSpritesList = new List<Sprite> { };

    [Header("Number and types of Homes")]
    public List<FieldHome> LevelHomeList= new List<FieldHome> {};
    [Header("Number and types of Animals")]
    public List<Animal> LevelAnimalsList = new List<Animal> { };
    [Header("Number and types of Buldozers")]
    public List<Buldozer> LevelBuldozersList = new List<Buldozer> { };
    [Header("Types of Forests")]
    public List<Trees> LevelTreesList = new List<Trees> { };
    [Header("Types of Clouds")]
    public List<Cloud> LevelCloudsList = new List<Cloud> { };
}
