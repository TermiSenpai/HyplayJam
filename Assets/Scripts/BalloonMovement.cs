using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonMovement : MonoBehaviour
{
    public float speed = 5f;
    public int initialDirection = 1;
    private Vector2 screenBounds;
    private float objectWidth;
    private int direction;

    void Start()
    {
        direction = initialDirection;

        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

        objectWidth = transform.GetComponent<SpriteRenderer>().bounds.extents.x;
    }

    void Update()
    {
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);

        if (transform.position.x > screenBounds.x - objectWidth || transform.position.x < -screenBounds.x + objectWidth)
        {
            direction *= -1;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        gameObject.SetActive(false);
    }
}
