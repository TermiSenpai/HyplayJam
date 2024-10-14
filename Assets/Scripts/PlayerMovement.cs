using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 10f;
    public float minX = -10f;
    public float maxX = 10f;
    public BulletMovement objectToMove;
    AudioSource source;

    public bool canMove = true; // Variable para controlar si el jugador puede moverse

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    void Start()
    {
        // Suscribirse al evento cuando el BulletMovement termina de moverse
        objectToMove.OnShoot += EnableMovement;
    }

    void Update()
    {
        if (canMove) // Solo permite el movimiento si canMove es true
        {
            float moveInput = Input.GetAxis("Horizontal");

            Vector3 movement = new Vector3(moveInput, 0, 0) * speed * Time.deltaTime;

            // Aplicar el movimiento
            transform.Translate(movement);

            // Limitar la posición horizontal dentro de los límites
            Vector3 clampedPosition = transform.position;
            clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
            transform.position = clampedPosition;
        }

        // Control para disparar el objeto asignado
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            if (objectToMove != null && canMove) // Solo dispara si el jugador puede moverse
            {
                source.PlayOneShot(source.clip);
                objectToMove.StartMoving();
                DisableMovement(); // Deshabilitar el movimiento del jugador mientras el objeto se mueve
            }
        }
    }

    // Método para deshabilitar el movimiento del jugador
    private void DisableMovement()
    {
        canMove = false;
    }

    // Método para habilitar el movimiento del jugador cuando el BulletMovement termine de moverse
    private void EnableMovement()
    {
        canMove = true;
    }

    private void OnDestroy()
    {
        // Asegurarse de desuscribirse del evento para evitar errores
        objectToMove.OnShoot -= EnableMovement;
    }
}
