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

    // Update is called once per frame
    void Update()
    {
        // Get local hand position/rotation
        Vector3 localHandPos = rightHandPosition.action.ReadValue<Vector3>();
        transform.rotation = rightHandRotation.action.ReadValue<Quaternion>();

        transform.position = headsetRigTransform.TransformPoint(localHandPos);
    }
}
