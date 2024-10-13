using UnityEngine;

public class MenuToggle : MonoBehaviour
{
    public GameObject menuPanel;  // Panel que contiene los Sliders de audio
    private bool isMenuOpen = false;  // Variable para controlar el estado del menú

    // Función que alterna la visibilidad del menú
    public void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen;  // Cambia el estado del menú (abierto/cerrado)
        menuPanel.SetActive(isMenuOpen);  // Activa o desactiva el panel según el estado
    }
}
