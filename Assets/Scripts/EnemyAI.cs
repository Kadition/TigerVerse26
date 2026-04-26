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

    [SerializeField] private Animator animator;

    private Vector3 ballCollisionPosition;

    int animateForward;
    int animateBackward;
    int animateRight;
    int animateLeft;
    

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
        animateForward = Animator.StringToHash("Forward");
        animateBackward = Animator.StringToHash("Backward");
        animateRight = Animator.StringToHash("Right");
        animateLeft = Animator.StringToHash("Left");
    }

    // Update is called once per frame
    void Update()
    {
        if(!BallCollision.instance.canHit)
        {
            return;
        }

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
            animator.SetBool(animateForward, false);
            animator.SetBool(animateBackward, false);
            animator.SetBool(animateRight, false);
            animator.SetBool(animateLeft, false);
        }
        else
        {

            // if(direction.magnitude > 0.02f)
            {
                if(Mathf.Abs(direction.x) > Mathf.Abs(direction.z))
                {
                    if(direction.x > 0)
                    {
                        Debug.Log("right");
                        animator.SetBool(animateRight, true);
                        animator.SetBool(animateForward, false);
                        animator.SetBool(animateBackward, false);
                        animator.SetBool(animateLeft, false);
                    }
                    else
                    {
                        Debug.Log("left");
                        animator.SetBool(animateLeft, true);
                        animator.SetBool(animateRight, false);
                        animator.SetBool(animateForward, false);
                        animator.SetBool(animateBackward, false);
                    }
                }
                else
                {
                    if(direction.z > 0)
                    {
                        Debug.Log("forward");
                        animator.SetBool(animateForward, true);
                        animator.SetBool(animateBackward, false);
                        animator.SetBool(animateRight, false);
                        animator.SetBool(animateLeft, false);
                    }
                    else
                    {
                        Debug.Log("backward");
                        animator.SetBool(animateBackward, true);
                        animator.SetBool(animateForward, false);
                        animator.SetBool(animateRight, false);
                        animator.SetBool(animateLeft, false);
                    }
                }
            }
        
            transform.position = transform.position + Time.deltaTime * movementSpeed * direction.normalized;  
        }

        if((BallCollision.instance.locationIn(ballCollisionPosition) || BallCollision.instance.doubleBounce) && Vector3.Distance(transform.position, BallCollision.instance.transform.position) < distanceToHit)
        {
            BallCollision.instance.opponentHit(false);

            if(BallCollision.instance.transform.position.x - transform.position.x > 0)
            {
                animator.SetTrigger("SwingRight"); 
            }
            else
            {
                animator.SetTrigger("SwingLeft"); 
            }
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
        StartCoroutine(serve());
    }

    private IEnumerator serve()
    {
        while(true)
        {
            Vector3 directionStandard = new Vector3(servePositionX - transform.position.x, 0, servePositionZ - transform.position.z);

            if(directionStandard.magnitude < 0.02f)
            {
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
