using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SmoothCameraFollow : MonoBehaviour
{
    private Vector3 _offset;
    private Vector3 _currentVelocity = Vector3.zero;

    [SerializeField] private Transform target;
    [SerializeField] private float smoothTime;

    public InputAction cameraControls;

    public void Start()
    {
        cameraControls.Enable();
        cameraControls.performed += moveCamera;
    }

    private void Awake()
    {
        _offset = transform.position - target.position;
    }

    private void LateUpdate()
    {
        Vector3 targetPosition = target.position + _offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _currentVelocity, smoothTime);
    }

    private void moveCamera(InputAction.CallbackContext context)
    {
    }
}
