using UnityEngine;
using UnityEngine.InputSystem;

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
    [SerializeField] private GameObject ball;

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
        Vector3 localHandPos = rightHandPosition.action.ReadValue<Vector3>();
        Quaternion controllerRotation = rightHandRotation.action.ReadValue<Quaternion>();

        controllerRotation = Quaternion.Euler(-controllerRotation.eulerAngles.x, controllerRotation.eulerAngles.y, -controllerRotation.eulerAngles.z);

        transform.rotation = controllerRotation;

        transform.position = headsetRigTransform.TransformPoint(localHandPos);

    }

    void SpawnBall(InputAction.CallbackContext context)
    {
        Instantiate(ball);
    }
}