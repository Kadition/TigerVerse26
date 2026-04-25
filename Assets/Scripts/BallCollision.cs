using UnityEngine;

public class BallCollision : MonoBehaviour
{
    public Vector3 predictedBounceLocation;

    private const float gravityAccel = -9.81f;

    private const float speedMultiplier = 0.1f;

    [SerializeField] private Transform racketFace;

    private Vector3 lastTransform;

    private Vector3 currentVelocity;

    public bool hasBeenHit = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lastTransform = racketFace.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(!hasBeenHit)
        {
            return;
        }

        transform.position = transform.position + currentVelocity * Time.deltaTime + 0.5f * gravityAccel * Time.deltaTime * Time.deltaTime * Vector3.up;

        currentVelocity += gravityAccel * Time.deltaTime * Vector3.up;
    }

    void OnTriggerEnter(Collider other)
    {
        hasBeenHit = true;
    
        float speed = Vector3.Dot(racketFace.position - lastTransform, racketFace.forward) / Time.deltaTime;

        currentVelocity = -speedMultiplier * speed * racketFace.forward;
    }

    private float timeAtGroundHit()
    {
        // shoutout to Brendan for this equation
        return (currentVelocity.y + Mathf.Sqrt(currentVelocity.y * currentVelocity.y + 2 * gravityAccel * transform.position.y)) / gravityAccel;
    }

    public Vector3 locationAtGroundHit()
    {
        float time = timeAtGroundHit();

        return transform.position + currentVelocity * time + 0.5f * gravityAccel * time * time * Vector3.up;
    }
}
