using System;
using UnityEngine;

// Manager for handling balloon spawning and tracking
public class BalloonManager : MonoBehaviour
{
    public BalloonPool pool;
    public IBalloonPool balloonPool; // Reference to the balloon pool
    public event Action OnBalloonDeactivated; // Event triggered when a balloon is deactivated

    public int maxBalloonsInScene = 15;
    private int currentBalloonsInScene = 0;

    [Header("Spawn Area")]
    public Vector2 minSpawnArea = new Vector2(-5f, -4f); // Minimum spawn area
    public Vector2 maxSpawnArea = new Vector2(5f, 4f);   // Maximum spawn area

    [Header("Common X Point")]
    public float commonXPoint = 0f;
    public float moreminuscommon = 2f;

    void Start()
    {
        balloonPool = pool;
        // Check for missing balloonPool assignment
        if (balloonPool == null)
        {
            Debug.LogError("BalloonPool reference is not assigned in BalloonManager. Please assign it in the Inspector.");
        }
    }

    // Method to spawn balloons for the current level
    public void SpawnBalloons(int count, float screenMinX, float screenMaxX)
    {
        if (balloonPool == null)
        {
            Debug.LogError("BalloonPool is not assigned. Cannot spawn balloons.");
            return; // Early return to prevent null reference exception
        }

        ClearActiveBalloons();

        // Set a new random common X point within the screen limits
        commonXPoint = UnityEngine.Random.Range(screenMinX, screenMaxX);

        int balloonsToSpawn = Math.Min(count, maxBalloonsInScene);

        // Spawn balloons
        for (int i = 0; i < balloonsToSpawn; i++)
        {
            GameObject balloon = balloonPool.GetBalloonFromPool();
            if (balloon == null)
            {
                Debug.LogError("Failed to retrieve balloon from pool.");
                continue; // Skip this iteration if the balloon is not available
            }

            // Generate random Y position within the spawn area
            float randomY = UnityEngine.Random.Range(minSpawnArea.y, maxSpawnArea.y);
            float randomX = Mathf.Clamp(
                UnityEngine.Random.Range(commonXPoint - moreminuscommon, commonXPoint + moreminuscommon),
                screenMinX, screenMaxX);

            balloon.transform.position = new Vector2(randomX, randomY);
            balloon.SetActive(true);

            // Initialize balloon movement
            IBalloonMovement balloonMovement = balloon.GetComponent<IBalloonMovement>();
            if (balloonMovement != null)
            {
                balloonMovement.Initialize(commonXPoint, screenMinX, screenMaxX);

                // Register for the OnBalloonDestroyed event
                balloonMovement.OnBalloonDestroyed -= HandleBalloonDeactivation; // Unsubscribe first to avoid duplicates
                balloonMovement.OnBalloonDestroyed += HandleBalloonDeactivation;
            }
            else
            {
                Debug.LogWarning("Balloon does not have a BalloonMovement component.");
            }
        }

        currentBalloonsInScene = balloonsToSpawn;
        Debug.Log($"Spawned {balloonsToSpawn} balloons.");
    }

    // Handle balloon deactivation
    private void HandleBalloonDeactivation()
    {
        currentBalloonsInScene--;
        Debug.Log($"Balloon deactivated, {currentBalloonsInScene} remaining.");

        // Trigger the event to notify the BalloonSpawner
        OnBalloonDeactivated?.Invoke();
    }

    // Clear active balloons
    private void ClearActiveBalloons()
    {
        foreach (Transform balloon in transform)
        {
            balloon.gameObject.SetActive(false);
        }
        currentBalloonsInScene = 0;
    }

    public int GetCurrentBalloonsCount()
    {
        return currentBalloonsInScene;
    }
}
