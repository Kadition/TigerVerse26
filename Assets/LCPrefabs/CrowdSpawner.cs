using UnityEngine;

public class CrowdSpawner : MonoBehaviour
{
    public GameObject fanPrefab;
    public int fansInRow = 20;
    public float spacing = 1.0f;

    void Start()
    {
        for (int i = 0; i < fansInRow; i++)
        {
            Vector3 spawnPos = transform.position + (transform.right * (i * spacing));
            Instantiate(fanPrefab, spawnPos, transform.rotation, transform);
        }
    }
}