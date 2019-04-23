using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerManager : MonoBehaviour {

    public Slider MagicPool;
    private float _manaMax;
    private float _manaMin;
    private float _manaCurrent;

    [Header("Fill Up Rate (sec)")]
    public float _fillUpRate = 1.0f;
    private float _fillUpPoints;
    private float _timer = 0.0f;

    private LevelManager _theLevelManager;

    // Use this for initialization
	void Start () {
        _manaMax = MagicPool.maxValue;
        _manaMin = MagicPool.minValue;
        //MagicPool.value = _manaMax;
        _manaCurrent = MagicPool.value;

        _theLevelManager = GetComponent<LevelManager>();
        _fillUpPoints = _theLevelManager.LevelData.ManaFillUpSpeed;

        _timer = _fillUpRate;
    }

    private void Update()
    {
        if(_manaCurrent < _manaMax)
        {
            _timer -= Time.deltaTime;
        
            if (_timer <= 0)
            {
                _manaCurrent += _fillUpPoints/60f * _fillUpRate;
                MagicPool.value = _manaCurrent;
                _timer = _fillUpRate;
            }
        }
    }

    public void MagicPoolAdd (float powerToAdd) {

        float currentMana = _manaCurrent;

        if(currentMana + powerToAdd >= _manaMax)
        {
            _manaCurrent = _manaMax;
            MagicPool.value = _manaCurrent;
            //return true;
        } else if (currentMana + powerToAdd < _manaMax) {
            _manaCurrent += powerToAdd;
            MagicPool.value = _manaCurrent;
            //return true;
        }

        //return false;
	}

    public bool MagicPoolTake(float powerToTake)
    {
        if(powerToTake <= _manaCurrent)
        {
            _manaCurrent -= powerToTake;
            MagicPool.value = _manaCurrent;
            return true;
        } else if (powerToTake > _manaCurrent){
            //Animiraj power slider
            ////return false;






            return true;








        }

        return false;
    }


}
