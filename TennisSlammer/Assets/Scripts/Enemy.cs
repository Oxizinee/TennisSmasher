using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float HitForce = 15;
    public float UpForce = 10;
    public float Distance = 10;

    public bool IsCurrentTarget = false;

    public Material TargetMat;

    private float _timer;
    private MeshRenderer _render;
    private Material _deafultMat;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            Debug.Log("gksjkhgk");
            Vector3 direction = (transform.position + (transform.forward * Distance))- transform.position;
            collision.gameObject.GetComponent<Rigidbody>().velocity = direction.normalized * HitForce + new Vector3(0, UpForce, 0);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        _render = GetComponent<MeshRenderer>();
        _deafultMat = _render.sharedMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsCurrentTarget)
        {
            _render.sharedMaterial = _deafultMat;
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
    }
}
