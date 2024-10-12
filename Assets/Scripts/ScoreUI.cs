using TMPro;
using UnityEngine;
using System.Collections;

public class ScoreUI : MonoBehaviour
{
    public TextMeshProUGUI scoreText; // Referencia al Text UI para mostrar el puntaje
    ScoreManager score;

    public float animationDuration = 0.2f; // Duración de la animación de cambio de escala
    public float maxScale = 1.5f; // Tamaño máximo al que escalará el texto durante la animación

    private Coroutine currentAnimation; // Para almacenar la corutina activa

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

        // Si hay una animación en curso, detenerla
        if (currentAnimation != null)
        {
            StopCoroutine(currentAnimation);
        }

        // Iniciar la animación de rebote
        currentAnimation = StartCoroutine(AnimateText());
    }

    // Coroutine para animar el texto con una escala que crece y luego vuelve a su tamaño normal
    private IEnumerator AnimateText()
    {
        // Fase de crecimiento
        float timer = 0f;
        while (timer < animationDuration)
        {
            timer += Time.deltaTime;
            float scale = Mathf.Lerp(1f, maxScale, timer / animationDuration);
            scoreText.rectTransform.localScale = new Vector3(scale, scale, scale);
            yield return null;
        }

        // Fase de regreso al tamaño normal
        timer = 0f;
        while (timer < animationDuration)
        {
            timer += Time.deltaTime;
            float scale = Mathf.Lerp(maxScale, 1f, timer / animationDuration);
            scoreText.rectTransform.localScale = new Vector3(scale, scale, scale);
            yield return null;
        }

        // Finalizar la animación actual
        currentAnimation = null;
    }
}
