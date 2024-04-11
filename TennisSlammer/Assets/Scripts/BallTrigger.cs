using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTrigger : MonoBehaviour
{

    private PlayerManualTarget _playerManual;
    public GameObject rocket;
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Ball")
        {
            rocket.SetActive(true);
            if (_playerManual.IsSmashingValue == 1)
            {
                Vector3 direction = _playerManual.AimPos.position - transform.position;
                other.gameObject.GetComponent<Rigidbody>().velocity = direction.normalized * _playerManual.HitForce + new Vector3(0, _playerManual.UpForce, 0);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Ball")
        {
            rocket.SetActive(false);
            
        }
    }

    private void Start()
    {
      
       _playerManual = GetComponentInParent<PlayerManualTarget>();
    }
}
