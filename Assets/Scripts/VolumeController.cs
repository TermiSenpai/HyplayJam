using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeController : MonoBehaviour
{
    public AudioMixer audioMixer;  // Referencia al AudioMixer

    public Slider masterSlider;    // Slider para el volumen general
    public Slider musicSlider;     // Slider para el volumen de la música
    public Slider effectsSlider;   // Slider para el volumen de los efectos

    // Funciones para ajustar cada uno de los volúmenes
    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume);
    }

    public void SetEffectsVolume(float volume)
    {
        audioMixer.SetFloat("EffectsVolume", volume);
    }

    void Start()
    {
        // Configurar los listeners para los Sliders
        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        effectsSlider.onValueChanged.AddListener(SetEffectsVolume);

        // Establecer valores iniciales (puedes modificar estos valores iniciales)
        masterSlider.value = -20f;
        musicSlider.value = -20f;
        effectsSlider.value = -20f;
    }
}
