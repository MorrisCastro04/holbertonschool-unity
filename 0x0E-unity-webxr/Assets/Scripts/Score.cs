using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    [SerializeField]
    GameObject pins;

    int totalFallen = 0;

    [SerializeField]
    TextMeshProUGUI fallenPinsText;

    void Update()
    {
        CountFallenPins();
        fallenPinsText.text = totalFallen.ToString();
    }

    void CountFallenPins()
    {
        totalFallen = 0;

        foreach (Transform pin in pins.transform)
        {
            float xRotation = pin.eulerAngles.x;

            if (xRotation > 180f)
            {
                xRotation -= 360f;
            }

            if (xRotation > -80f || xRotation < -100f)
            {
                totalFallen++;
            }
        }
    }
}
