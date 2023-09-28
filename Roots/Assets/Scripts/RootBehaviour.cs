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
    void Start()
    {
        hasSpawnedNewCell = false;
        isHydrating = false;
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
            //StartCoroutine(SuckHydration(other.gameObject)); // Pass the "Water" object
            Debug.Log("Hydrating!");
        }
    }

    private IEnumerator SuckHydration(GameObject waterObject) // Accept a reference to the "Water" object 
    {
        while (hydrationAvailable > 0)
        {
            hydrationAvailable -= suckFactor;

            // Check hydrationAvailable here, after it has been updated
            if (hydrationAvailable <= 0)
            {
                // Destroy the "Water" object passed as a parameter
                Destroy(waterObject);
                isHydrating = false;
            }

            yield return new WaitForSeconds(1f);
        }
    }
}
