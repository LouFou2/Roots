using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSpawner : MonoBehaviour
{
    [SerializeField] private GameObject waterDropPrefab;
    private Vector3 spawnPosition = Vector3.zero;

    void Start()
    {
        StartCoroutine(SpawnWater());
    }

    private IEnumerator SpawnWater()
    {
        while (true) // This will keep the spawning process running indefinitely
        {
            float randomWaitTime = Random.Range(0.1f, 0.5f);
            float randomX = Random.Range(-25f, 25f);
            float randomY = Random.Range(-14f, 14f);
            spawnPosition = new Vector3(randomX, randomY, 0);
            yield return new WaitForSeconds(randomWaitTime);

            // Instantiate the water drop after the random wait time
            Instantiate(waterDropPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
