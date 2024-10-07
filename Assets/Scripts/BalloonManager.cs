using System;
using UnityEngine;

public class BalloonManager : MonoBehaviour
{
    public BalloonPool pool;
    public IBalloonPool balloonPool; // Reference to the balloon pool
    public event Action OnBalloonDeactivated; // Event triggered when a balloon is deactivated

    public int maxBalloonsInScene = 10; // Max of 10 balloons active at once
    private int currentBalloonsInScene = 0;

    [Header("Spawn Area")]
    public Vector2 minSpawnArea = new Vector2(-5f, -4f); // Minimum spawn area
    public Vector2 maxSpawnArea = new Vector2(5f, 4f);   // Maximum spawn area

    [Header("Common X Point")]
    public float commonXPoint = 0f;
    public float moreminuscommon = 2f;

    private void Start()
    {
        balloonPool = pool;
    }

    // Method to spawn balloons for the current level, limited to maxBalloonsInScene (10 balloons)
    public void SpawnBalloons(int level, float screenMinX, float screenMaxX)
    {
        ClearActiveBalloons();

        int balloonsToSpawn = Math.Min(level, maxBalloonsInScene); // Limit to maxBalloonsInScene

        commonXPoint = UnityEngine.Random.Range(screenMinX, screenMaxX);

        for (int i = 0; i < balloonsToSpawn; i++)
        {
            GameObject balloon = balloonPool.GetBalloonFromPool();
            if (balloon == null)
            {
                Debug.LogError("Failed to retrieve balloon from pool.");
                continue;
            }

            float randomY = UnityEngine.Random.Range(minSpawnArea.y, maxSpawnArea.y);
            float randomX = Mathf.Clamp(UnityEngine.Random.Range(commonXPoint - moreminuscommon, commonXPoint + moreminuscommon), screenMinX, screenMaxX);

            balloon.transform.position = new Vector2(randomX, randomY);
            balloon.SetActive(true);

            IBalloonMovement balloonMovement = balloon.GetComponent<IBalloonMovement>();
            if (balloonMovement != null)
            {
                balloonMovement.Initialize(commonXPoint, screenMinX, screenMaxX);
                balloonMovement.OnBalloonDestroyed -= HandleBalloonDeactivation; // Unsubscribe to avoid duplicates
                balloonMovement.OnBalloonDestroyed += HandleBalloonDeactivation;
            }
        }

        currentBalloonsInScene = balloonsToSpawn;

        Debug.Log($"Spawned {balloonsToSpawn} balloons.");
    }

    // Handle balloon deactivation
    private void HandleBalloonDeactivation()
    {
        currentBalloonsInScene--;

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
