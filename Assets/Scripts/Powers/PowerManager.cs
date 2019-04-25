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
    private Animator _myAC;

    private bool _doFill = true;

    private void Awake()
    {
        _myAC = MagicPool.GetComponent<Animator>();
    }

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
        if(_manaCurrent < _manaMax && _doFill)
        {
            _timer -= Time.deltaTime;
        
            if (_timer <= 0)
            {
                _manaCurrent += _fillUpPoints/60f * _fillUpRate;
                MagicPool.value = _manaCurrent;
                _timer = _fillUpRate;
            }
        } else if (_manaCurrent <= 0){
            _manaCurrent = 0;
            MagicPool.value = _manaCurrent;
        }

        /*if (MagicPool.value < _manaCurrent)
        {
            MagicPool.value += 0.2f;
            if(MagicPool.value > MagicPool.maxValue)
            {
                _manaCurrent = MagicPool.maxValue;
                MagicPool.value = MagicPool.maxValue;
            }

        } else */if (MagicPool.value > _manaCurrent)
        {
            _doFill = false;
            MagicPool.value -= 0.5f;
            if (MagicPool.value < MagicPool.minValue)
            {
                _manaCurrent = MagicPool.minValue;
                MagicPool.value = MagicPool.minValue;
            }
        } else if (MagicPool.value <= _manaCurrent)
        {
            _doFill = true;
        }


        //if (Input.GetMouseButtonDown(0))
        //{
        //    //_myAC.SetTrigger("Grow");
        //
        //    _manaCurrent -= 10;
        //
        //}
    }

    public void MagicPoolAdd (float powerToAdd) {

        float currentMana = _manaCurrent;

        if(currentMana + powerToAdd >= _manaMax)
        {
            _manaCurrent = _manaMax;
            //MagicPool.value = _manaCurrent;
            //return true;
        } else if (currentMana + powerToAdd < _manaMax) {
            _manaCurrent += powerToAdd;
            //MagicPool.value = _manaCurrent;
            //return true;
        }

        //return false;
	}

    public bool MagicPoolTake(float powerToTake)
    {
        if(powerToTake <= _manaCurrent)
        {
            _manaCurrent -= powerToTake;
            //MagicPool.value = _manaCurrent;
            return true;
        } else if (powerToTake > _manaCurrent){
            //Animiraj power slider
            Debug.Log("Nema dovoljno mane");
            _myAC.SetTrigger("Grow");
            return false;
        }

        return false;
    }

    
}
