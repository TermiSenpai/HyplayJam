using System.Collections;
using UnityEngine;

public class BalloonSpawner : MonoBehaviour
{
    private BalloonManager balloonManager;
    private int level = 1;
    private bool gameStarted = false;

    void Start()
    {
        // Get the BalloonManager component and validate it
        balloonManager = GetComponent<BalloonManager>();
        if (balloonManager == null)
        {
            Debug.LogError("BalloonManager component is missing on this GameObject. Please add it.");
        }
        else
        {
            balloonManager.OnBalloonDeactivated += OnBalloonDeactivated; // Subscribe to the event
        }
    }

    // Method to start the game
    public void StartGame()
    {
        if (!gameStarted)
        {
            gameStarted = true;
            StartCoroutine(StartLevel(level));
        }
    }

    IEnumerator StartLevel(int level)
    {
        yield return new WaitForSeconds(2f);

        float screenMinX = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        float screenMaxX = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;

        // Spawn up to 10 balloons for the current level
        balloonManager.SpawnBalloons(level, screenMinX, screenMaxX);
    }

    // Handle the balloon deactivation event
    private void OnBalloonDeactivated()
    {
        // If all balloons have been deactivated, start the next level
        if (balloonManager.GetCurrentBalloonsCount() == 0)
        {
            level++;
            StartCoroutine(StartLevel(level));
        }
    }
}
