using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio; // Importar el namespace para AudioMixer

public class OptionsMenu : MonoBehaviour
{
    public Toggle invertToggleY;
    [SerializeField] private Slider BGMSlider;
    [SerializeField] private AudioMixer audioMixer; // Referencia al AudioMixer

    void Start()
    {
        // Configuración inicial de Invert Y
        if (PlayerPrefs.HasKey("InvertY"))
        {
            invertToggleY.isOn = PlayerPrefs.GetInt("InvertY") == 1;
        }
        else
        {
            invertToggleY.isOn = false;
        }

        // Configuración inicial de volumen BGM
        if (PlayerPrefs.HasKey("BGMVolume"))
        {
            float savedVolume = PlayerPrefs.GetFloat("BGMVolume");
            BGMSlider.value = savedVolume;
            SetBGMVolume(savedVolume);
        }
        else
        {
            BGMSlider.value = 0.75f; // Valor predeterminado (75%)
            SetBGMVolume(0.75f);
        }

        // Listener para cambios en el slider
        BGMSlider.onValueChanged.AddListener(SetBGMVolume);
    }

    public void Apply()
    {
        // Guardar configuración de Invert Y
        PlayerPrefs.SetInt("InvertY", invertToggleY.isOn ? 1 : 0);

        // Guardar configuración del volumen BGM
        PlayerPrefs.SetFloat("BGMVolume", BGMSlider.value);
        PlayerPrefs.Save();

        // Regresar a la escena anterior
        if (PlayerPrefs.HasKey("PreviousScene"))
        {
            SceneManager.LoadScene(PlayerPrefs.GetString("PreviousScene"));
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }

    public void Back()
    {
        // Regresar a la escena anterior
        if (PlayerPrefs.HasKey("PreviousScene"))
        {
            SceneManager.LoadScene(PlayerPrefs.GetString("PreviousScene"));
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }

    public void SetBGMVolume(float sliderValue)
    {
        // Convert the slider value (0 to 1) to dB (-80 to 0)
        float dB = Mathf.Log10(Mathf.Clamp(sliderValue, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat("BGM", dB);
    }
}
