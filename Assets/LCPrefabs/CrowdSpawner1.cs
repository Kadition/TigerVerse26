using UnityEngine;

public class CrowdSpawner1 : MonoBehaviour
{
    [Header("What to Spawn")]
    public GameObject fanPrefab;
    
    [Header("Column Settings (Left to Right)")]
    public int fansInRow = 12;
    public float spacing = 1.79f;

    [Header("Row Settings (Front to Back)")]
    public int numberOfRows = 5;
    
    // I have set the default to the exact math we just calculated!
    public Vector3 rowOffset = new Vector3(-3.22f, 2.03f, -0.02f); 

    void Start1()
    {
        // 1. Loop through the number of rows (Front to Back)
        for (int row = 0; row < numberOfRows; row++)
        {
            // Calculate where this specific row should start using your offset
            Vector3 rowStartPos = transform.position + (rowOffset * row);

            // 2. Loop to spawn the fans in this row (Left to Right)
            for (int i = 0; i < fansInRow; i++)
            {
                // Calculate individual fan position along the bleacher
                Vector3 spawnPos = rowStartPos + (transform.right * (i * spacing));
                
                // Spawn the fan and attach it to this GameObject to keep the Hierarchy clean
                Instantiate(fanPrefab, spawnPos, transform.rotation, transform);
            }
        }
    }
}