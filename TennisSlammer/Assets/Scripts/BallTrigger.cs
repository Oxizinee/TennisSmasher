using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTrigger : MonoBehaviour
{

    private PlayerManualTarget _playerManual;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ball")
        {
            Vector3 direction = _playerManual.AimPos.position - transform.position;
            other.gameObject.GetComponent<Rigidbody>().velocity = direction.normalized * _playerManual.HitForce + new Vector3(0, _playerManual.UpForce, 0);
        }
    }

    private void Start()
    {
      
       _playerManual = GetComponentInParent<PlayerManualTarget>();
    }
}
