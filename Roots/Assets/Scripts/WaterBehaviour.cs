using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBehaviour : MonoBehaviour
{
    [SerializeField] private float _growFactor = 0.1f; // Adjust this value to control the growth rate
    [SerializeField] private float _alphaFactor = 0.2f; // The initial value of the water material alpha
    [SerializeField] private float _evaporationSpeed = 1f;
    public float hydrationValue = 1f;
    public bool isGettingSucked = false;
    private float _initialHydrationValue = 1f;
    
    void Start() 
    {
        _initialHydrationValue = hydrationValue;
        AudioSource audio = GetComponent<AudioSource>();
        if (audio != null && audio.clip != null) 
        {
            audio.pitch = Random.Range(1f, 2f);
            audio.Play();
        }
            
    }

    void Update()
    {
        // Calculate the interpolation parameter using InverseLerp
        float alphaLerp = Mathf.InverseLerp(0f, _initialHydrationValue, hydrationValue);
        alphaLerp *= _alphaFactor;

        // Avoid flickering by using an epsilon value for comparison
        float epsilon = 0.001f;
        if (alphaLerp < epsilon) alphaLerp = 0f;

        // Change the alpha value of the object's material color
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            Material material = spriteRenderer.material;
            Color color = material.color;
            color.a = alphaLerp;
            material.color = color;
        }

        Vector3 scaleChange = new Vector3(_growFactor, _growFactor, _growFactor);
        transform.localScale += scaleChange * Time.deltaTime;
        hydrationValue -= (_growFactor * _evaporationSpeed) * Time.deltaTime;

        if (hydrationValue < 0f) 
            Destroy(gameObject);
    }
}
