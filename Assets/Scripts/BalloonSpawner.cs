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

        balloonManager.SpawnBalloons(level, screenMinX, screenMaxX);
    }

    private void OnBalloonDeactivated()
    {
        if (balloonManager.GetCurrentBalloonsCount() == 0)
        {
            level++;
            StartCoroutine(StartLevel(level));
        }
    }

    public void OnPlayerShot()
    {

        if (balloonManager.GetCurrentBalloonsCount() == 0)
        {
            level++;
            StartCoroutine(StartLevel(level));
        }
    }
}
