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
        // Buscar el GameObject que contiene el AudioSource externo (esto solo debería hacerse una vez)
        if (source == null)
        {
            GameObject audioObject = GameObject.Find("balloonPOPs"); // Cambia el nombre según el objeto que contiene el AudioSource
            if (audioObject != null)
            {
                source = audioObject.GetComponent<AudioSource>();
            }
            else
            {
                Debug.LogError("No se encontró el objeto ExternalAudioSource.");
            }
        }
    }

    // Inicializar el movimiento del globo
    public void Initialize(float newCommonXPoint, float minX, float maxX)
    {
        commonXPoint = newCommonXPoint;
        screenMinX = minX;
        screenMaxX = maxX;

        // Determinar la dirección en función de la posición actual relativa a commonXPoint
        direction = (transform.position.x < commonXPoint) ? Vector2.right : Vector2.left;
    }

    void Update()
    {
        Move();
        CheckScreenBounds(); // Verificar si ha llegado al límite de la pantalla
    }

    private void Move()
    {
        // Moverse en la dirección actual con una velocidad constante
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    private void CheckScreenBounds()
    {
        // Si el globo ha alcanzado el límite derecho de la pantalla
        if (transform.position.x >= screenMaxX && direction == Vector2.right)
        {
            ChangeDirection(); // Cambiar la dirección a la izquierda
        }
        // Si el globo ha alcanzado el límite izquierdo de la pantalla
        else if (transform.position.x <= screenMinX && direction == Vector2.left)
        {
            ChangeDirection(); // Cambiar la dirección a la derecha
        }
    }

    // Cambiar la dirección del globo
    private void ChangeDirection()
    {
        direction *= -1; // Invertir la dirección
    }

    private void DeactivateBalloon()
    {
        ScoreManager.Instance.AddPoints(points); // Sumar puntos al destruir el globo
        OnBalloonDestroyed?.Invoke(); // Activar el evento de destrucción del globo
        gameObject.SetActive(false); // Desactivar el globo
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        source.time = soundTime;
        source.PlayOneShot(source.clip);
        DeactivateBalloon(); // Desactivar el globo al colisionar
    }
}
