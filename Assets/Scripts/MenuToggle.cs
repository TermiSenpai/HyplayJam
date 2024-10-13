using UnityEngine;

public class MenuToggle : MonoBehaviour
{
    public GameObject menuPanel;  // Panel que contiene los Sliders de audio
    private bool isMenuOpen = false;  // Variable para controlar el estado del men�

    // Funci�n que alterna la visibilidad del men�
    public void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen;  // Cambia el estado del men� (abierto/cerrado)
        menuPanel.SetActive(isMenuOpen);  // Activa o desactiva el panel seg�n el estado
    }
}
