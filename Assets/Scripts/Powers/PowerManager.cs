using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerManager : MonoBehaviour {

    public Slider MagicPoolSlider;

    public Image MagicPoolSliderFill;
    private float _red;
    private float _green;
    private float _blue;
    private float _alpha;
    private Color _barColor;

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
        _myAC = MagicPoolSlider.GetComponent<Animator>();
    }

    // Use this for initialization
    void Start () {
        _manaMax = MagicPoolSlider.maxValue;
        _manaMin = MagicPoolSlider.minValue;
        MagicPoolSlider.value = _manaMax;
        _manaCurrent = MagicPoolSlider.value;

        _theLevelManager = GetComponent<LevelManager>();
        _fillUpPoints = _theLevelManager.LevelData.ManaFillUpSpeed;

        _red = 1 - (_manaCurrent / _manaMax);
        _green = 0f;
        _blue = _manaCurrent / _manaMax;
        _alpha = 1f;
        _barColor = MagicPoolSliderFill.color;

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

                MagicPoolSlider.value = _manaCurrent;
                SetBarColor();

                _timer = _fillUpRate;
            }
        } else if (_manaCurrent <= 0){
            _manaCurrent = _manaMin;
            MagicPoolSlider.value = _manaCurrent;
            SetBarColor();
        }

        /*if (MagicPool.value < _manaCurrent)
        {
            MagicPool.value += 0.2f;
            if(MagicPool.value > MagicPool.maxValue)
            {
                _manaCurrent = MagicPool.maxValue;
                MagicPool.value = MagicPool.maxValue;
            }

        } else */if (MagicPoolSlider.value > _manaCurrent)
        {
            _doFill = false;
            MagicPoolSlider.value -= 0.5f;
            SetBarColor();
            if (MagicPoolSlider.value < MagicPoolSlider.minValue)
            {
                _manaCurrent = MagicPoolSlider.minValue;
                MagicPoolSlider.value = MagicPoolSlider.minValue;
                SetBarColor();
            }
        } else if (MagicPoolSlider.value <= _manaCurrent)
        {
            _doFill = true;
        }

        /* TEST
        if (Input.GetMouseButtonDown(0))
        {
            //_myAC.SetTrigger("Grow");
            _manaCurrent -= 5;
        }

        if (Input.GetMouseButtonDown(1))
        {
            _manaCurrent += 5;
        }*/
    }

    public void MagicPoolAdd (float powerToAdd) {

        float currentMana = _manaCurrent;

        if(currentMana + powerToAdd >= _manaMax)
        {
            _manaCurrent = _manaMax;
            MagicPoolSlider.value = _manaCurrent;
            SetBarColor();
        } else if (currentMana + powerToAdd < _manaMax) {
            _manaCurrent += powerToAdd;
            MagicPoolSlider.value = _manaCurrent;
            SetBarColor();
        }
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
            //Debug.Log("Nema dovoljno mane");
            _myAC.SetTrigger("Grow");
            return false;
        }

        return false;
    }

    public void SetBarColor()
    {
        _red = 1 - (_manaCurrent / _manaMax);
        _blue = _manaCurrent / _manaMax;
        _barColor.r = _red;
        _barColor.g = _green;
        _barColor.b = _blue;
        _barColor.a = _alpha;
        MagicPoolSliderFill.color = _barColor;
    }
    
}
