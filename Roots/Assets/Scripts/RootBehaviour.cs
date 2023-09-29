using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RootBehaviour : MonoBehaviour
{
    public bool hasSpawnedNewCell = false;
    public bool isHydrating = false;
    private float hydrationAvailable = 0f;
    [SerializeField] private float suckFactor = 0.1f;
    [SerializeField] private float _decayIncrement = 0.1f;
    [SerializeField] private float _decayFactor = 0.1f;
    [SerializeField] private float _initialDecayValue = 1f;
    public float decayValue = 1f;
    void Start()
    {
        hasSpawnedNewCell = false;
        isHydrating = false;
        _initialDecayValue = 1f;
        decayValue = _initialDecayValue;
        StartCoroutine(LifeSpan());
    }

    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isHydrating = true;
            hydrationAvailable = other.GetComponent<WaterBehaviour>().hydrationValue;
            StartCoroutine(SuckHydration(other.gameObject)); // Pass the "Water" object
            Debug.Log("Hydrating!");
        }
    }

    private IEnumerator SuckHydration(GameObject waterObject) // Accept a reference to the "Water" object 
    {
        while (hydrationAvailable > 0)
        {
            hydrationAvailable -= suckFactor;
            waterObject.GetComponent<WaterBehaviour>().hydrationValue = hydrationAvailable;

            yield return new WaitForSeconds(0.2f);
        }
        // Check hydrationAvailable here, after it has been updated
        if (hydrationAvailable <= 0)
        {
            // Destroy the "Water" object passed as a parameter
            Destroy(waterObject);
            isHydrating = false;
        }
    }

    private IEnumerator LifeSpan() 
    {
        while(decayValue > 0f) 
        {
            // Calculate the interpolation parameter using InverseLerp
            float alphaLerp = Mathf.InverseLerp(0f, _initialDecayValue, decayValue);

            // Change the alpha value of the object's material color
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                Material material = meshRenderer.material;
                Color color = material.color;
                color.a = alphaLerp;
                material.color = color;
            }
            decayValue -= _decayFactor;
            yield return new WaitForSeconds(_decayIncrement);
        }
    }
}
