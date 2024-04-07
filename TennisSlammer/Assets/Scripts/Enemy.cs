using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float HitForce = 15;
    public float UpForce = 10;
    public float Distance = 10;
    public float Speed;
    public int Health = 1;

    public bool IsCurrentTarget = false;

    public Material TargetMat;
    public GameObject[] TargetIndicators;

    private float _timer;
    private MeshRenderer _render;
    private Material _deafultMat;
    private PlayerManualTarget _playerScript;
    private GameObject _player;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            Health--;
            Vector3 direction = (transform.position + (transform.forward * Distance))- transform.position;
            collision.gameObject.GetComponent<Rigidbody>().velocity = direction.normalized * HitForce + new Vector3(0, UpForce, 0);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        _playerScript = FindObjectOfType<PlayerManualTarget>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _render = GetComponent<MeshRenderer>();
        _deafultMat = _render.sharedMaterial;

        Speed = Random.Range(2, 4);
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsCurrentTarget)
        {
            _render.sharedMaterial = _deafultMat;
            foreach (GameObject go in TargetIndicators)
            {
                go.SetActive(false);
            }
        }
        else
        {
            _timer += Time.deltaTime;
            _render.sharedMaterial = TargetMat;

            if (_timer < 3)
            {
                _timer = 0;
                IsCurrentTarget = false;
            }
        }

        if (Health == 0)
        {
            _playerScript.Enemies.Remove(this.gameObject);
            Destroy(gameObject);
        }

        if (!isGrounded())
        {
            transform.position -= Vector3.up * 2 * Time.deltaTime;
        }
    }
    private void LateUpdate()
    {
        if(isGrounded() && !PlayerTooFar()) 
        {
            Vector3 targetPos =  _player.transform.position - transform.position ;
            targetPos.Normalize();
            transform.Translate(targetPos * Speed * Time.deltaTime);
        }
    }
    private bool isGrounded()
    {
        Debug.DrawRay(new Vector3(transform.position.x, transform.position.y - (transform.localScale.y / 2), transform.position.z), -Vector3.up, Color.red);
        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y - (transform.localScale.y / 2), transform.position.z), -Vector3.up, 0.4f))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool PlayerTooFar()
    {
        if (Vector3.Distance(_player.transform.position,transform.position) > 25)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
