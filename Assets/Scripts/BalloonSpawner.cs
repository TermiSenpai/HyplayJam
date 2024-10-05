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
        balloonManager.SpawnBalloons(level); // Delegate the task of spawning balloons to BalloonManager
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

    void Update()
    {
        // Debugging: Start the game manually by pressing the 'S' key
        if (Input.GetKeyDown(KeyCode.S) && !gameStarted)
        {
            Debug.Log("Starting the game manually.");
            StartGame();
        }
    }
}
