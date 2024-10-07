using System.Collections.Generic;
using UnityEngine;

// Implementation of IBalloonPool with a maximum of 10 balloons
public class BalloonPool : MonoBehaviour, IBalloonPool
{
    public GameObject balloonPrefab;
    public int initialPoolSize = 5; // Initial number of balloons
    private List<GameObject> balloonPool;
    private const int maxPoolSize = 10; // Maximum number of balloons allowed

    void Awake()
    {
        balloonPool = new List<GameObject>();
        PopulateBalloonPool(initialPoolSize); // Pre-populate the pool
    }

    // Populate the pool with a specified number of balloons, but don't exceed maxPoolSize
    public void PopulateBalloonPool(int amount)
    {
        int newBalloonCount = Mathf.Min(amount, maxPoolSize - balloonPool.Count); // Ensure we don't exceed the max size

        for (int i = 0; i < newBalloonCount; i++)
        {
            GameObject balloon = Instantiate(balloonPrefab);
            balloon.SetActive(false); // Deactivate the balloon immediately after instantiating
            balloonPool.Add(balloon); // Add to the pool
        }
    }

    // Retrieve a balloon from the pool, ensuring no more than maxPoolSize exists
    public GameObject GetBalloonFromPool()
    {
        // Try to find an inactive balloon in the pool
        foreach (GameObject balloon in balloonPool)
        {
            if (!balloon.activeInHierarchy)
            {
                return balloon; // Reuse the inactive balloon
            }
        }

        // If no inactive balloons are available and we're below the maximum pool size, populate more
        if (balloonPool.Count < maxPoolSize)
        {
            PopulateBalloonPool(maxPoolSize - balloonPool.Count); // Populate remaining slots up to the max size
        }

        // Return the first balloon from the newly added balloons, if any were added
        if (balloonPool.Count > 0)
        {
            return balloonPool[balloonPool.Count - 1]; // Return the last balloon added to the pool
        }

        Debug.LogError("BalloonPool has reached the maximum size of " + maxPoolSize + " and no inactive balloons are available.");
        return null; // No balloons available
    }
}
