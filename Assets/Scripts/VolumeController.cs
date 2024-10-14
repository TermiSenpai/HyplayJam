using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeController : MonoBehaviour
{
    public AudioMixer audioMixer;  // Referencia al AudioMixer

    public Slider masterSlider;    // Slider para el volumen general
    public Slider musicSlider;     // Slider para el volumen de la música
    public Slider effectsSlider;   // Slider para el volumen de los efectos

    public float starterVolume = -10f;

    private const string MasterVolumeKey = "MasterVolume";
    private const string MusicVolumeKey = "MusicVolume";
    private const string EffectsVolumeKey = "EffectsVolume";

    // Funciones para ajustar cada uno de los volúmenes
    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat(MasterVolumeKey, volume);
        PlayerPrefs.SetFloat(MasterVolumeKey, volume); // Guardar el valor
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat(MusicVolumeKey, volume);
        PlayerPrefs.SetFloat(MusicVolumeKey, volume); // Guardar el valor
    }

    public void SetEffectsVolume(float volume)
    {
        audioMixer.SetFloat(EffectsVolumeKey, volume);
        PlayerPrefs.SetFloat(EffectsVolumeKey, volume); // Guardar el valor
    }

    void Start()
    {
        // Configurar los listeners para los Sliders
        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        effectsSlider.onValueChanged.AddListener(SetEffectsVolume);

        // Cargar valores guardados o usar valores por defecto si no existen
        float savedMasterVolume = PlayerPrefs.GetFloat(MasterVolumeKey, starterVolume);
        float savedMusicVolume = PlayerPrefs.GetFloat(MusicVolumeKey, starterVolume);
        float savedEffectsVolume = PlayerPrefs.GetFloat(EffectsVolumeKey, starterVolume);

        // Establecer los valores en los sliders
        masterSlider.value = savedMasterVolume;
        musicSlider.value = savedMusicVolume;
        effectsSlider.value = savedEffectsVolume;

        // También establecer el volumen en el AudioMixer
        audioMixer.SetFloat(MasterVolumeKey, savedMasterVolume);
        audioMixer.SetFloat(MusicVolumeKey, savedMusicVolume);
        audioMixer.SetFloat(EffectsVolumeKey, savedEffectsVolume);
    }

    void OnApplicationQuit()
    {
        // Guardar los últimos valores de volumen al salir del juego
        PlayerPrefs.SetFloat(MasterVolumeKey, masterSlider.value);
        PlayerPrefs.SetFloat(MusicVolumeKey, musicSlider.value);
        PlayerPrefs.SetFloat(EffectsVolumeKey, effectsSlider.value);
        PlayerPrefs.Save();
    }
}
