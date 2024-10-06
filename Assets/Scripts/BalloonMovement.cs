using UnityEngine;
using System;

public class BalloonMovement : MonoBehaviour
{
    public float baseSpeed = 5f; // Base movement speed for the balloon
    private Vector2 direction; // Current direction of movement
    private float commonXPoint; // The X coordinate where all balloons will pass through
    private float screenMinX; // Left screen boundary
    private float screenMaxX; // Right screen boundary
    private float timeToReachXPoint; // Time it should take to reach common X point
    private float distanceToCommonXPoint; // Distance from the current position to the common X point
    private float adjustedSpeed; // Adjusted speed for synchronization

    // Action event to notify when the balloon is destroyed or deactivated
    public event Action OnBalloonDestroyed;

    // Initialize the balloon properties
    public void Initialize(float newCommonXPoint, float minX, float maxX)
    {
        commonXPoint = newCommonXPoint;
        screenMinX = minX;
        screenMaxX = maxX;

        // Set the initial direction based on position relative to commonXPoint
        direction = (transform.position.x < commonXPoint) ? Vector2.right : Vector2.left;

        // Calculate the distance to the common X point
        distanceToCommonXPoint = Mathf.Abs(commonXPoint - transform.position.x);

        // Set the time we want all balloons to take to reach the common X point (fixed time)
        timeToReachXPoint = 2.0f; // Example: all balloons should reach the X point in 2 seconds

        // Adjust speed so all balloons reach the common X point at the same time
        adjustedSpeed = distanceToCommonXPoint / timeToReachXPoint;
    }

    // Update the movement
    void Update()
    {
        Move(); // Handle movement in every frame
        CheckScreenBounds(); // Check if the balloon has reached the screen bounds
    }

    // Move the balloon in the current direction
    private void Move()
    {
        // Move the balloon at the adjusted speed towards the direction
        transform.position += (Vector3)(direction * adjustedSpeed * Time.deltaTime);
    }

    // Change the direction when reaching the screen bounds
    private void CheckScreenBounds()
    {
        // Reverse direction when hitting the right bound
        if (transform.position.x >= screenMaxX && direction == Vector2.right)
        {
            ChangeDirection();
        }
        // Reverse direction when hitting the left bound
        else if (transform.position.x <= screenMinX && direction == Vector2.left)
        {
            ChangeDirection();
        }
    }

    // Reverse the current direction of the balloon
    private void ChangeDirection()
    {
        direction *= -1; // Reverse the direction by multiplying by -1
    }

    private void DeactivateBalloon()
    {
        // Unsubscribe from the event when deactivating the balloon
        OnBalloonDestroyed?.Invoke(); // Trigger the event when the balloon is destroyed
        gameObject.SetActive(false); // Deactivate the balloon
        OnBalloonDestroyed -= null; // Clear the event subscriptions to avoid potential issues
        Debug.Log("Balloon deactivated and returned to the pool.");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        DeactivateBalloon();
    }
}
