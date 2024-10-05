using System;
using UnityEngine;

public class BalloonMovement : MonoBehaviour
{
    public float speed = 5f;
    public int initialDirection = 1;
    private int direction;
    private float screenLimitX; // Horizontal screen boundary in world coordinates

    // Action event to notify when the balloon is destroyed or deactivated
    public event Action OnBalloonDestroyed;

    void Start()
    {
        direction = initialDirection;
        // Calculate the horizontal screen limit (boundaries in world coordinates)
        screenLimitX = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;
    }

    void Update()
    {
        // Move the balloon based on its direction
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);

        // If the balloon crosses the horizontal screen limits, invert its direction
        if (transform.position.x > screenLimitX || transform.position.x < -screenLimitX)
        {
            ChangeDirection();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Deactivate the balloon on collision
        DeactivateBalloon();
    }

    private void DeactivateBalloon()
    {
        // Unsubscribe from the event when deactivating the balloon
        OnBalloonDestroyed?.Invoke(); // Trigger the event when the balloon is destroyed
        gameObject.SetActive(false); // Deactivate the balloon
        OnBalloonDestroyed -= null; // Clear the event subscriptions to avoid potential issues
        Debug.Log("Balloon deactivated and returned to the pool.");
    }

    // Method to change the direction when the balloon hits the screen limit
    private void ChangeDirection()
    {
        direction *= -1; // Invert the direction
        Debug.Log("Balloon hit the screen limit, changing direction.");
    }
}
