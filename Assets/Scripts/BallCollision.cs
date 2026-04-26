using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class BallCollision : MonoBehaviour
{
    public static BallCollision instance;

    private const float gravityAccel = -9.81f;

    private const float speedMultiplier = 2.8f;

    public const float xSides = 3.333f;

    public const float zSides = 8.162f;

    private const float netHeight = 0.72f;

    [SerializeField] private Transform racketHandle;

    private Vector3 lastRacketTransform;

    private Vector3 lastRacketFaceTransform;

    private Vector3 lastTransform;

    private Vector3 secondToLastRacketTransform;

    private Vector3 secondToLastRacketFaceTransform;

    private Vector3 currentVelocity;

    public bool hasBeenHit {get; private set;} = false;

    public bool playerLastHit {get; private set;} = false;

    public bool doubleBounce {get; private set;} = false;

    [SerializeField] private Transform racketPoint;

    [SerializeField] private InputActionReference hapticDevice;

    public AudioSource BallAudioSource;
    public AudioClip Bounce;
    public AudioClip Hit;

    [SerializeField] private AudioClip netSound;

    [SerializeField] private AudioSource netAudioSource;

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
        lastRacketFaceTransform = HandTracking.instance.racketFace.transform.position;
        secondToLastRacketFaceTransform = HandTracking.instance.racketFace.transform.position;
        lastRacketTransform = racketHandle.position;
        secondToLastRacketTransform = racketHandle.position;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Vector3.Distance(lastRacketFaceTransform, secondToLastRacketFaceTransform));

        HandTracking.instance.racketCollider.size = new Vector3(HandTracking.instance.racketCollider.size.x, Mathf.Lerp(1.2f, 6f, Mathf.InverseLerp(0.05f, 0.4f, Vector3.Distance(lastRacketFaceTransform, secondToLastRacketFaceTransform))), HandTracking.instance.racketCollider.size.z);

        if(!hasBeenHit)
        {
            lastTransform = transform.position;
            secondToLastRacketTransform = lastRacketTransform;
            lastRacketTransform = racketHandle.position;
            secondToLastRacketFaceTransform = lastRacketFaceTransform;
            lastRacketFaceTransform = HandTracking.instance.racketFace.transform.position;
            return;
        }

        transform.position = transform.position + currentVelocity * Time.deltaTime + 0.5f * gravityAccel * Time.deltaTime * Time.deltaTime * Vector3.up;

        Vector3 forceMove = Vector3.zero;

        if(playerLastHit)
        {
            Vector3 pointVector = Vector3.ProjectOnPlane(racketPoint.forward, Vector3.up).normalized;

            forceMove = new Vector3(pointVector.x, 0, pointVector.z) * 1.5f;
        }

        currentVelocity += gravityAccel * Time.deltaTime * Vector3.up + forceMove * Time.deltaTime;

        if (transform.position.y < 0)
        {
            BallAudioSource.PlayOneShot(Bounce);
        }
        // in bounds and hit ground
        if(transform.position.y < 0 && locationIn(transform.position))
        {
            // you hit your own ground or it bounce twice in yours
            if ((playerLastHit && transform.position.z > 0) || (!playerLastHit && transform.position.z > 0 && doubleBounce))
            {
                ScoreTracker.instance.RecordPoint(false);
                // TODO - opponent wins
                Debug.Log("Opponent wins1");
                resetBall();
                return;
            }
            else if ((!playerLastHit && transform.position.z < 0) || (playerLastHit && transform.position.z < 0 && doubleBounce))
            {
                ScoreTracker.instance.RecordPoint(true);
                // TODO - PLAYER WINS
                Debug.Log("Player wins");
                resetBall();
                return;
            }

            doubleBounce = true;
            currentVelocity.y = -currentVelocity.y * 0.8f;

            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }
        else if(transform.position.y < 0 && doubleBounce)
        {
            if(!playerLastHit)
            {
                ScoreTracker.instance.RecordPoint(false);
                // TODO - opponent wins
                Debug.Log("Opponent wins2");
                resetBall();
                return;
            }
            else
            {
                ScoreTracker.instance.RecordPoint(true);
                // TODO - PLAYER WINS
                Debug.Log("Player wins");
                resetBall();
                return;
            }
        }
        // out of bounds
        else if(transform.position.y < 0)
        {
            if(playerLastHit)
            {
                ScoreTracker.instance.RecordPoint(false);
                // TODO - opponent wins
                Debug.Log("Opponent wins2");
                resetBall();
                return;
            }
            else
            {
                ScoreTracker.instance.RecordPoint(true);
                // TODO - PLAYER WINS
                Debug.Log("Player wins");
                resetBall();
                return;
            }
        }
        
        // went into net
        if(transform.position.z <= 0 && lastTransform.z >= 0 && transform.position.y < netHeight)
        {
            netAudioSource.PlayOneShot(netSound);
            ScoreTracker.instance.RecordPoint(false);
            // TODO - opponent wins
            Debug.Log("Opponent wins3");
            resetBall();
            return;
        }
        else if(transform.position.z >= 0 && lastTransform.z <= 0 && transform.position.y < netHeight)
        {
            netAudioSource.PlayOneShot(netSound);
            ScoreTracker.instance.RecordPoint(true);
            // TODO - PLAYER WINS
            Debug.Log("Player wins");
            resetBall();
            return;
        }

        lastTransform = transform.position;
        secondToLastRacketTransform = lastRacketTransform;
        lastRacketTransform = racketHandle.position;
        secondToLastRacketFaceTransform = lastRacketFaceTransform;
        lastRacketFaceTransform = HandTracking.instance.racketFace.transform.position;
    }

    void OnTriggerEnter(Collider other)
    {
        XRControllerWithRumble controller = hapticDevice.action.activeControl.device as XRControllerWithRumble;

        controller.SendImpulse(1, 0.1f);

        BallAudioSource.PlayOneShot(Hit);
        if (playerLastHit)
        {
            ScoreTracker.instance.RecordPoint(false);
            // TODO - opponent won
            Debug.Log("Opponent wins4");
            resetBall();
            return;
        }

        doubleBounce = false;

        playerLastHit = true;

        hasBeenHit = true;
    
        float speed = Vector3.Dot(lastRacketTransform - secondToLastRacketTransform, HandTracking.instance.racketFace.forward) / Time.deltaTime;

        currentVelocity = speedMultiplier * speed * HandTracking.instance.racketFace.forward;

        // Debug.Log(currentVelocity);
    }

    public void opponentHit()
    {
        BallAudioSource.PlayOneShot(Hit);
        playerLastHit = false;

        doubleBounce = false;

        hasBeenHit = true;

        // the chance the enemy just straight flubs it
        if(Random.Range(0, EnemyAI.flubChance) == 0)
        {
            currentVelocity = new Vector3(Random.Range(-1f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        }
        else
        {
            if(transform.position.x >= 2.8)
            {
                currentVelocity = new Vector3(Random.Range(-2.5f, 0.2f), Random.Range(4f, 6f), Random.Range(6f, 8f));
            }
            else if(transform.position.x <= -2.8)
            {
                currentVelocity = new Vector3(Random.Range(-0.2f, 2.5f), Random.Range(4f, 6f), Random.Range(6f, 8f));
            }
            else
            {
                currentVelocity = new Vector3(Random.Range(-1.8f, 1.8f), Random.Range(4f, 6f), Random.Range(6f, 8f));
            }
        }
    }

    private float timeAtGroundHit()
    {
        if (((-currentVelocity.y + Mathf.Sqrt(currentVelocity.y * currentVelocity.y - 2 * gravityAccel * transform.position.y)) / gravityAccel) < 0)
        {
            return (-currentVelocity.y - Mathf.Sqrt(currentVelocity.y * currentVelocity.y - 2 * gravityAccel * transform.position.y)) / gravityAccel;
        }

        // shoutout to Brendan for this equation
        return (-currentVelocity.y + Mathf.Sqrt(currentVelocity.y * currentVelocity.y - 2 * gravityAccel * transform.position.y)) / gravityAccel;
    }

    public Vector3 locationAtGroundHit()
    {
        float time = timeAtGroundHit();

        return new Vector3(transform.position.x + currentVelocity.x * time, 0f, transform.position.z + currentVelocity.z * time);
    }

    private void resetBall()
    {
        hasBeenHit = false;

        playerLastHit = false;

        doubleBounce = false;

        currentVelocity = Vector3.zero;

        transform.position = new Vector3(0, -8, 0);
    }

    public void respawnBall()
    {
        resetBall();

        transform.position = new Vector3(0, 0.8f, 5);
    }

    public bool locationIn(Vector3 transform)
    {
        return transform.x < xSides && transform.x > -xSides && transform.z < zSides && transform.z > -zSides;
    }
}
