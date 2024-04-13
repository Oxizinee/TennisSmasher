using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
using Cinemachine;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEditor.Build.Content;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms.Impl;
public class PlayerManualTarget : MonoBehaviour
{
    public int Health = 3;
    public Text HealthText;
    public float PushBackStrength = 10;

    [Header("Cameras")]
    public CinemachineVirtualCamera ThirdPersonCam;
    public CinemachineVirtualCamera TargetCam;
    public CinemachineTargetGroup TargetGroupCam;
    [SerializeField] private bool _firstCamActive = true;

    [Header("Movement")]
    public float MovementSpeed = 17;
    public float RotationSpeed = 160;
    public float JumpHeight = 5;
    public float SpeedBostDuration = 3;

    [Header("Ball Hitting")]
    public Transform AimPos;
    public Transform AimObject;
    public float HitForce = 13;
    public float UpForce = 6;
    public float IsSmashingValue, IsStunningSmashingValue;

    [Header("Aiming")]
    public GameObject cam;
    public LayerMask layerMask;
    public GameObject currentTarget = null;
    public int currentAim = 0;
    public float TransitionDuration = 2;

    [Header("Serve")]
    public GameObject BallPrefab;
    public float SpawnDistance = 2;
    public int MaxBalls = 3;
    private int _ballCounter = 0; 

    public List<GameObject> Enemies;

    public Text ScoreText;
    public int Score = 0;

    private bool _isFocusedOnTarget;
    private Vector2 _rotateInput, _input;
    private Vector3 _moveVector, _inputDirection;
    private float _verticalVel, _gravity = 12, _isJumpingValue, _originalSpeed, _speedBostTimer;
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
        if(Enemies.Count > 0) 
        _isFocusedOnTarget = !_isFocusedOnTarget;
    }
    private void OnSwitchTarget(InputValue value)
    {
        currentAim++;
        if (currentAim > Enemies.Count)
        {
            currentAim = 0;
        }
    }
    private void OnSmash(InputValue value)
    {
        IsSmashingValue = value.Get<float>();
    }
    private void OnStunSmash(InputValue value)
    {
        IsStunningSmashingValue = value.Get<float>();
    }
    private void OnServe()
    {
        if (_ballCounter < MaxBalls)
        {
            GameObject Ball = Instantiate(BallPrefab, transform.position + (transform.forward * SpawnDistance), Quaternion.identity);
            _ballCounter++;
        }
    }

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        Enemies = new List<GameObject>();
        TargetCam.Priority = 0;
        ScoreText = GameObject.Find("ScoreText").GetComponent<Text>();

        HealthText.text = Health.ToString() + "/3";
        ScoreText.text = Score.ToString();

        _originalSpeed = MovementSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
        Movement();

        _inputDirection = Camera.main.transform.forward * _input.y + Camera.main.transform.right * _input.x;

        if(_isFocusedOnTarget)
        {
            SwitchCameras(false);
        }
        else
        {
            SwitchCameras(true);
        }

        if (Enemies.Count <= 0)
        {
            _isFocusedOnTarget = false;
            AimPos = AimObject.transform;
        }


        if (_firstCamActive)
        {
            FirstCamBehaviour();
        }
        else
        {
            SecondCamBehaviour();
        }

        BringTheSpeedBack();
        RestartLevel();
    }
    private void RestartLevel()
    {
        if (Health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
    private void SecondCamBehaviour()
    {
        currentTarget = Enemies[currentAim];
        currentTarget.GetComponent<Enemy>().IsCurrentTarget = true;
        foreach (GameObject go in currentTarget.GetComponent<Enemy>().TargetIndicators)
        {
            go.SetActive(true);
        }

        AimPos = currentTarget.transform;

        TargetGroupCam.m_Targets[1].target = currentTarget.transform;
        TargetGroupCam.m_Targets[1].weight = 1;
        TargetGroupCam.m_Targets[1].radius = 4;
    }

    private void FirstCamBehaviour()
    {
        RaycastHit info;

        if (Physics.SphereCast(transform.position, 3f, cam.transform.forward, out info, 50, layerMask))
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
        _firstCamActive = firstCamBool;
        if(firstCamBool) 
        {
            ThirdPersonCam.Priority = 10;
            TargetCam.Priority = 0;
        }
        else
        {
            ThirdPersonCam.Priority = 0;
            TargetCam.Priority = 10;
        }
    }

    private void Rotate()
    {

            transform.Rotate(Vector3.up, _rotateInput.x * RotationSpeed * Time.deltaTime);
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

    public void IncreaseSpeed(float moreSpeed)
    {
        float newSpeed = MovementSpeed + moreSpeed;

        MovementSpeed = newSpeed;
    }

    private void BringTheSpeedBack()
    {
        if(MovementSpeed != _originalSpeed) 
        {
            _speedBostTimer += Time.deltaTime;
            if(_speedBostTimer > SpeedBostDuration) 
            {
                MovementSpeed = _originalSpeed;
                _speedBostTimer = 0;
            }
        }
    }
}
