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
            Debug.LogError("ScoreManager.Instance es null. Aseg�rate de que el ScoreManager est� presente en la escena.");
        }
    }

    private void OnDisable()
    {
        // Desuscribirse del evento de ScoreManager si est� disponible
        if (score != null)
        {
            score.OnScoreChanged -= UpdateScore;
        }
    }

    // M�todo que se llama cuando cambia el puntaje
    private void UpdateScore(int newScore)
    {
        // Actualizar el texto del puntaje
        scoreText.text = "Score: " + newScore.ToString();
    }
}
