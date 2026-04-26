using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private const float movementSpeed = 1.8f;

    private const float distanceToHit = 1.5f;

    public const int flubChance = 20;

    public const int tryChance = 20;

    private const float sitPosition = -3f;

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
            standardSit();
            return;
        }

        if(!BallCollision.instance.playerLastHit)
        {
            standardSit();
            return;
        }

        ballCollisionPosition = BallCollision.instance.locationAtGroundHit();

        if(float.IsNaN(ballCollisionPosition.x))
        {
            Debug.LogWarning("bad");
            return;
        }

        Debug.Log(ballCollisionPosition);

        Vector3 direction = new Vector3(ballCollisionPosition.x - transform.position.x, 0, ballCollisionPosition.z - transform.position.z);

        // TODO - if close enough, go towards it
        // TODO - if ball collision position is outside of the map, go towards it, but not really far outside

        if(direction.magnitude < 0.01f)
        {
            transform.position = new Vector3(ballCollisionPosition.x, transform.position.y, ballCollisionPosition.z);
        }
        else
        {
            transform.position = transform.position + Time.deltaTime * movementSpeed * direction.normalized;   
        }

        if(Vector3.Distance(transform.position, BallCollision.instance.transform.position) < distanceToHit)
        {
            BallCollision.instance.opponentHit();
        }
    }

    void standardSit()
    {
        Vector3 directionStandard = new Vector3(-transform.position.x, 0, sitPosition - transform.position.z);

        if(directionStandard.magnitude < 0.01f)
        {
            transform.position = new Vector3(0, transform.position.y, sitPosition);
        }
        else
        {
            transform.position = transform.position + Time.deltaTime * movementSpeed * directionStandard.normalized;   
        }
    }
}
