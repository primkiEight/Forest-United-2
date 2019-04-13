using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour {

    private float _freq = 0.0f;
    private int _offset = 2;

    private LevelManager _theLevelManager;

    public void StartSpawning()
    {
        _theLevelManager = LevelManager.Instance;
        _freq = _theLevelManager.LevelData.TimeForTheFirstCloudSpawn;

        StartCoroutine(CoSpawnCloud());
    }

    private void SpawnCloud(Cloud cloudToSpawn)
    {
        int randomIndex = Random.Range(0, _theLevelManager.GetBoundaryForestList.Count);

        float randomSpawnPositionX = _theLevelManager.LevelData.Xmax + _offset;
        float randomSpawnPositionY = Random.Range(0.0f, (float) _theLevelManager.LevelData.Ymax);

        Vector3 randomSpawnPosition = new Vector3(randomSpawnPositionX, randomSpawnPositionY, 0.0f);

        Cloud cloudClone = Instantiate(cloudToSpawn, randomSpawnPosition, Quaternion.identity, _theLevelManager.GetCloudsParent());

        float cloneCloudSpeed = Random.Range(_theLevelManager.LevelData.CloudMoveSpeed.x, _theLevelManager.LevelData.CloudMoveSpeed.y) * 0.1f;

        cloudClone.SetCloudSpeed(cloneCloudSpeed);
        int cloneCloudDestroyPosition = -_offset;
        cloudClone.SetDestroyPosition(cloneCloudDestroyPosition);
        ////cloudClone.SetLayerMask();
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
