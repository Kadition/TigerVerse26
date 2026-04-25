using UnityEngine;

public class BallCollision : MonoBehaviour
{
    public Vector3 predictedBounceLocation;

    private const float gravityAccel = -9.81f;

    [SerializeField] private Transform racketFace;

    private Vector3 lastTransform;

    private Vector3 currentVelocity;

    bool hasBeenHit = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lastTransform = racketFace.transform.position;
    }

    //futurePosition = currentPosition + velocity * time + 0.5 * acceleration * time^2

    // Update is called once per frame
    void Update()
    {
        if(!hasBeenHit)
        {
            return;
        }

        transform.position = transform.position + currentVelocity * Time.deltaTime + 0.5f * gravityAccel * Time.deltaTime * Time.deltaTime * transform.up;
    }

    void OnTriggerEnter(Collider other)
    {
        float speed = Vector3.Dot(racketFace.position - lastTransform, racketFace.forward) / Time.deltaTime;

        currentVelocity = speed * racketFace.forward;
    }

    float timeAtGroundHit()
    {
        return (currentVelocity.y + Mathf.Sqrt(currentVelocity.y * currentVelocity.y + 2 * gravityAccel * transform.position.y)) / gravityAccel;
    }
}
