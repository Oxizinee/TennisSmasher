using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerManualTarget : MonoBehaviour
{
    [Header("Movement")]
    public float MovementSpeed = 17;
    public float RotationSpeed = 160;
    public float JumpHeight = 5;

    [Header("Ball Hitting")]
    public Transform AimPos;
    public float HitForce = 13;
    public float UpForce = 6;

    [Header("Aiming")]
    public GameObject cinemaCam;
    public LayerMask layerMask;
    public GameObject currentTarget = null;
    public int currentAim = 0;

    public List<GameObject> Enemies;

    private Vector2 _rotateInput, _input;
    private Vector3 _moveVector, _inputDirection;
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
        Enemies = new List<GameObject>();
        //currentTarget = Enemies[currentAim];
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
        Movement();

        _inputDirection = Camera.main.transform.forward * _input.y + Camera.main.transform.right * _input.x;

        if (UnityEngine.Input.GetKeyDown(KeyCode.P))
        {
            currentAim++;
            if(currentAim > Enemies.Count) 
            {
                currentAim = 0;
            }
        }
        if(Enemies.Count > 0) 
        {
            currentTarget = Enemies[currentAim];
            cinemaCam.cinema
            AimPos = currentTarget.transform;
            Camera.main.transform.LookAt(currentTarget.transform);
        }
        
    }
    private void Rotate()
    {
        //transform.Rotate(0, _rotateInput.x * RotationSpeed * Time.deltaTime, 0);
        if(currentTarget != null) 
        {
            Camera.main.transform.RotateAround(currentTarget.transform.position, Vector3.up, _rotateInput.x * RotationSpeed * Time.deltaTime);
        }
        else
        Camera.main.transform.RotateAround(transform.position, Vector3.up, _rotateInput.x * RotationSpeed * Time.deltaTime);
    }

    private void Movement()
    {
        _moveVector = _inputDirection * MovementSpeed;
       // _moveVector = (transform.forward * _input.y + transform.right * _input.x) * MovementSpeed;
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
