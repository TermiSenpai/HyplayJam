using UnityEngine;
using System;

public class BalloonManager : MonoBehaviour
{
    public BalloonPool balloonPool;
    public Action OnBalloonDeactivated; // Event triggered when a balloon is deactivated
    public int maxBalloonsInScene = 15; // Maximum balloons allowed in the scene
    private int currentBalloonsInScene = 0; // Keeps track of active balloons

    // Spawns the required number of balloons for the current level
    public void SpawnBalloons(int count)
    {
        Debug.Log($"Attempting to spawn {count} balloons.");
        int balloonsToSpawn = Math.Min(count, maxBalloonsInScene - currentBalloonsInScene); // Ensure we don't exceed the max limit

        if (balloonsToSpawn <= 0)
        {
            Debug.LogWarning("Cannot spawn more balloons. Max limit reached.");
            return;
        }

        Debug.Log($"Spawning {balloonsToSpawn} balloons.");
        for (int i = 0; i < balloonsToSpawn; i++)
        {
            GameObject balloon = balloonPool.GetBalloonFromPool();
            float randomY = UnityEngine.Random.Range(-4f, 4f); // Random Y position
            balloon.transform.position = new Vector2(0, randomY);
            balloon.SetActive(true); // Activate the balloon

            currentBalloonsInScene++; // Track how many balloons are active

            BalloonMovement balloonMovement = balloon.GetComponent<BalloonMovement>();
            if (balloonMovement != null)
            {
                balloonMovement.OnBalloonDestroyed -= HandleBalloonDeactivation; // Ensure no multiple subscriptions
                balloonMovement.OnBalloonDestroyed += HandleBalloonDeactivation;
            }
        }
    }

    // Handles the deactivation of a balloon
    private void HandleBalloonDeactivation()
    {
        currentBalloonsInScene--; // Decrease the count of balloons in the scene
        OnBalloonDeactivated?.Invoke(); // Notify that a balloon has been deactivated
        Debug.Log("A balloon has been deactivated.");
    }
}
