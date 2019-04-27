using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotemSpawner : MonoBehaviour {

    private float _freq = 0.0f;
    
    private LevelManager _theLevelManager;
    private List<FieldForest> _fieldForestList = new List<FieldForest> { };

    public void StartSpawning(List<FieldForest> fieldForestList)
    {
        _theLevelManager = LevelManager.Instance;
        _fieldForestList = fieldForestList;
        _freq = _theLevelManager.LevelData.TimeForTheFirstTotemSpawn;

        StartCoroutine(CoSpawnTotem());
    }
    
    private void SpawnTotem()
    {
        int ranTotemIndex = Random.Range(0, _theLevelManager.ThemeData.TotemPrefabsList.Count);

        Totem totemToSpawn = (_theLevelManager.ThemeData.TotemPrefabsList[ranTotemIndex]);

        FieldForest fieldToSpawnTotem = null;

        do
        {
            int ranFieldIndex = Random.Range(0, _fieldForestList.Count);
            fieldToSpawnTotem = _fieldForestList[ranFieldIndex];
        } while (fieldToSpawnTotem.InstantiateTotemHere(totemToSpawn));

        //Pronađi odgovarajući fieldforest u kojem već nema totema i u kojemu već nema životinje
        //Pozovi funkciju za instanciranje totema na tom fieldu, prenesi mu ovaj totem
        StartCoroutine(CoSpawnTotem());
    }

    private IEnumerator CoSpawnTotem()
    {
        yield return new WaitForSeconds(_freq);

        _freq = Random.Range(_theLevelManager.LevelData.TotemSpawnerMinNMax.x, _theLevelManager.LevelData.TotemSpawnerMinNMax.y);

        SpawnTotem();
    }
}
