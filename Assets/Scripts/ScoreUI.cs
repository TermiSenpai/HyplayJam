using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    public TextMeshProUGUI scoreText; // Referencia al Text UI para mostrar el puntaje
    ScoreManager score;

    private void Awake()
    {
        score = ScoreManager.Instance;
    }

    private void OnEnable()
    {
        // Verifica si el ScoreManager ya ha sido inicializado
        if (score != null)
        {
            // Suscribirse al evento de ScoreManager cuando cambia el puntaje
            score.OnScoreChanged += UpdateScore;
        }
        else
        {
            Debug.LogError("ScoreManager.Instance es null. Asegúrate de que el ScoreManager está presente en la escena.");
        }
    }

    private void OnDisable()
    {
        // Desuscribirse del evento de ScoreManager si está disponible
        if (score != null)
        {
            score.OnScoreChanged -= UpdateScore;
        }
    }

    // Método que se llama cuando cambia el puntaje
    private void UpdateScore(int newScore)
    {
        // Actualizar el texto del puntaje
        scoreText.text = "Score: " + newScore.ToString();
    }
}
