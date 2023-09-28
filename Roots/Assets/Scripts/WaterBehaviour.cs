using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBehaviour : MonoBehaviour
{
    [SerializeField] private float growFactor = 0.01f; // Adjust this value to control the growth rate
    public float hydrationValue = 5f;

    void Update()
    {
        Vector3 scaleChange = new Vector3(growFactor, growFactor, growFactor);
        transform.localScale += scaleChange * Time.deltaTime;
    }
}
