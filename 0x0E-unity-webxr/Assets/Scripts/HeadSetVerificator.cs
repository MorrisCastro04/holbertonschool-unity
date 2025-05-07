using UnityEngine;
using UnityEngine.XR;

public class HeadSetVerificator : MonoBehaviour
{
    public static bool isHeadsetConnected = false;

    void Start()
    {
        CheckForHeadset();
    }

    private void CheckForHeadset()
    {
        isHeadsetConnected = XRSettings.isDeviceActive && XRSettings.loadedDeviceName == "Oculus";

        if (isHeadsetConnected)
        {
            Debug.Log("Casque détecté. Mode VR activé.");
        }
        else
        {
            Debug.LogWarning("Aucun casque détecté. Mode clavier/souris activé.");
        }
    }
}
