using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    public float MovementSpeed = 17;
    public float RotationSpeed = 160;
    public float JumpHeight = 5;

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
