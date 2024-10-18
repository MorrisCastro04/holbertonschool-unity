using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public Toggle invertToggleY;
    void Start()
    {
        if (PlayerPrefs.HasKey("InvertY"))
        {
            invertToggleY.isOn = PlayerPrefs.GetInt("InvertY") == 1;
        }
        else
        {
            invertToggleY.isOn = false;
        }
        invertToggleY.onValueChanged.AddListener(delegate { ToggleInvertY(invertToggleY.isOn); });
    }
    public void Back()
    {
        SceneManager.LoadScene(0);
    }
    void ToggleInvertY(bool invertY)
    {
        PlayerPrefs.SetInt("InvertY", invertY ? 1 : 0);
        PlayerPrefs.Save();
    }
}
