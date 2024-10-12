using UnityEngine;

public class RandomSpriteSelector : MonoBehaviour
{
    public Sprite[] sprites; // Array de sprites asignados desde el inspector
    private SpriteRenderer spriteRenderer; // Referencia al SpriteRenderer del objeto


    private void Awake()
    {
        // Obtener la referencia al SpriteRenderer
        spriteRenderer = GetComponent<SpriteRenderer>();        
    }

    void OnEnable()
    {
        // Comprobar si hay sprites disponibles en el array
        if (sprites.Length > 0)
        {
            // Seleccionar un sprite aleatoriamente
            int randomIndex = Random.Range(0, sprites.Length);

            // Asignar el sprite seleccionado al SpriteRenderer
            spriteRenderer.sprite = sprites[randomIndex];
        }
        else
        {
            Debug.LogWarning("No se han asignado sprites al array.");
        }
    }
}
