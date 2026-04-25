using UnityEngine;

public class FanBehavior : MonoBehaviour
{
    private float bounceSpeed;
    private float bounceHeight;
    private float randomOffset;
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
        bounceSpeed = Random.Range(3f, 6f);
        bounceHeight = Random.Range(0.1f, 0.3f);
        randomOffset = Random.Range(0f, 10f); 
    }

    void Update()
    {
        float newY = startPos.y + (Mathf.Sin(Time.time * bounceSpeed + randomOffset) * bounceHeight);
        if (newY < startPos.y) newY = startPos.y; // Stops them from sinking into the floor
        transform.localPosition = new Vector3(startPos.x, newY, startPos.z);
    }
}