using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBehaviour : MonoBehaviour
{
    [SerializeField] private float _growFactor = 0.1f; // Adjust this value to control the growth rate
    [SerializeField] private float _alphaFactor = 0.2f; // The initial value of the water material alpha
    public float hydrationValue = 1f;
    private float _initialHydrationValue = 1f;
    
    void Start() 
    {
        _initialHydrationValue = hydrationValue;
    }

    void Update()
    {
        // Calculate the interpolation parameter using InverseLerp
        float alphaLerp = Mathf.InverseLerp(0f, _initialHydrationValue, hydrationValue);
        alphaLerp *= _alphaFactor;

        // Change the alpha value of the object's material color
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            Material material = meshRenderer.material;
            Color color = material.color;
            color.a = alphaLerp;
            material.color = color;
        }

        Vector3 scaleChange = new Vector3(_growFactor, _growFactor, _growFactor);
        transform.localScale += scaleChange * Time.deltaTime;
    }
}
