using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HandTracking : MonoBehaviour
{
    public static HandTracking instance;

    [SerializeField] private InputActionReference rightHandPosition;
    // [SerializeField] private InputActionReference leftHandPosition;
    [SerializeField] private InputActionReference rightHandRotation;
    // [SerializeField] private InputActionReference leftHandRotation;
    // this is the xr origin rig (NOT THE CAMERA)
    [SerializeField] private Transform headsetRigTransform;
    [SerializeField] private InputActionReference buttonClick;
    [SerializeField] private InputActionReference joystick;

    private const float movementSpeed = 1.5f;

    public Transform racketFace;
    public BoxCollider racketCollider;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        buttonClick.action.performed += SpawnBall;
    }

    void OnDestroy()
    {
        buttonClick.action.performed -= SpawnBall;
    }

    void Update()
    {
        if(buttonClick.action.IsPressed())
        {
            headsetRigTransform.position = new Vector3(headsetRigTransform.position.x, Mathf.Clamp(headsetRigTransform.position.y + BallCollision.instance.pointDirection() * Time.deltaTime, 0, 1), headsetRigTransform.position.z);
        }

        Vector3 localHandPos = rightHandPosition.action.ReadValue<Vector3>();
        Quaternion controllerRotation = rightHandRotation.action.ReadValue<Quaternion>();

        controllerRotation = Quaternion.Euler(-controllerRotation.eulerAngles.x, controllerRotation.eulerAngles.y, -controllerRotation.eulerAngles.z);

        transform.rotation = controllerRotation;

        transform.position = headsetRigTransform.TransformPoint(localHandPos);

        // TODO - two buttions to change height

        Vector2 joystickVector = joystick.action.ReadValue<Vector2>();

        headsetRigTransform.position = headsetRigTransform.position + Time.deltaTime * movementSpeed * new Vector3(-joystickVector.x, 0, -joystickVector.y);

        headsetRigTransform.position = new Vector3(Mathf.Clamp(headsetRigTransform.position.x, -BallCollision.xSides - 0.2f, BallCollision.xSides + 0.2f), headsetRigTransform.position.y, Mathf.Clamp(headsetRigTransform.position.z, 0.1f, BallCollision.zSides + 1f));
    }

    void SpawnBall(InputAction.CallbackContext context)
    {

    }
}