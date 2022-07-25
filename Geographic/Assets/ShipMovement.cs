using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(ShipInput))]
public class ShipMovement : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private ShipInput _shipInput;
    private Quaternion _smothedRotation;
    private Quaternion _targetRotation;

    [SerializeField] private float _topSpeed;
    [SerializeField] private float _topSpeedBackwards;
    [SerializeField] private float _verticalSpeed;
    [SerializeField] private float _horizontalSpeed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _yawSpeed;
    [SerializeField] private float _pitchSpeed;
    [SerializeField] private float _rollSpeed;

    [SerializeField] private float _enginSpeed;
    [SerializeField] private float _enginSpeedIncrement;

    [SerializeField] private bool _cruiseControl;
    [SerializeField] private bool _flightAssist;
    [SerializeField] private Vector3 _flistAssist;
    [SerializeField] private Vector3 _flightAssistTorque;

    private void Awake()
    {
        _shipInput = GetComponent<ShipInput>();

        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.useGravity = false;
        _rigidbody.isKinematic = false;
        _rigidbody.drag = 0f;

        _targetRotation = Quaternion.identity;
        _smothedRotation = Quaternion.identity;
    }

    private void FixedUpdate()
    {
        Vector3 force = new Vector3(
            _shipInput.Right * _horizontalSpeed,
            _shipInput.Up * _verticalSpeed,
            _enginSpeed);

        _rigidbody.AddRelativeForce(force, ForceMode.Impulse);

        Vector3 torque = new Vector3(
            _shipInput.Pitch * _pitchSpeed,
            _shipInput.Yaw * _yawSpeed,
            _shipInput.Roll * _rollSpeed);

        _rigidbody.AddRelativeTorque(torque, ForceMode.VelocityChange);

        _flistAssist = FlightAssistLinear();
        _flightAssistTorque = FlightAssistAngular();

        if (_flightAssist)
        {
            _rigidbody.AddForce(_flistAssist - _rigidbody.velocity, ForceMode.VelocityChange);
            _rigidbody.AddRelativeTorque(_flightAssistTorque - _rigidbody.angularVelocity, ForceMode.VelocityChange);
        }
    }

    private void Update()
    {
        UpdateEnginSpeed();

        if (_shipInput.FlightAssist)
            _flightAssist = !_flightAssist;

        if (_shipInput.CruiseControl)
            _cruiseControl = !_cruiseControl;
    }

    private void UpdateEnginSpeed()
    {
        _enginSpeed += _shipInput.Forward * _enginSpeedIncrement * Time.deltaTime;
        _enginSpeed = Mathf.Clamp(_enginSpeed, -_topSpeedBackwards, _topSpeed);

        if (!_cruiseControl && _shipInput.Forward == 0f)
            _enginSpeed = Mathf.Lerp(_enginSpeed, 0, _enginSpeedIncrement * Time.deltaTime);
    }

    public Vector3 FlightAssistLinear()
    {
        Vector3 velocity = _rigidbody.velocity;
        float enginForce = _enginSpeed < 0 ? _topSpeed : _topSpeedBackwards;

        return new Vector3(
            _shipInput.Right == 0 ? Mathf.Lerp(velocity.x, 0f, _horizontalSpeed * Time.deltaTime) : velocity.x,
            _shipInput.Up == 0 ? Mathf.Lerp(velocity.y, 0f, _verticalSpeed * Time.deltaTime) : velocity.y,
            Mathf.Abs(_enginSpeed) <= 0.01f && !_cruiseControl ? Mathf.Lerp(velocity.z,0f, enginForce) : velocity.z);
    }

    public Vector3 FlightAssistAngular()
    {
        Vector3 velocity = _rigidbody.angularVelocity;

        return new Vector3(
            _shipInput.Pitch == 0 ? Mathf.Lerp(velocity.x, 0f, _pitchSpeed) : velocity.x,
            _shipInput.Yaw == 0 ? Mathf.Lerp(velocity.y, 0f, _yawSpeed) : velocity.y,
            _shipInput.Roll == 0 ? Mathf.Lerp(velocity.z, 0f, _rollSpeed) : velocity.z);
    }
}