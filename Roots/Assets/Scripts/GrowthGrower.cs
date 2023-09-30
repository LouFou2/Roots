using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowthGrower : MonoBehaviour
{
    [SerializeField] private int _growthAmount = 0;
    [SerializeField] private GrowManager _growManager;
    [SerializeField] private float _maxRootCellAmount = 200f;
    [SerializeField] private float _maxGrowthScale = 17f;
    void Start()
    {
        if (_growManager != null)
            _growthAmount = _growManager.growthAmount;
    }

    // Update is called once per frame
    void Update()
    {
        if (_growManager != null)
            _growthAmount = _growManager.growthAmount;

        float growthScale = Mathf.InverseLerp(0f, _maxRootCellAmount, (float)_growthAmount);
        growthScale *= _maxGrowthScale; 
        transform.localScale = new Vector3(growthScale, growthScale, growthScale);
    }
}
