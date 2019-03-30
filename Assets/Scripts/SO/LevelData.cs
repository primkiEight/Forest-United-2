using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level Data", menuName = "Forest United/Level Data", order = 1)]
public class LevelData : ScriptableObject {

    /*public MiddleGround MiddleGroundPrefab;
    //[Range(5,10, order = 1)]
    public Vector2Int LevelQuadrantSize;
    */

    [HideInInspector]
    public int Xmin = 6;
    public int Xmax;
    [HideInInspector]
    public int Ymin = 6;
    public int Ymax;

    //[Range(1,2)]
    public Vector2 BuldozerMoveSpeed;
    //[Range(1, 2)]
    public Vector2 AnimalMoveSpeed;
    public FieldForest FieldForestPrefab;    

    public List<FieldHome> LevelHomeList= new List<FieldHome> {};
    public List<Animal> LevelAnimalsList = new List<Animal> { };
    public List<Buldozer> LevelBuldozersList = new List<Buldozer> { };
    public List<Trees> LevelTreesList = new List<Trees> { };
    

}
