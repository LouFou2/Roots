using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GrowManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> rootCellList = new List<GameObject>();
    private List<GameObject> newCells = new List<GameObject>(); // List to store new cells to be added
    [SerializeField] private float rootHydration = 0f;
    [SerializeField] private float neededHydration = 1f; // can change in inspector
    [SerializeField] private float rootNutrition = 0f;
    [SerializeField] private float neededNutrition = 1f;
    private bool sufficientHydration = false;
    private bool sufficientNutrition = false; // if both these bools are true for rootsystem, can span a new cell

    private float currentAngle;
    private Vector3 lastCellPosition = Vector3.zero;
    void Start()
    {
        // Instantiate the first rootCell GameObject
        if (rootCellList.Count > 0)
        {
            rootCellList.Add(rootCellList[0]);
            Vector3 randomPosition = GetRandomPosition();
            Instantiate(rootCellList[0], randomPosition, Quaternion.identity);
            rootCellList[0].GetComponent<RootBehaviour>().hasSpawnedNewCell = false;
            lastCellPosition = randomPosition;
        }

        currentAngle = Random.Range(0f, 360f);
    }

    void Update()
    {
        newCells.Clear(); // Clear the list of new cells at the start of each update

        foreach (GameObject cell in rootCellList)
        {
            RootBehaviour rootBehaviour = cell.GetComponent<RootBehaviour>();
            // Debug log to check the value of isHydrating
            Debug.Log("isHydrating for cell: " + rootBehaviour.isHydrating);

            if (rootBehaviour.isHydrating)
            {
                sufficientHydration = true;
                Debug.Log("Should hydrate now...");
            }

            if (sufficientHydration && sufficientNutrition)
            {
                if (!rootBehaviour.hasSpawnedNewCell)
                {
                    // Instantiate a new root cell and set hasSpawnedNewCell to true for this index
                    Vector3 randomPosition = GetRandomPosition();
                    if (randomPosition.x > 25 || randomPosition.x < -25 || randomPosition.y > 14 || randomPosition.y < -14)
                    {
                        randomPosition = Vector3.zero;
                        lastCellPosition = randomPosition;
                    }
                    else
                    {
                        GameObject newCell = Instantiate(cell, randomPosition, Quaternion.identity);
                        rootBehaviour.hasSpawnedNewCell = true;
                        newCells.Add(newCell); // Add the newly instantiated cell to the list
                    }
                }
            }
        }

        // Add the new cells to the rootCellList after the loop
        rootCellList.AddRange(newCells);

        // if rootHydration >= neededHydration:
        //sufficientHydration = true;

        // rootNutrition >= neededNutrition:
        sufficientNutrition = true;
    }
    Vector3 GetRandomPosition()
    {
        float minDistance = 0.5f; // Adjust this value to control the minimum distance between cells (half of the sphere diameter)

        // get a new angle that varies only slightly from the previous angle
        float angleVariance = Random.Range(-30f, 30f);
        float newAngle = currentAngle + angleVariance;
        currentAngle = newAngle; // set angle for next update

        float newRadAngle = newAngle * Mathf.Deg2Rad;

        Vector3 randomDirection = new Vector3(Mathf.Cos(newRadAngle), Mathf.Sin(newRadAngle), 0) * minDistance;
        Vector3 randomPosition = lastCellPosition + randomDirection;
        lastCellPosition = randomPosition;

        return randomPosition;
    }
}
