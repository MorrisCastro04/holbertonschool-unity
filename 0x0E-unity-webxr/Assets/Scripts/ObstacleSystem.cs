using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSystem : MonoBehaviour
{

    public GameObject obstaclePrefab;

    public int numberOfObstacles = 3;
    public void SpawnObstacle()
    {
        for (int i = 0; i < numberOfObstacles; i++)
        {
            float posX = Random.Range(-11.5f, -9.7f);
            float posY = 0.93f;
            float posZ = Random.Range(-16f, -9f);
            Vector3 position = new Vector3(posX, posY, posZ);
            Quaternion rotation = Quaternion.Euler(90f, 0f, 0f);
            Instantiate(obstaclePrefab, position, rotation, transform);
        }
    }

    public void ClearObstacles()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject, 0.5f);
        }
    }
}
