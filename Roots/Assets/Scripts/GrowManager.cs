using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GrowManager : MonoBehaviour
{
    [SerializeField] private GameObject _rootPrefab;
    [SerializeField] private float _growFactor = 1f;
    [SerializeField] private List<GameObject> _rootCellList = new List<GameObject>();
    private List<GameObject> _newCells = new List<GameObject>(); // List to store new cells to be added
    private bool _isGrowingNewCells = false;

    /*[SerializeField] private float rootHydration = 0f;
    [SerializeField] private float neededHydration = 1f; // can change in inspector
    [SerializeField] private float rootNutrition = 0f;
    [SerializeField] private float neededNutrition = 1f;*/ //MAY OR MAY NOT USE THIS

    [SerializeField] private bool _sufficientHydration = false;
    private bool _sufficientNutrition = false; // if both these bools are true for rootsystem, can span a new cell

    private float _currentAngle;
    private Vector3 _lastCellPosition = Vector3.zero;
    void Start()
    {
        // Instantiate the first rootCell GameObject
        if (_rootPrefab != null)
        {
            Vector3 randomPosition = GetRandomPosition();
            GameObject rootCell = Instantiate(_rootPrefab, randomPosition, Quaternion.identity);
            RootBehaviour rootBehaviour = rootCell.GetComponent<RootBehaviour>();
            rootBehaviour.hasSpawnedNewCell = false;
            _lastCellPosition = randomPosition;

            _rootCellList.Add(rootCell); // Add the root cell to the list
        }

        _currentAngle = Random.Range(0f, 360f);
    }

    void Update()
    {
        _newCells.Clear(); // Clear the list of new cells at the start of each update

        foreach (GameObject cell in _rootCellList)
        {
            RootBehaviour rootBehaviour = cell.GetComponent<RootBehaviour>();

            if (rootBehaviour.isHydrating)
            {
                _sufficientHydration = true;
                Debug.Log("Should hydrate now...");
            }
            // Check if there is sufficient hydration and nutrition to start growing cells
            if (_sufficientHydration && _sufficientNutrition)
            {
                // Ensure the coroutine is not already running to avoid starting it multiple times
                //if (!IsCoroutineRunning("GrowNewCells"))
                if (!_isGrowingNewCells)
                {
                    StartCoroutine(GrowNewCells());
                }
            }
            /*if (sufficientHydration && sufficientNutrition)
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
                        GameObject newCell = Instantiate(rootPrefab, randomPosition, Quaternion.identity);
                        RootBehaviour newRootBehaviour = newCell.GetComponent<RootBehaviour>();
                        newRootBehaviour.hasSpawnedNewCell = false;
                        rootBehaviour.hasSpawnedNewCell = true;
                        newCells.Add(newCell); // Add the newly instantiated cell to the list
                    }
                }
            }*/
        }

        // Add the new cells to the rootCellList after the loop
        _rootCellList.AddRange(_newCells);

        // if rootHydration >= neededHydration:
        _sufficientHydration = false;

        // if rootNutrition >= neededNutrition:
        _sufficientNutrition = true;
    }
    private bool IsCoroutineRunning(string methodName)
    {
        // Check if a coroutine with the specified method name is already running
        foreach (var coroutine in GetComponents(typeof(MonoBehaviour)))
        {
            if (coroutine.ToString().Contains(methodName))
            {
                return true;
            }
        }
        return false;
    }
    private IEnumerator GrowNewCells()
    {
        while (_sufficientHydration && _sufficientNutrition) // Loop as long as both conditions are met
        {
            foreach (GameObject cell in _rootCellList)
            {
                RootBehaviour rootBehaviour = cell.GetComponent<RootBehaviour>();

                if (!rootBehaviour.hasSpawnedNewCell)
                {
                    // Instantiate a new root cell and set hasSpawnedNewCell to true for this index
                    Vector3 randomPosition = GetRandomPosition();
                    if (randomPosition.x > 25 || randomPosition.x < -25 || randomPosition.y > 14 || randomPosition.y < -14)
                    {
                        randomPosition = Vector3.zero;
                        _lastCellPosition = randomPosition;
                    }
                    else
                    {
                        GameObject newCell = Instantiate(_rootPrefab, randomPosition, Quaternion.identity);
                        RootBehaviour newRootBehaviour = newCell.GetComponent<RootBehaviour>();
                        newRootBehaviour.hasSpawnedNewCell = false;
                        rootBehaviour.hasSpawnedNewCell = true;
                        _newCells.Add(newCell); // Add the newly instantiated cell to the list
                    }
                }
            }
            _isGrowingNewCells = true;
            yield return new WaitForSeconds(_growFactor);
        }
        _isGrowingNewCells = false;
    }

    Vector3 GetRandomPosition()
    {
        float minDistance = 1f; // Adjust this value to control the minimum distance between cells (the sphere diameter)

        // get a new angle that varies only slightly from the previous angle
        float angleVariance = Random.Range(-30f, 30f);
        float newAngle = _currentAngle + angleVariance;
        _currentAngle = newAngle; // set angle for next update

        float newRadAngle = newAngle * Mathf.Deg2Rad;

        Vector3 randomDirection = new Vector3(Mathf.Cos(newRadAngle), Mathf.Sin(newRadAngle), 0) * minDistance;
        Vector3 randomPosition = _lastCellPosition + randomDirection;
        _lastCellPosition = randomPosition;

        return randomPosition;
    }
}
