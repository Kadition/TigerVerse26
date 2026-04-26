using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public static EnemyAI instance;
    private const float movementSpeed = 1.8f;

    private const float distanceToHit = 1.5f;

    public const int flubChance = 20;

    private const float sitPosition = -6f;

    private const float servePositionZ = -8.4f;

    private const float servePositionX = -0.8f;

    private Vector3 ballCollisionPosition;

    public bool haveServed {get; private set;} = false;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!BallCollision.instance.hasBeenHit || !BallCollision.instance.canHit)
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

        // if(!BallCollision.instance.locationIn(ballCollisionPosition))
        // {
        //     standardSit();
        //     return;
        // }

        // Debug.Log(ballCollisionPosition);

        Vector3 direction = new Vector3(ballCollisionPosition.x - transform.position.x, 0, ballCollisionPosition.z - transform.position.z);

        if(direction.magnitude < 0.01f)
        {
            transform.position = new Vector3(ballCollisionPosition.x, transform.position.y, ballCollisionPosition.z);
        }
        else
        {
            transform.position = transform.position + Time.deltaTime * movementSpeed * direction.normalized;   
        }

        if((BallCollision.instance.locationIn(ballCollisionPosition) || BallCollision.instance.doubleBounce) && Vector3.Distance(transform.position, BallCollision.instance.transform.position) < distanceToHit)
        {
            BallCollision.instance.opponentHit(false);
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

    public void moveToServe()
    {
        haveServed = false;
        StartCoroutine(serve());
    }

    private IEnumerator serve()
    {
        haveServed = false;

        Vector3 directionStandard = new Vector3(servePositionX - transform.position.x, 0, servePositionZ - transform.position.z);

        while(true)
        {
            if(directionStandard.magnitude < 0.02f)
            {
                haveServed = true;
                transform.position = new Vector3(servePositionX, transform.position.y, servePositionZ);

                yield return new WaitForSeconds(0.8f);

                BallCollision.instance.opponentHit(true);

                yield break;
            }
            else
            {
                transform.position = transform.position + Time.deltaTime * movementSpeed * directionStandard.normalized;   
            }

            yield return null;
        }
    }
}
