using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonSpawner : MonoBehaviour
{
    public GameObject balloonPrefab; // Prefab del globo
    public int balloonCount = 10;    // N�mero de globos a generar
    public Vector2 spawnAreaMin;     // Coordenadas m�nimas de la zona de spawn (inferior izquierda)
    public Vector2 spawnAreaMax;     // Coordenadas m�ximas de la zona de spawn (superior derecha)

    private float alignedXPosition;  // Coordenada X donde todos los globos estar�n alineados

    void Start()
    {
        // Generar una posici�n X aleatoria donde los globos estar�n alineados
        alignedXPosition = Random.Range(spawnAreaMin.x, spawnAreaMax.x);

        SpawnBalloons();
    }

    void SpawnBalloons()
    {
        for (int i = 0; i < balloonCount; i++)
        {
            // Generar una posici�n Y aleatoria dentro del �rea de spawn
            float randomY = Random.Range(spawnAreaMin.y, spawnAreaMax.y);

            // Todos los globos comparten la misma posici�n X (se alinean en el eje X)
            Vector2 spawnPosition = new Vector2(alignedXPosition, randomY);

            // Instanciar el globo en la posici�n generada
            GameObject balloon = Instantiate(balloonPrefab, spawnPosition, Quaternion.identity);

            // Asignar una direcci�n de movimiento aleatoria (+1 o -1) al globo
            BalloonMovement balloonMovement = balloon.GetComponent<BalloonMovement>();
            if (balloonMovement != null)
            {
                balloonMovement.initialDirection = Random.Range(0, 2) == 0 ? 1 : -1;
            }
        }
    }

    // Dibujar los gizmos para mostrar el �rea de spawn
    private void OnDrawGizmos()
    {
        // Cambia el color de Gizmos
        Gizmos.color = Color.green;

        // Obtener el tama�o del �rea de spawn
        Vector2 spawnAreaSize = new Vector2(spawnAreaMax.x - spawnAreaMin.x, spawnAreaMax.y - spawnAreaMin.y);

        // Dibujar un rect�ngulo representando el �rea de spawn
        Gizmos.DrawWireCube((spawnAreaMin + spawnAreaMax) / 2, spawnAreaSize);
    }
}
