using UnityEngine;

public class BalloonSpawner : MonoBehaviour
{
    private BalloonManager balloonManager;
    private int level = 1; // Starting level
    private int activeBalloons = 0; // Active balloons counter
    private bool gameStarted = false;

    void Start()
    {
        balloonManager = GetComponent<BalloonManager>(); // Reference to the BalloonManager
        balloonManager.OnBalloonDeactivated += OnBalloonDeactivated; // Subscribe to the event
        Debug.Log("BalloonSpawner initialized.");
    }

    // Public method to start the game
    public void StartGame()
    {
        if (!gameStarted)
        {
            gameStarted = true;
            Debug.Log("Game started.");
            StartLevel(level); // Start the first level
        }
        else
        {
            Debug.LogWarning("The game has already started.");
        }
    }

    void StartLevel(int level)
    {
        Debug.Log($"Starting level {level} with {level} balloons.");
        activeBalloons = level; // Set the number of balloons for this level

        // Get the screen boundaries in world coordinates
        float screenMinX = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        float screenMaxX = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;

        // Call SpawnBalloons with the correct number of balloons and screen boundaries
        balloonManager.SpawnBalloons(level, screenMinX, screenMaxX);
    }

    // Method to calculate the new common X point for each level
    float CalculateCommonXPoint(int level)
    {
        // Example logic: Change the common X point for each level
        return level * 2f; // This will move the common X point further right as the level increases
    }

    // Method called when a balloon is deactivated
    private void OnBalloonDeactivated()
    {
        activeBalloons--;
        Debug.Log($"Balloon deactivated. {activeBalloons} balloons remaining.");

        if (activeBalloons == 0)
        {
            Debug.Log($"Level {level} completed.");
            level++; // Increase the level
            StartLevel(level); // Start the next level
        }
    }
}
