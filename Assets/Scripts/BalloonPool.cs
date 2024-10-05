using System.Collections.Generic;
using UnityEngine;

public class BalloonPool : MonoBehaviour
{
    public GameObject balloonPrefab;
    public int initialPoolSize = 5; // Initial number of balloons (block of 5)
    private List<GameObject> balloonPool;
    private const int spawnBlockSize = 5; // We will add balloons in blocks of 5

    void Awake()
    {
        balloonPool = new List<GameObject>();
        PopulateBalloonPool(initialPoolSize); // Pre-populate the pool with balloons
        Debug.Log("BalloonPool initialized with " + initialPoolSize + " balloons.");
    }

    // Populate the pool with a specified number of balloons
    public void PopulateBalloonPool(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject balloon = Instantiate(balloonPrefab);
            balloon.SetActive(false); // Deactivate the balloon immediately after instantiating
            balloonPool.Add(balloon); // Add to the pool
        }
        Debug.Log($"{amount} balloons added to the pool.");
    }

    // Retrieve a balloon from the pool, or spawn new ones if necessary
    public GameObject GetBalloonFromPool()
    {
        foreach (GameObject balloon in balloonPool)
        {
            if (!balloon.activeInHierarchy)
            {
                Debug.Log("Reusing balloon from pool.");
                return balloon; // Reuse an inactive balloon
            }
        }

        // If no inactive balloons, add 5 more to the pool
        PopulateBalloonPool(spawnBlockSize);
        Debug.Log("No inactive balloons available. Adding 5 more to the pool.");
        return balloonPool[balloonPool.Count - spawnBlockSize]; // Return the first newly added balloon
    }
}
