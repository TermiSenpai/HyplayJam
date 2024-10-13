using System;
using UnityEngine;

public class BalloonManager : MonoBehaviour
{
    public BalloonPool pool;
    public IBalloonPool balloonPool; // Reference to the balloon pool
    public event Action OnBalloonDeactivated; // Event triggered when a balloon is deactivated
    public BulletMovement bullet;
    public event Action OnGameOver;

    public int maxBalloonsInScene = 10; // Max of 10 balloons active at once
    private int currentBalloonsInScene = 0;

    [Header("Spawn Area")]
    public Vector2 minSpawnArea = new Vector2(-5f, -4f); // Minimum spawn area
    public Vector2 maxSpawnArea = new Vector2(5f, 4f);   // Maximum spawn area

    [Header("Common X Point")]
    public float commonXPoint;  // This value will be assigned during gameplay
    public float moreminuscommon = 2f;

    private void Start()
    {
        balloonPool = pool;
    }

    private void Awake()
    {
        bullet.OnShoot += HandleShoots;
    }

    public void SpawnBalloons(int level, float screenMinX, float screenMaxX)
    {
        ClearActiveBalloons();

        int balloonsToSpawn = Math.Min(level, maxBalloonsInScene);

        // Set commonXPoint within the spawn area
        commonXPoint = UnityEngine.Random.Range(minSpawnArea.x, maxSpawnArea.x);

        // Set a fixed time for all balloons to reach the commonXPoint
        float timeToReachCommonXPoint = 2.0f; // All balloons will pass through the commonXPoint in 2 seconds

        for (int i = 0; i < balloonsToSpawn; i++)
        {
            GameObject balloon = balloonPool.GetBalloonFromPool();
            if (balloon == null)
            {
                Debug.LogError("Failed to retrieve balloon from pool.");
                continue;
            }

            // Set a random Y position within the spawn area
            float randomY = UnityEngine.Random.Range(minSpawnArea.y, maxSpawnArea.y);

            // Calculate the direction: decide whether the balloon starts on the left or right
            bool startOnLeft = UnityEngine.Random.value > 0.5f;
            float initialXPosition;

            // Calculate initial X position based on the timeToReachCommonXPoint and the balloon speed
            if (startOnLeft)
            {
                initialXPosition = commonXPoint - (timeToReachCommonXPoint * balloon.GetComponent<BalloonMovement>().speed);
            }
            else
            {
                initialXPosition = commonXPoint + (timeToReachCommonXPoint * balloon.GetComponent<BalloonMovement>().speed);
            }

            // Set the balloon's initial position
            balloon.transform.position = new Vector2(initialXPosition, randomY);
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

    private void HandleBalloonDeactivation()
    {
        currentBalloonsInScene--;
        OnBalloonDeactivated?.Invoke();
    }

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

    public bool AreBalloonsRemaining()
    {
        return currentBalloonsInScene > 0;
    }

    private void HandleShoots()
    {
        if(AreBalloonsRemaining())
        {
            OnGameOver?.Invoke();
            ClearActiveBalloons();
        }
        else
        {
            bullet.Reactivate();
        }

    }

    // Method to draw Gizmos in the Scene view
    private void OnDrawGizmos()
    {
        // Set the Gizmo color for the spawn area (cyan)
        Gizmos.color = Color.cyan;

        // Calculate the size of the spawn area
        Vector3 spawnAreaSize = new Vector3(maxSpawnArea.x - minSpawnArea.x, maxSpawnArea.y - minSpawnArea.y, 1f);

        // Draw the spawn area as a wireframe box in the Scene view
        Gizmos.DrawWireCube(new Vector3((minSpawnArea.x + maxSpawnArea.x) / 2f, (minSpawnArea.y + maxSpawnArea.y) / 2f, 0), spawnAreaSize);

        // Set the Gizmo color for the commonXPoint (red)
        Gizmos.color = Color.red;

        // Draw a vertical line representing the commonXPoint in the Scene view
        Gizmos.DrawLine(new Vector3(commonXPoint, minSpawnArea.y, 0), new Vector3(commonXPoint, maxSpawnArea.y, 0));
    }
}
