using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private BallCollision ballCollision;

    private const float movementSpeed = 0.5f;

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

        ballCollisionPosition = ballCollision.locationAtGroundHit();

        transform.position = transform.position + Time.deltaTime * movementSpeed * new Vector3(transform.position.x - ballCollisionPosition.x, 0, transform.position.z - ballCollisionPosition.z);
    }
}
