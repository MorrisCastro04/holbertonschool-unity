using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obstacle : MonoBehaviour
{
    private Transform explosionChild;

    ObstacleSystem obstacleSystem;

    void Start()
    {
        explosionChild = transform.Find("Explosion");
        obstacleSystem = GameObject.Find("obstacleSystem")?.GetComponent<ObstacleSystem>();

        if (explosionChild != null)
        {
            explosionChild.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("No se encontró el hijo 'Explosion' en " + gameObject.name);
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
        }
    }
}
