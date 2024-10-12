using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 10f;
    public float minX = -10f;
    public float maxX = 10f;
    public BulletMovement objectToMove;

    void Update()
    {
        float moveInput = Input.GetAxis("Horizontal");

        Vector3 movement = new Vector3(moveInput, 0, 0) * speed * Time.deltaTime;

        // Aplicar el movimiento
        transform.Translate(movement);

        // Limitar la posición horizontal dentro de los límites
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
        transform.position = clampedPosition;

        // Control para disparar el objeto asignado
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            if (objectToMove != null)
            {
                objectToMove.StartMoving();
            }
        }
    }
}
