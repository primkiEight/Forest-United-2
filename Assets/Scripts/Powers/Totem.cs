using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Totem : MonoBehaviour {

    [Header("Totem Values")]
    [Range(10.0f, 100.0f)]
    public float TotemManaValue = 10.0f;
    [Range(10.0f, 60.0f)]
    public float TotemLifeTime = 10.0f;

    [HideInInspector]
    public Animator TotemStoneAnimator;
    
    private FieldForest _myFieldForest = null;

    private void Awake()
    {
        TotemStoneAnimator = GetComponent<Animator>();
    }

    public void SetMyFieldForest(FieldForest myFieldForest)
    {
        _myFieldForest = myFieldForest;

        Invoke("DestroyTotem", TotemLifeTime);
    }

    public void DestroyTotem()
    {
        _myFieldForest.ActivateTotem();
    }
}
