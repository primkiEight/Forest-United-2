using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour {

    private float _freq = 0.0f;
    private int _offset = 2;

    private int _counter = 1;
    //public int StartingNoOfClouds = 10;
    public Vector3 ShadowOffset;
    public string ShadowsSortingLayerName;

    private LevelManager _theLevelManager;

    [Header("Clouds Layers' Names")]
    public List<string> CloudsLayersNamesList = new List<string> { };

    public void StartSpawning()
    {
        _theLevelManager = LevelManager.Instance;
        _freq = _theLevelManager.LevelData.TimeForTheFirstCloudSpawn;

        StartCoroutine(CoSpawnCloud());
    }

    private void SpawnCloud(Cloud cloudToSpawn)
    {
        int randomIndex = Random.Range(0, _theLevelManager.GetBoundaryForestList.Count);

        float randomSpawnPositionX = 0.0f;
        float randomSpawnPositionY = 0.0f;

        //if (_counter <= StartingNoOfClouds)
        //{
        //    randomSpawnPositionY = Random.Range(0.0f, (float)_theLevelManager.LevelData.Xmax);
        //} else
        //{
            randomSpawnPositionX = _theLevelManager.LevelData.Xmax + _offset;
        //}
        randomSpawnPositionY = Random.Range(0.0f, (float) _theLevelManager.LevelData.Ymax);

        Vector3 randomSpawnPosition = new Vector3(randomSpawnPositionX, randomSpawnPositionY, 0.0f);

        Cloud cloudClone = Instantiate(cloudToSpawn, randomSpawnPosition, Quaternion.identity, _theLevelManager.GetCloudsParent());

        float cloneCloudSpeed = Random.Range(_theLevelManager.LevelData.CloudMoveSpeed.x, _theLevelManager.LevelData.CloudMoveSpeed.y) * 0.1f;

        cloudClone.SetCloudSpeed(cloneCloudSpeed);
        int cloneCloudDestroyPosition = -_offset;
        cloudClone.SetDestroyPosition(cloneCloudDestroyPosition);

        int ranIndex = Random.Range(1, CloudsLayersNamesList.Count);
        cloudClone.SetLayerMask(CloudsLayersNamesList[ranIndex]);

        cloudClone.CreateShadow(CloudsLayersNamesList[0], ShadowOffset, ShadowsSortingLayerName);

        cloudClone.StartMoving();

        StartCoroutine(CoSpawnCloud());
    }

    private IEnumerator CoSpawnCloud()
    {
        yield return new WaitForSeconds(_freq);

        int randomIndex = Random.Range(0, _theLevelManager.LevelData.LevelCloudsList.Count);

        SpawnCloud(_theLevelManager.LevelData.LevelCloudsList[randomIndex]);

        _freq = Random.Range(_theLevelManager.LevelData.CloudSpawnerMinNMax.x, _theLevelManager.LevelData.CloudSpawnerMinNMax.y);        
    }
}
