using System.Collections;
using HYPLAY.Core.Runtime;
using HYPLAY.Leaderboards.Runtime;
using TMPro;
using UnityEngine;

public class GameOverLeaderboard : MonoBehaviour
{
    [SerializeField] private HyplayLeaderboard leaderboard;
    [SerializeField, Range(0, 10)] private int numScoresToShow = 10;
    [SerializeField] private HyplayLeaderboard.OrderBy orderBy;
    [SerializeField] private TextMeshProUGUI[] scoreText;
    [SerializeField] private TextMeshProUGUI myScore;

    private int _score = 0;

    private void Awake()
    {
        // Obtener los puntajes si ya está conectado
        HyplayBridge.LoggedIn += GetScores;
        if (HyplayBridge.IsLoggedIn)
            GetScores();

        if (leaderboard == null)
            Debug.LogError("Please select a leaderboard to use");
    }

    // Esta función se llamará cuando termine el juego (GameOver)
    public void OnGameOver(int finalScore)
    {
        _score = finalScore;
        myScore.text = $"Score: {_score}";  // Eliminar formato decimal ya que es un entero
        SubmitScore();  // Enviar el puntaje al terminar el juego
    }

    // Enviar el puntaje
    public async void SubmitScore()
    {
        if (leaderboard == null) return;

        var res = await leaderboard.PostScore(_score);  // Enviar directamente el entero
        if (res.Success)
            Debug.Log($"Successfully posted score {res.Data.score}");
        else
            Debug.LogError($"Update score failed: {res.Error}");

        GetScores();  // Obtener los puntajes después de enviar el nuevo puntaje
    }

    // Obtener los puntajes de la tabla de líderes
    private async void GetScores()
    {
        if (leaderboard == null) return;

        // Esconder los textos antes de actualizarlos
        foreach (var text in scoreText)
            text.gameObject.SetActive(false);

        var scores = await leaderboard.GetScores(orderBy, 0, numScoresToShow);
        if (!scores.Success)
        {
            Debug.Log($"Getting scores failed: {scores.Error}");
            return;
        }

        // Mostrar los puntajes
        for (var i = 0; i < scores.Data.scores.Length; i++)
        {
            var score = scores.Data.scores[i];
            var text = scoreText[i];
            text.gameObject.SetActive(true);
            text.text = $"{score.username} scored {score.score}";  // Mostrar puntaje como entero
        }
    }
}
