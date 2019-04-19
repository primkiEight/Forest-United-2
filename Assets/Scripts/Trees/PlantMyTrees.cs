using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantMyTrees : MonoBehaviour {

    private LevelManager _theLevelManager;

    [Header("Tree Position Shifts")]
    public Vector2 ShiftRangeX = Vector2.zero;
    public Vector2 ShiftRangeY = Vector2.zero;
    [Header("Random Tree Hight Scale")]
    public Vector2 ScaleOnYMinAndMax = Vector2.one;
    private int[] _orientation = { -1, 1 };

    private void Awake()
    {
        _theLevelManager = LevelManager.Instance;

        Transform[] allChildren = GetComponentsInChildren<Transform>();

        foreach (Transform child in allChildren)
        {
            float ranX = Random.Range(ShiftRangeX.x, ShiftRangeX.y);
            float ranY = Random.Range(ShiftRangeY.x, ShiftRangeY.y);

            Vector3 treePosition = new Vector3(child.position.x + ranX, child.position.y + ranY, child.position.z);

            int ranIndex = Random.Range(0, _theLevelManager.ThemeData.TreesPrefabsList.Count);

            Tree treeClone = Instantiate(_theLevelManager.ThemeData.TreesPrefabsList[ranIndex], treePosition, Quaternion.identity, transform);

            float randomScale = Random.Range(ScaleOnYMinAndMax.x, ScaleOnYMinAndMax.y);
            int ranOIndex = Random.Range(0,2);
            int randomOrientation = _orientation[ranOIndex];

            Vector3 newRandomScale = new Vector3(treeClone.transform.localScale.x * randomOrientation, treeClone.transform.localScale.y * randomScale, treeClone.transform.localScale.z);
            treeClone.transform.localScale = newRandomScale;
        }
    }

    private void Start()
    {
        Destroy(this);
    }
}
