using UnityEngine;
using TMPro;

public class EnemyAI : MonoBehaviour
{
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
        if(!BallCollision.instance.hasBeenHit)
        {
            return;
        }

        if(!BallCollision.instance.playerLastHit)
        {
            transform.position = transform.position + Time.deltaTime * movementSpeed * new Vector3(-transform.position.x, 0, -2.5f - transform.position.z);
            return;
        }

        // TODO - if close enough, go towards it

        ballCollisionPosition = BallCollision.instance.locationAtGroundHit();

        transform.position = transform.position + Time.deltaTime * movementSpeed * new Vector3(ballCollisionPosition.x - transform.position.x, 0, ballCollisionPosition.z - transform.position.z);

        if(Vector3.Distance(transform.position, BallCollision.instance.transform.position) < distanceToHit)
        {
            BallCollision.instance.opponentHit();
        }
    }
}
