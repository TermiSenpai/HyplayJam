using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonSpawner : MonoBehaviour
{
    public GameObject balloonPrefab; // Prefab del globo
    public int balloonCount = 10;    // Número de globos a generar
    public Vector2 spawnAreaMin;     // Coordenadas mínimas de la zona de spawn (inferior izquierda)
    public Vector2 spawnAreaMax;     // Coordenadas máximas de la zona de spawn (superior derecha)

    private float alignedXPosition;  // Coordenada X donde todos los globos estarán alineados

    void Start()
    {
        // Generar una posición X aleatoria donde los globos estarán alineados
        alignedXPosition = Random.Range(spawnAreaMin.x, spawnAreaMax.x);

        SpawnBalloons();
    }

    void SpawnBalloons()
    {
        for (int i = 0; i < balloonCount; i++)
        {
            // Generar una posición Y aleatoria dentro del área de spawn
            float randomY = Random.Range(spawnAreaMin.y, spawnAreaMax.y);

            // Todos los globos comparten la misma posición X (se alinean en el eje X)
            Vector2 spawnPosition = new Vector2(alignedXPosition, randomY);

            // Instanciar el globo en la posición generada
            GameObject balloon = Instantiate(balloonPrefab, spawnPosition, Quaternion.identity);

            // Asignar una dirección de movimiento aleatoria (+1 o -1) al globo
            BalloonMovement balloonMovement = balloon.GetComponent<BalloonMovement>();
            if (balloonMovement != null)
            {
                balloonMovement.initialDirection = Random.Range(0, 2) == 0 ? 1 : -1;
            }
        }
    }

    // Dibujar los gizmos para mostrar el área de spawn
    private void OnDrawGizmos()
    {
        // Cambia el color de Gizmos
        Gizmos.color = Color.green;

        // Obtener el tamaño del área de spawn
        Vector2 spawnAreaSize = new Vector2(spawnAreaMax.x - spawnAreaMin.x, spawnAreaMax.y - spawnAreaMin.y);

        // Dibujar un rectángulo representando el área de spawn
        Gizmos.DrawWireCube((spawnAreaMin + spawnAreaMax) / 2, spawnAreaSize);
    }
}
