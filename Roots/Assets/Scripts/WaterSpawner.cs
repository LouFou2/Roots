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
            float randomWaitTime = Random.Range(0.5f, 5f);

            float spawnRadius = Random.Range(0, 12f);
            float randomAngle = Random.Range(0f, 2f * Mathf.PI); // Choose a random angle in radians
            float spawnX = spawnRadius * Mathf.Cos(randomAngle); // Calculate x position using polar coordinates
            float spawnY = spawnRadius * Mathf.Sin(randomAngle); // Calculate y position using polar coordinates
            spawnPosition = new Vector3(spawnX, spawnY, 0);

            yield return new WaitForSeconds(randomWaitTime);

            // Instantiate the water drop after the random wait time
            Instantiate(waterDropPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
