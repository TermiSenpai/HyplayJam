using TMPro;
using UnityEngine;
using System.Collections;

public class ScoreUI : MonoBehaviour
{
    public TextMeshProUGUI scoreText; // Referencia al Text UI para mostrar el puntaje
    ScoreManager score;

    public float animationDuration = 0.2f; // Duraci�n de la animaci�n de cambio de escala
    public float maxScale = 1.5f; // Tama�o m�ximo al que escalar� el texto durante la animaci�n

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

        // Si hay una animaci�n en curso, detenerla
        if (currentAnimation != null)
        {
            StopCoroutine(currentAnimation);
        }

        // Iniciar la animaci�n de rebote
        currentAnimation = StartCoroutine(AnimateText());
    }

    // Coroutine para animar el texto con una escala que crece y luego vuelve a su tama�o normal
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

        // Fase de regreso al tama�o normal
        timer = 0f;
        while (timer < animationDuration)
        {
            timer += Time.deltaTime;
            float scale = Mathf.Lerp(maxScale, 1f, timer / animationDuration);
            scoreText.rectTransform.localScale = new Vector3(scale, scale, scale);
            yield return null;
        }

        // Finalizar la animaci�n actual
        currentAnimation = null;
    }
}
