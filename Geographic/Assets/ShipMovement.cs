using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ShipMovement : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Quaternion _smothedRotation;
    private Quaternion _targetRotation;

    [SerializeField] private float _topSpeed;
    [SerializeField] private float _topSpeedBackwards;
    [SerializeField] private float _elevationSpeed;
    [SerializeField] private float _horizontalSpeed;
    [SerializeField] private float _rotationSpeed;

    [SerializeField] private float _targetVelocityFWD;
    [SerializeField] private float _targetVelocityHOR;
    [SerializeField] private float _targetVelocityVERT;

    [SerializeField] private float _targetVelocityAcc;
    [SerializeField] private bool _cruiseControl;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.useGravity = false;
        _rigidbody.isKinematic = false;

        _targetRotation = Quaternion.identity;
        _smothedRotation = Quaternion.identity;
    }

    private void FixedUpdate()
    {
        Vector3 force =
            transform.forward * _targetVelocityFWD +
            transform.up * _targetVelocityVERT +
            transform.right * _targetVelocityHOR;

        _rigidbody.AddForce(force, ForceMode.Impulse);
        _rigidbody.MoveRotation(_smothedRotation);
    }

    private void Update()
    {
        HandleRotation();
        HandleMainThuster();
        HandleThust();
    }

    private void HandleRotation()
    {
        float yawInput = Input.GetAxis("Mouse X");
        float pitchInput = Input.GetAxis("Mouse Y");

        _targetRotation = Quaternion.Euler(
            transform.up * yawInput +
            transform.right * pitchInput) * _targetRotation;

        _smothedRotation = Quaternion.Slerp(transform.rotation, _targetRotation, _rotationSpeed);
    }

    private void HandleMainThuster()
    {
        if (Input.GetKey(KeyCode.Alpha0))
            _targetVelocityFWD = 0f;

        if (Input.GetKeyDown(KeyCode.Tab))
            _cruiseControl = !_cruiseControl;

        float v = Input.GetAxis("Vertical");

        _targetVelocityFWD += v * _targetVelocityAcc * Time.deltaTime;
        _targetVelocityFWD = Mathf.Clamp(_targetVelocityFWD, _topSpeedBackwards, _topSpeed);

        if (!_cruiseControl && v == 0f)
            _targetVelocityFWD = Mathf.Lerp(_targetVelocityFWD, 0, _targetVelocityAcc * Time.deltaTime);
    }

    private void HandleThust()
    {
        _targetVelocityHOR = Input.GetAxis("Horizontal") * _horizontalSpeed;
        _targetVelocityVERT = Input.GetAxis("Elevation") * _elevationSpeed;
    }
}