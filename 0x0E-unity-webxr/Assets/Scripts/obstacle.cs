using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obstacle : MonoBehaviour
{
    private Transform explosionChild;

    ObstacleSystem obstacleSystem;
    CameraController cameraController;

    void Start()
    {
        explosionChild = transform.Find("Explosion");
        obstacleSystem = GameObject.Find("obstacleSystem")?.GetComponent<ObstacleSystem>();
        cameraController = GameObject.Find("CameraController")?.GetComponent<CameraController>();

        if (explosionChild != null)
        {
            explosionChild.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("No se encontró el hijo 'Explosion' en " + gameObject.name);
        }

        cameraController = GameObject.Find("Main Camera")?.GetComponent<CameraController>();
        if (!cameraController)
        {
            Debug.LogError("No se encontró el CameraController en la cámara principal.");
            enabled = false;
            return;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            if (explosionChild != null)
            {
                explosionChild.gameObject.SetActive(true);
            }

            Destroy(other.gameObject);
            obstacleSystem?.ClearObstacles();
            cameraController.StartCoroutine(cameraController.CameraDown());
        }
    }
}
