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
        if(!BallCollision.instance.canHit)
        {
            animator.SetFloat("MoveX", 0);
            animator.SetFloat("MoveY", 0);
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

        if(direction.magnitude < 0.02f)
        {
            transform.position = new Vector3(ballCollisionPosition.x, transform.position.y, ballCollisionPosition.z);
            animator.SetFloat("MoveX", 0);
            animator.SetFloat("MoveY", 0);
        }
        else
        {

            if(direction.magnitude > 0.02f)
            {
                if(Mathf.Abs(direction.x) > Mathf.Abs(direction.z))
                {
                    if(direction.x > 0)
                    {
                        // Debug.Log("right");
                        animator.SetFloat("MoveX", 1);
                        animator.SetFloat("MoveY", 0);
                    }
                    else
                    {
                        // Debug.Log("left");
                        animator.SetFloat("MoveX", -1);
                        animator.SetFloat("MoveY", 0);
                    }
                }
                else
                {
                    if(direction.z > 0)
                    {
                        // Debug.Log("forward");
                        animator.SetFloat("MoveX", 0);
                        animator.SetFloat("MoveY", 1);
                    }
                    else
                    {
                        // Debug.Log("backward");
                        animator.SetFloat("MoveX", 0);
                        animator.SetFloat("MoveY", -1);
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
        animator.SetFloat("MoveX", 0);
        animator.SetFloat("MoveY", 0);

        Vector3 directionStandard = new Vector3(-transform.position.x, 0, sitPosition - transform.position.z);

        if(directionStandard.magnitude < 0.02f)
        {
            animator.SetFloat("MoveX", 0);
            animator.SetFloat("MoveY", 0);
            transform.position = new Vector3(0, transform.position.y, sitPosition);
        }
        else
        {
            if(Mathf.Abs(directionStandard.x) > Mathf.Abs(directionStandard.z))
            {
                if(directionStandard.x > 0)
                {
                    // Debug.Log("right");
                    animator.SetFloat("MoveX", 1);
                    animator.SetFloat("MoveY", 0);
                }
                else
                {
                    // Debug.Log("left");
                    animator.SetFloat("MoveX", -1);
                    animator.SetFloat("MoveY", 0);
                }
            }
            else
            {
                if(directionStandard.z > 0)
                {
                    // Debug.Log("forward");
                    animator.SetFloat("MoveX", 0);
                    animator.SetFloat("MoveY", 1);
                }
                else
                {
                    // Debug.Log("backward");
                    animator.SetFloat("MoveX", 0);
                    animator.SetFloat("MoveY", -1);
                }
            }
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
                animator.SetFloat("MoveX", 0);
                animator.SetFloat("MoveY", 0);
                transform.position = new Vector3(servePositionX, transform.position.y, servePositionZ);

                yield return new WaitForSeconds(0.8f);

                BallCollision.instance.opponentHit(true);
                animator.SetTrigger("SwingRight"); 

                yield break;
            }
            else
            {
                if(Mathf.Abs(directionStandard.x) > Mathf.Abs(directionStandard.z))
                {
                    if(directionStandard.x > 0)
                    {
                        // Debug.Log("right");
                        animator.SetFloat("MoveX", 1);
                        animator.SetFloat("MoveY", 0);
                    }
                    else
                    {
                        // Debug.Log("left");
                        animator.SetFloat("MoveX", -1);
                        animator.SetFloat("MoveY", 0);
                    }
                }
                else
                {
                    if(directionStandard.z > 0)
                    {
                        // Debug.Log("forward");
                        animator.SetFloat("MoveX", 0);
                        animator.SetFloat("MoveY", 1);
                    }
                    else
                    {
                        // Debug.Log("backward");
                        animator.SetFloat("MoveX", 0);
                        animator.SetFloat("MoveY", -1);
                    }
                }
                transform.position = transform.position + Time.deltaTime * movementSpeed * directionStandard.normalized;   
            }

            yield return null;
        }
    }
}
