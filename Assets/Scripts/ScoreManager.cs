using UnityEngine;

// Delegado para el evento de cambio de score
public delegate void ScoreChangedHandler(int newScore);

public class ScoreManager : MonoBehaviour
{
    // Singleton de ScoreManager
    public static ScoreManager Instance { get; private set; }

    // Evento que se dispara cuando cambia el score
    public event ScoreChangedHandler OnScoreChanged;

    // Variable privada del puntaje
    private int _score;

    // Propiedad para el puntaje
    public int Score
    {
        get => _score;
        private set
        {
            // Verifica si el puntaje ha cambiado
            if (_score != value)
            {
                _score = value;

                // Lanza el evento si hubo un cambio
                OnScoreChanged?.Invoke(_score);
            }
        }
    }

    // Asegurar que el Singleton sea único
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Mantener la instancia entre escenas
        }
        else
        {
            Destroy(gameObject); // Si ya existe, destruir el duplicado
        }
    }

    // Método para agregar puntos
    public void AddPoints(int points)
    {
        Score += points;
    }

    // Método para restar puntos
    public void SubtractPoints(int points)
    {
        Score -= points;
    }

    // Método para resetear el puntaje
    public void ResetScore()
    {
        Score = 0;
    }
}
