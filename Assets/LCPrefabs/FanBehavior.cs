using UnityEngine;

public class FanBehavior : MonoBehaviour
{
    [Header("Appearance")]
    public Material[] teamColors;

    private float bounceSpeed;
    private float bounceHeight;
    private float randomOffset;
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
        
        // Randomize the bounce math for every single fan so they look alive
        bounceSpeed = Random.Range(3f, 6f);
        bounceHeight = Random.Range(0.1f, 0.3f);
        randomOffset = Random.Range(0f, 10f); 

        // Automatically find the capsule body and sphere head
        Renderer[] allRenderers = GetComponentsInChildren<Renderer>();

        // Paint the fan a random color from your list
        if (teamColors.Length > 0)
        {
            Material chosenColor = teamColors[Random.Range(0, teamColors.Length)];
            foreach (Renderer r in allRenderers)
            {
                r.material = chosenColor;
            }
        }
    }

    void Update()
    {
        // CPU-friendly bouncing math (No Animator needed)
        float newY = startPos.y + (Mathf.Sin(Time.time * bounceSpeed + randomOffset) * bounceHeight);
        
        // Prevent the fan from sinking through the floor
        if (newY < startPos.y) newY = startPos.y; 
        
        transform.localPosition = new Vector3(startPos.x, newY, startPos.z);
    }
}