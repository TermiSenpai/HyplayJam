using UnityEngine;
using System;

// Balloon movement logic
public class BalloonMovement : MonoBehaviour, IBalloonMovement
{
    public float baseSpeed = 5f;
    private Vector2 direction;
    private float commonXPoint;
    private float screenMinX;
    private float screenMaxX;
    private float timeToReachXPoint;
    private float distanceToCommonXPoint;
    private float adjustedSpeed;

    public event Action OnBalloonDestroyed; // Event to signal balloon destruction

    // Initialize balloon with movement parameters
    public void Initialize(float newCommonXPoint, float minX, float maxX)
    {
        commonXPoint = newCommonXPoint;
        screenMinX = minX;
        screenMaxX = maxX;

        direction = (transform.position.x < commonXPoint) ? Vector2.right : Vector2.left;

        distanceToCommonXPoint = Mathf.Abs(commonXPoint - transform.position.x);
        timeToReachXPoint = 2.0f;
        adjustedSpeed = distanceToCommonXPoint / timeToReachXPoint;
    }

    void Update()
    {
        Move();
        CheckScreenBounds();
    }

    // Move the balloon
    private void Move()
    {
        transform.position += (Vector3)(direction * adjustedSpeed * Time.deltaTime);
    }

    // Check if balloon is out of screen bounds
    private void CheckScreenBounds()
    {
        if (transform.position.x >= screenMaxX && direction == Vector2.right)
        {
            ChangeDirection();
        }
        else if (transform.position.x <= screenMinX && direction == Vector2.left)
        {
            ChangeDirection();
        }
    }

    // Reverse the balloon direction
    private void ChangeDirection()
    {
        direction *= -1;
    }

    // Deactivate balloon and invoke destruction event
    private void DeactivateBalloon()
    {
        OnBalloonDestroyed?.Invoke(); // Notify that balloon is destroyed
        gameObject.SetActive(false); // Deactivate the balloon
    }

    // Handle collision events to destroy the balloon
    private void OnCollisionEnter2D(Collision2D collision)
    {
        DeactivateBalloon();
    }
}
