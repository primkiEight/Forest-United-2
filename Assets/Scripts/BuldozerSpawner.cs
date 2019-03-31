﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuldozerSpawner : MonoBehaviour {

    private float _freq = 0.0f;

    private LevelManager _theLevelManager;

    private void Awake()
    {
        _theLevelManager = LevelManager.Instance;
        _freq = _theLevelManager.LevelData.TimeForTheFirstBuldozerSpawn;
    }

    public void StartSpawning()
    {
        StartCoroutine(CoSpawnBuldozer());
    }

    private void SpawnBuldozer(Buldozer buldozerToSpawn)
    {
        int randomIndex = Random.Range(0, _theLevelManager.GetBoundaryForestList.Count);

        Transform randomSpawnPosition = _theLevelManager.GetBoundaryForestList[randomIndex].BuldozerPosition;

        if(_theLevelManager.GetBoundaryForestList[randomIndex].BuldozerOnMyField != null)
        {
            //Ako pokušavam spawnati na istoj poziciji, makni ovoga, pa nastavi dalje sa spawnanjem
            //samo je bitno da ne zaboravim na Move() staiviti i čišćenje (nulliranje) BuldozerOnMyielda
            
        //_theLevelManager.GetBoundaryForestList[randomIndex].BuldozerOnMyField.Move();
        }

        if (_theLevelManager.GetBoundaryForestList[randomIndex].BuldozerOnMyField == null)
        {
            Buldozer buldozerClone = Instantiate(buldozerToSpawn, randomSpawnPosition.position, Quaternion.identity, _theLevelManager.GetBoundaryForestList[randomIndex].BuldozerPosition);

            _theLevelManager.GetBoundaryForestList[randomIndex].BuldozerOnMyField = buldozerClone;
        }
    }

    private IEnumerator CoSpawnBuldozer()
    {
        for (int i = 0; i < _theLevelManager.LevelData.NoOfBuldozersToSpawn; i++)
        {
            yield return new WaitForSeconds(_freq);

            int randomIndex = Random.Range(0, _theLevelManager.LevelData.LevelBuldozersList.Count);
            SpawnBuldozer(_theLevelManager.LevelData.LevelBuldozersList[randomIndex]);

            _freq = Random.Range(_theLevelManager.LevelData.BuldozerSpawnerMinNMax.x, _theLevelManager.LevelData.BuldozerSpawnerMinNMax.y);
        }        
    }


}
