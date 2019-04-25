using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour {

    private float _freq = 0.0f;
    private int _offset = 2;

    public Vector3 ShadowOffset;
    public string ShadowsSortingLayerName;

    private LevelManager _theLevelManager;

    [Header("Clouds Layers' Names")]
    public List<string> CloudsLayersNamesList = new List<string> { };

    public void StartSpawning()
    {
        _theLevelManager = LevelManager.Instance;
        _freq = _theLevelManager.LevelData.TimeForTheFirstCloudSpawn;

        SpawnStartingClouds();

        StartCoroutine(CoSpawnCloud());
    }

    private void MakeACloud(float randomSpawnPositionX, float randomSpawnPositionY)
    {
        int randomIndex = Random.Range(0, _theLevelManager.LevelData.LevelCloudsList.Count);

        Cloud cloudToSpawn = (_theLevelManager.LevelData.LevelCloudsList[randomIndex]);

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
    }

    private void SpawnStartingClouds()
    {
        int min = Mathf.Min(_theLevelManager.LevelData.Xmax, _theLevelManager.LevelData.Ymax);
        int max = Mathf.Max(_theLevelManager.LevelData.Xmax, _theLevelManager.LevelData.Ymax);
        int length = Random.Range(min, max);

        for (int i = 0; i < length; i++)
        {
            float randomSpawnPositionX = Random.Range(0.0f, (float)_theLevelManager.LevelData.Xmax);
            float randomSpawnPositionY = Random.Range(0.0f, (float)_theLevelManager.LevelData.Ymax);

            MakeACloud(randomSpawnPositionX, randomSpawnPositionY);
        }
    }

    private void SpawnCloud()
    {
        int randomIndex = Random.Range(0, _theLevelManager.GetBoundaryForestList.Count);

        float randomSpawnPositionX = _theLevelManager.LevelData.Xmax + _offset;
        float randomSpawnPositionY = Random.Range(0.0f, (float)_theLevelManager.LevelData.Ymax);
        
        MakeACloud(randomSpawnPositionX, randomSpawnPositionY);

        StartCoroutine(CoSpawnCloud());
    }

    private IEnumerator CoSpawnCloud()
    {
        yield return new WaitForSeconds(_freq);

        SpawnCloud();

        _freq = Random.Range(_theLevelManager.LevelData.CloudSpawnerMinNMax.x, _theLevelManager.LevelData.CloudSpawnerMinNMax.y);        
    }
}
