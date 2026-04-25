using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private BallCollision ballCollision;

    [SerializeField] private Transform ballTransform;

    private const float movementSpeed = 0.5f;

    private const float distanceToHit = 1.5f;

    public const int flubChance = 20;

    public const int tryChance = 20;

    private Vector3 ballCollisionPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!ballCollision.hasBeenHit)
        {
            return;
        }

        if(!ballCollision.playerLastHit)
        {
            // TODO - go to default position
        }

        ballCollisionPosition = ballCollision.locationAtGroundHit();

        transform.position = transform.position + Time.deltaTime * movementSpeed * new Vector3(transform.position.x - ballCollisionPosition.x, 0, transform.position.z - ballCollisionPosition.z);

        if(Vector3.Distance(transform.position, ballTransform.position) < distanceToHit)
        {
            ballCollision.opponentHit();
        }
    }
}
