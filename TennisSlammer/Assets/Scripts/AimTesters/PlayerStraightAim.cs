using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerStraightAim : MonoBehaviour
{
    [Header("Movement")]
    public GameObject Cam;
    public float MovementSpeed = 17;
    public float RotationSpeed = 160;
    public float CameraRotationSpeed = 50;
    public float JumpHeight = 5;

    public float CamRot;

    [Header("Ball Hitting")]
    public Transform AimPos;
    public float HitForce = 13;
    public float UpForce = 6;

    private Vector2 _rotateInput, _input;
    private Vector3 _moveVector;
    private float _verticalVel, _gravity = 12, _isJumpingValue;
    private CharacterController _characterController;
    private void OnRotate(InputValue value)
    {
        _rotateInput = value.Get<Vector2>();
    }
    private void OnMove(InputValue value)
    {
        _input = value.Get<Vector2>();
    }
    private void OnJump(InputValue value)
    {
        _isJumpingValue = value.Get<float>();   
    }
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
        Movement();
    }
    private void Rotate()
    {
        CamRot = _rotateInput.y * CameraRotationSpeed * Time.deltaTime;
        float newRotationAngle = Cam.transform.rotation.eulerAngles.x - CamRot;
        Cam.transform.localRotation = Quaternion.Euler(Mathf.Clamp(newRotationAngle, 0, 13), Cam.transform.localRotation.eulerAngles.y, Cam.transform.localRotation.eulerAngles.z);


        transform.Rotate(0, _rotateInput.x * RotationSpeed * Time.deltaTime, 0);
    }

    private void Movement()
    {
        _moveVector = (transform.forward * _input.y + transform.right * _input.x) * MovementSpeed;
        _moveVector.y = _verticalVel;

        if (_characterController.isGrounded)
        {
            _verticalVel = -0.5f;
        }
        if (_characterController.isGrounded && _isJumpingValue == 1) //Jump
        {
            _verticalVel = JumpHeight;
        }
        else
        {
            _verticalVel -= _gravity * Time.deltaTime;
        }

        _characterController.Move(_moveVector * Time.deltaTime);
    }

}