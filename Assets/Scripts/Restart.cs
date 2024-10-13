using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    private static bool musicPlaying = false;  // Flag para verificar si la música ya está sonando

    [SerializeField] private GameObject musicObject;  // El objeto que contiene el AudioSource

    void Start()
    {
        // Verificar si la música ya está sonando
        if (!musicPlaying)
        {
            DontDestroyOnLoad(musicObject);  // Evitar que el objeto de la música se destruya al recargar la escena
            musicPlaying = true;  // Marcar la música como activa
        }
        else
        {
            Destroy(musicObject);  // Si ya existe otro objeto de música, destruir este para evitar duplicados
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);  // Reiniciar la escena actual
    }
}
