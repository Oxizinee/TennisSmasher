using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
using Cinemachine;
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
   public CinemachineTargetGroup cinemaCam;
    public GameObject secondCam;
    public GameObject cam;
    public LayerMask layerMask;
    public GameObject currentTarget = null;
    public int currentAim = 0;

    public List<GameObject> Enemies;

    private bool _isFocusedOnTarget;
    private Vector2 _rotateInput, _input;
    private Vector3 _moveVector, _inputDirection;
    private float _verticalVel, _gravity = 12, _isJumpingValue, _isSwitchingTargetValue;
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
    private void OnTurnTarget()
    {
        _isFocusedOnTarget = !_isFocusedOnTarget;
    }

    private void OnSwitchTarget(InputValue value)
    {
        //_isSwitchingTargetValue = value.Get<float>();
        currentAim++;
        if (currentAim > Enemies.Count)
        {
            currentAim = 0;
        }
    }
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        Enemies = new List<GameObject>();
        secondCam.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
        Movement();

        RaycastHit info;
        _inputDirection = Camera.main.transform.forward * _input.y + Camera.main.transform.right * _input.x;

        if(Enemies.Count > 0 && _isFocusedOnTarget)
        {
            SwitchCameras(false);
        }
        else if(!_isFocusedOnTarget)
        {
            SwitchCameras(true);
        }

        if (Enemies.Count > 0 && secondCam.activeSelf == true) 
        {
            currentTarget = Enemies[currentAim];
            currentTarget.GetComponent<Enemy>().IsCurrentTarget = true;
            AimPos = currentTarget.transform;

            cinemaCam.m_Targets[1].target = currentTarget.transform;
            cinemaCam.m_Targets[1].weight = 1;
            cinemaCam.m_Targets[1].radius = 4;
        }

        if(Enemies.Count == 0 && secondCam.activeSelf == true)
        {
            SwitchCameras(true);
        }


        //Find targets with normal cam
        if (cam.activeSelf == true &&  Physics.SphereCast(transform.position, 3f, cam.transform.forward, out info, 50, layerMask))
        {
            if (info.collider.gameObject.tag == "Enemy")
            {
                currentTarget = info.collider.gameObject;
                AimPos = currentTarget.transform;
                info.collider.gameObject.GetComponent<Enemy>().IsCurrentTarget = true;
            }
        }
    }

    private void SwitchCameras(bool firstCamBool)
    {
        cam.SetActive(firstCamBool);
        secondCam.SetActive(!firstCamBool);
    }
    private void Rotate()
    {
        Camera.main.transform.RotateAround(transform.position, Vector3.up, _rotateInput.x * RotationSpeed * Time.deltaTime);

        if(secondCam.activeSelf == true) 
        {
            transform.Rotate(Vector3.up, _rotateInput.x * RotationSpeed * Time.deltaTime);
        }
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
