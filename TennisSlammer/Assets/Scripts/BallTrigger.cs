using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTrigger : MonoBehaviour
{
    private Player _playerScript;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ball")
        {
            Vector3 direction = _playerScript.AimPos.position - transform.position;
            other.gameObject.GetComponent<Rigidbody>().velocity = direction.normalized * _playerScript.HitForce + new Vector3(0, _playerScript.UpForce, 0);
        }
    }

    private void Start()
    {
         _playerScript = GetComponentInParent<Player>();   
    }
}
