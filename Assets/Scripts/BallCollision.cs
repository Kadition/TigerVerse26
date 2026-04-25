using UnityEngine;

public class BallCollision : MonoBehaviour
{
    public Vector3 predictedBounceLocation;

    private const float gravityAccel = -9.81f;

    private const float speedMultiplier = 0.1f;

    private const float xSides = 3.333f;

    private const float zSides = 8.162f;

    private const float netHeight = 0.72f;

    [SerializeField] private Transform racketFace;

    private Vector3 lastTransform;

    private Vector3 currentVelocity;

    public bool hasBeenHit = false;

    bool playerLastHit = false;

    private bool doubleBounce = false;

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

        // in bounds and hit ground
        if(transform.position.y < 0 && transform.position.x < xSides && transform.position.x > -xSides && transform.position.z < zSides && transform.position.z > -zSides)
        {
            // you hit your own ground or it bounce twice in yours
            if((playerLastHit && transform.position.z > 0) || (!playerLastHit && transform.position.z > 0 && doubleBounce))
            {
                // TODO - opponent wins
                Debug.Log("Opponent wins");
                Destroy(gameObject);
                return;
            }
            else if((!playerLastHit && transform.position.z < 0) || (playerLastHit && transform.position.z < 0 && doubleBounce))
            {
                // TODO - PLAYER WINS
                Debug.Log("Player wins");
                Destroy(gameObject);
                return;
            }

            doubleBounce = true;
            currentVelocity.y = -currentVelocity.y * 0.8f;
        }
        // out of bounds
        else if(transform.position.y < 0)
        {
            if(playerLastHit)
            {
                // TODO - opponent wins
                Debug.Log("Opponent wins");
                Destroy(gameObject);
                return;
            }
            else
            {
                // TODO - PLAYER WINS
                Debug.Log("Player wins");
                Destroy(gameObject);
                return;
            }
        }
        // went into net
        else if(transform.position.z <= 0 && lastTransform.z >= 0 && transform.position.y < netHeight)
        {
            // TODO - opponent wins
            Debug.Log("Opponent wins");
            Destroy(gameObject);
            return;
        }
        else if(transform.position.z >= 0 && lastTransform.z <= 0 && transform.position.y < netHeight)
        {
            // TODO - PLAYER WINS
            Debug.Log("Player wins");
            Destroy(gameObject);
            return;
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if(playerLastHit)
        {
            // TODO - opponent won
            Destroy(gameObject);
            return;
        }

        doubleBounce = false;

        playerLastHit = true;

        hasBeenHit = true;
    
        float speed = Vector3.Dot(racketFace.position - lastTransform, racketFace.forward) / Time.deltaTime;

        currentVelocity = -speedMultiplier * speed * racketFace.forward;
    }

    public void opponentHit()
    {
        playerLastHit = false;

        doubleBounce = false;

        hasBeenHit = true;

        // TODO - either here or in th eenemy ai, make up the speed and velocity
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
