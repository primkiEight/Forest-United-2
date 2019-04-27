using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldHome : Field {

    private SpriteRenderer _mySprite;

    [Header("FieldHome Transforms")]
    public Transform TreeBottom;
    public Transform TreeBottomBackground;

    [Header("Animator Controller")]
    public Animator HomeAnimator;

     private void Awake()
    {
        _mySprite = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        _theLevelManager = LevelManager.Instance;

        if (_mySprite)
        {
            SetMyBackground(_mySprite, _theLevelManager.ThemeData);

            if(TreeBottom.gameObject.activeSelf != true)
            {
                TreeBottom.gameObject.SetActive(true);
            }

            SpriteRenderer treeBottomSprite = TreeBottomBackground.GetComponent<SpriteRenderer>();
            SetMyBackground(treeBottomSprite, _theLevelManager.ThemeData);
            TreeBottom.gameObject.SetActive(false);
        }
    }

    public override void AnimateHomeEarthquake()
    {
        if(HomeAnimator.isActiveAndEnabled == true)
        {
            HomeAnimator.SetTrigger("HomeEarthquake");
        }
    }

    public override void SetBuldozerOnMyField(Buldozer buldozerOnMyField)
    {
        BuldozerOnMyField = buldozerOnMyField;

        if (BuldozerOnMyField != null)
        {
            //Pokreni GameOver

            //SpriteRenderer buldozerSprite = BuldozerOnMyField.GetComponent<SpriteRenderer>();
            //buldozerSprite.sortingLayerName = TreeBottom.GetComponent<SpriteRenderer>().sortingLayerName;
            //BuldozerOnMyField.ChangeOrderInLayer = false;
            //buldozerSprite.sortingOrder = 2;
            //if (TreeBottom.gameObject.activeSelf != true)
            //{
            //    TreeBottom.gameObject.SetActive(true);
            //}

            AnimateHomeEarthquake();

            Debug.Log("GameOver");
        } else if (BuldozerOnMyField == null)
        {
            //Ne bi se nikada trebalo dogoditi            
        }
    }
}
