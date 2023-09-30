using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RootBehaviour : MonoBehaviour
{
    public bool hasSpawnedNewCell = false;
    public bool isHydrating = false;
    public float decayValue = 1f;
    [SerializeField] private float _suckFactor = 0.1f;
    [SerializeField] private float _decayTimer = 2f;
    [SerializeField] private float _decayFactor = 0.1f;
    [SerializeField] private float _alphaFactor = 0.5f;
    private float _initialDecayValue = 1f;
    private float _hydrationAvailable = 0f;
    
    void Start()
    {
        hasSpawnedNewCell = false;
        isHydrating = false;
        _initialDecayValue = 1f;
        decayValue = _initialDecayValue;
        StartCoroutine(LifeSpan());
        AudioSource audio = GetComponent<AudioSource>();
        if (audio != null && audio.clip != null) 
        {
            audio.pitch = Random.Range(1f, 2f);
            audio.Play();
        }
            
    }

    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water") && !isHydrating)
        {
            isHydrating = true;
            _hydrationAvailable = other.GetComponent<WaterBehaviour>().hydrationValue;
            StartCoroutine(SuckHydration(other.gameObject)); // Pass the "Water" object
        }
    }

    private IEnumerator SuckHydration(GameObject waterObject) // Accept a reference to the "Water" object 
    {
        bool waterIsGettingSucked = waterObject.GetComponent<WaterBehaviour>().isGettingSucked;
        if (waterObject != null && !waterIsGettingSucked) 
        {
            waterObject.GetComponent<WaterBehaviour>().isGettingSucked = true;
            while (_hydrationAvailable > 0 && waterObject != null)
            {
                _hydrationAvailable -= _suckFactor;
                waterObject.GetComponent<WaterBehaviour>().hydrationValue = _hydrationAvailable;

                yield return new WaitForSeconds(0.2f);
            }
            // Check hydrationAvailable here, after it has been updated
            if (_hydrationAvailable <= 0)
            {
                // Destroy the "Water" object passed as a parameter
                Destroy(waterObject);
                isHydrating = false;
            }
        }
    }

    private IEnumerator LifeSpan() 
    {
        while(decayValue > 0f) 
        {
            // Calculate the interpolation parameter using InverseLerp
            float alphaLerp = Mathf.InverseLerp(0f, _initialDecayValue, decayValue);
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
            decayValue -= _decayFactor;
            yield return new WaitForSeconds(_decayTimer);
        }
    }
}
