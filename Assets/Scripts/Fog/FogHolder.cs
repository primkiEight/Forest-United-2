using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogHolder : MonoBehaviour {

    [Header("Min and Max Local Scaling")]
    public Vector2 LocalScalingXMinNMax = Vector2.one;
    public Vector2 LocalScalingYMinNMax = Vector2.one;

    private List<Transform> myChildrenList = new List<Transform> { };

    private LevelManager _theLevelManager;

    private void Start()
    {
        _theLevelManager = LevelManager.Instance;
        List<Sprite> themeFogSpriteList = _theLevelManager.ThemeData.FogSpritesList;

        foreach (Transform child in transform)
        {
            float RandomScaleX = Random.Range(LocalScalingXMinNMax.x, LocalScalingXMinNMax.y);
            float RandomScaleY = Random.Range(LocalScalingYMinNMax.x, LocalScalingYMinNMax.y);

            int orientation = 1;
            if (child.localPosition.x < 0)
                orientation = -1;

            int ranIndex = Random.Range(0, themeFogSpriteList.Count);
            Sprite sprite = themeFogSpriteList[ranIndex];

            SpriteRenderer grandchildSpriteRenderer = child.GetComponentInChildren<SpriteRenderer>();

            grandchildSpriteRenderer.sprite = sprite;

            grandchildSpriteRenderer.transform.localPosition = new Vector3(RandomScaleX * orientation, RandomScaleY, grandchildSpriteRenderer.transform.localPosition.z);

            myChildrenList.Add(child.transform);
        }
    }

    public void AnimateFog(float positionX)
    {
        for (int i = 0; i < myChildrenList.Count; i++)
        {
            Animator _childAC = myChildrenList[i].GetComponent<Animator>();

            if (myChildrenList[i].transform.position.x >= positionX)
                _childAC.SetTrigger("FogDisappearsRight");
            else if ((myChildrenList[i].transform.position.x < positionX))
                _childAC.SetTrigger("FogDisappearsLeft");
        }
    }
}
