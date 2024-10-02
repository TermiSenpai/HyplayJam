using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public BulletMovement objectToMove; 

    void Update()
    {
        float moveInput = Input.GetAxis("Horizontal");

        Vector3 movement = new Vector3(moveInput, 0, 0) * speed * Time.deltaTime;

        transform.Translate(movement);

        if (Input.GetMouseButtonDown(0)) 
        {
            if (objectToMove != null)
            {                
                objectToMove.StartMoving();
            }
        }
    }
}
