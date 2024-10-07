using UnityEngine;
using System;

public class BalloonManager : MonoBehaviour
{
    public BalloonPool balloonPool;
    public Action OnBalloonDeactivated; // Event triggered when a balloon is deactivated
    public int maxBalloonsInScene = 15; // Maximum number of balloons allowed in the scene

    [Header("Spawn Area")]
    public Vector2 minSpawnArea = new Vector2(-5f, -4f); // Minimum spawn area
    public Vector2 maxSpawnArea = new Vector2(5f, 4f);   // Maximum spawn area

    [Header("Common X Point")]
    public float commonXPoint = 0f; // The X coordinate where all balloons will pass during their movement
    public float gizmoSize = 0.5f;  // Size of the Gizmo representing the common X point
    public float moreminuscommon = 2f; // Range around the common X point for random spawning

    private int currentBalloonsInScene = 0; // Tracks active balloons

    // Method to draw the spawn area and common X point in the scene
    private void OnDrawGizmos()
    {
        // Set gizmo color to green for the spawn area
        Gizmos.color = Color.green;

        // Calculate the center of the spawn area
        Vector3 center = new Vector3((minSpawnArea.x + maxSpawnArea.x) / 2, (minSpawnArea.y + maxSpawnArea.y) / 2, 0);

        // Calculate the size of the spawn area
        Vector3 size = new Vector3(maxSpawnArea.x - minSpawnArea.x, maxSpawnArea.y - minSpawnArea.y, 1);

        // Draw a wireframe cube representing the spawn area
        Gizmos.DrawWireCube(center, size);

        // Set gizmo color to yellow for the common X point
        Gizmos.color = Color.yellow;

        // Draw a sphere representing the common X point on the X axis at the fixed common point
        Gizmos.DrawSphere(new Vector3(commonXPoint, 0, 0), gizmoSize);
    }

    // Method to spawn balloons
    public void SpawnBalloons(int count, float screenMinX, float screenMaxX)
    {
        // Set a new random common X point within the screen limits
        commonXPoint = UnityEngine.Random.Range(screenMinX, screenMaxX);

        // Log the attempt to spawn balloons
        Debug.Log($"Attempting to spawn {count} balloons at common X point: {commonXPoint}");

        // Ensure we don't spawn more than the maximum allowed balloons in the scene
        int balloonsToSpawn = Math.Min(count, maxBalloonsInScene - currentBalloonsInScene);

        if (balloonsToSpawn <= 0)
        {
            Debug.LogWarning("Cannot spawn more balloons. Max limit reached.");
            return;
        }

        // Spawn balloons
        for (int i = 0; i < balloonsToSpawn; i++)
        {
            GameObject balloon = balloonPool.GetBalloonFromPool();

            // Generate random Y position within the spawn area
            float randomY = UnityEngine.Random.Range(minSpawnArea.y, maxSpawnArea.y);

            // Generate random X position within the screen boundaries, keeping a range around commonXPoint
            float randomX = Mathf.Clamp(
                UnityEngine.Random.Range(commonXPoint - moreminuscommon, commonXPoint + moreminuscommon),
                screenMinX, screenMaxX); // Clamp to ensure it's within the screen bounds

            // Set the balloon's initial position
            balloon.transform.position = new Vector2(randomX, randomY);

            // Activate the balloon
            balloon.SetActive(true);

            // Initialize balloon movement with the correct parameters
            BalloonMovement balloonMovement = balloon.GetComponent<BalloonMovement>();
            if (balloonMovement != null)
            {
                balloonMovement.Initialize(commonXPoint, screenMinX, screenMaxX);
                balloonMovement.OnBalloonDestroyed -= HandleBalloonDeactivation; // Ensure no multiple subscriptions
                balloonMovement.OnBalloonDestroyed += HandleBalloonDeactivation;
            }
        }
    }

    // Handle balloon deactivation
    private void HandleBalloonDeactivation()
    {
        // Decrease the count of active balloons
        currentBalloonsInScene--;

        // Invoke the event for balloon deactivation
        OnBalloonDeactivated?.Invoke();

        // Log that a balloon has been deactivated
        Debug.Log("A balloon has been deactivated.");
    }
}
