using UnityEngine;
using System;

public class BalloonMovement : MonoBehaviour, IBalloonMovement
{
    public float speed = 5f; // Velocidad constante para todos los globos
    private Vector2 direction;
    private float commonXPoint;
    private float screenMinX;
    private float screenMaxX;
    private AudioSource source;
    public float soundTime;

    [Header("Points")]
    [SerializeField] int points = 100;

    public event Action OnBalloonDestroyed; // Evento que se activa cuando el globo se destruye

    private void Awake()
    {
        // Buscar el GameObject que contiene el AudioSource externo (esto solo deber�a hacerse una vez)
        if (source == null)
        {
            GameObject audioObject = GameObject.Find("balloonPOPs"); // Cambia el nombre seg�n el objeto que contiene el AudioSource
            if (audioObject != null)
            {
                source = audioObject.GetComponent<AudioSource>();
            }
            else
            {
                Debug.LogError("No se encontr� el objeto ExternalAudioSource.");
            }
        }
    }

    // Inicializar el movimiento del globo
    public void Initialize(float newCommonXPoint, float minX, float maxX)
    {
        commonXPoint = newCommonXPoint;
        screenMinX = minX;
        screenMaxX = maxX;

        // Determinar la direcci�n en funci�n de la posici�n actual relativa a commonXPoint
        direction = (transform.position.x < commonXPoint) ? Vector2.right : Vector2.left;
    }

    void Update()
    {
        Move();
        CheckScreenBounds(); // Verificar si ha llegado al l�mite de la pantalla
    }

    private void Move()
    {
        // Moverse en la direcci�n actual con una velocidad constante
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    private void CheckScreenBounds()
    {
        // Si el globo ha alcanzado el l�mite derecho de la pantalla
        if (transform.position.x >= screenMaxX && direction == Vector2.right)
        {
            ChangeDirection(); // Cambiar la direcci�n a la izquierda
        }
        // Si el globo ha alcanzado el l�mite izquierdo de la pantalla
        else if (transform.position.x <= screenMinX && direction == Vector2.left)
        {
            ChangeDirection(); // Cambiar la direcci�n a la derecha
        }
    }

    // Cambiar la direcci�n del globo
    private void ChangeDirection()
    {
        direction *= -1; // Invertir la direcci�n
    }

    private void DeactivateBalloon()
    {
        ScoreManager.Instance.AddPoints(points); // Sumar puntos al destruir el globo
        OnBalloonDestroyed?.Invoke(); // Activar el evento de destrucci�n del globo
        gameObject.SetActive(false); // Desactivar el globo
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        source.time = soundTime;
        source.PlayOneShot(source.clip);
        DeactivateBalloon(); // Desactivar el globo al colisionar
    }
}
