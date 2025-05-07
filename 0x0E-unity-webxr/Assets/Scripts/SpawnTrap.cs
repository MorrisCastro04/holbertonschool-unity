using UnityEngine;

public class SpawnTrap : MonoBehaviour
{
    public GameObject trap;
    public Transform positionXLeft;
    public Transform positionXRight;
    public Transform positionYLeft;
    public Transform positionYRight;

    public Vector3 RandomPos()
    {
        Vector3 spawnPoint = Vector3.zero;
        bool rightSide = Random.Range(0f, 1f) > 0.5f;

        if (rightSide)
        {
            spawnPoint.x = positionXRight.position.x;
            spawnPoint.y = Random.Range(positionXRight.position.y, positionYRight.position.y);
        }
        else
        {
            spawnPoint.x = positionXLeft.position.x;
            spawnPoint.y = Random.Range(positionXLeft.position.y, positionYLeft.position.y);
        }

        spawnPoint.z = transform.position.z;

        return spawnPoint;
    }

    public void Spawn()
    {
        Vector3 spawnPoint = RandomPos();

        Quaternion rotation = Quaternion.Euler(90f, 0f, 0f);

        GameObject spawnedTrap = Instantiate(trap, spawnPoint, rotation);

        spawnedTrap.transform.localScale = new Vector3(15f, 15f, 3f);
    }

}
