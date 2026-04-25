using UnityEngine;
using UnityEngine.InputSystem;

public class HandTracking : MonoBehaviour
{

    [SerializeField] private InputActionReference rightHandPosition;
    // [SerializeField] private InputActionReference leftHandPosition;
    [SerializeField] private InputActionReference rightHandRotation;
    // [SerializeField] private InputActionReference leftHandRotation;
    // this is the xr origin rig (NOT THE CAMERA)
    [SerializeField] private Transform headsetRigTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void Update()
    {
    Vector3 localHandPos = rightHandPosition.action.ReadValue<Vector3>();
    Quaternion controllerRotation = rightHandRotation.action.ReadValue<Quaternion>();

    controllerRotation = Quaternion.Euler(-controllerRotation.eulerAngles.x, controllerRotation.eulerAngles.y, -controllerRotation.eulerAngles.z);

    transform.rotation = controllerRotation;

    transform.position = headsetRigTransform.TransformPoint(localHandPos);
    }
}