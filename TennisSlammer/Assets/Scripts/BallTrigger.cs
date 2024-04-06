using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerScript
{
    BatmanFirstPerson,
    FreeAimFirstPerson,
    ManualTarget
}

public class BallTrigger : MonoBehaviour
{
    public PlayerScript script;

    private PlayerManualTarget _playerManual;
    private Player _player;
    private PlayerStraightAim _playerScript;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ball")
        {
            if (script == PlayerScript.FreeAimFirstPerson)
            {
                Vector3 direction = _playerScript.AimPos.position - transform.position;
                other.gameObject.GetComponent<Rigidbody>().velocity = direction.normalized * _playerScript.HitForce + new Vector3(0, _playerScript.UpForce, 0);
            }
            else if(script == PlayerScript.BatmanFirstPerson) 
            {
                Vector3 direction = _player.AimPos.position - transform.position;
                other.gameObject.GetComponent<Rigidbody>().velocity = direction.normalized * _player.HitForce + new Vector3(0, _player.UpForce, 0);
            }
            else if (script == PlayerScript.ManualTarget)
            {
                Vector3 direction = _playerManual.AimPos.position - transform.position;
                other.gameObject.GetComponent<Rigidbody>().velocity = direction.normalized * _playerManual.HitForce + new Vector3(0, _playerManual.UpForce, 0);
            }
        }
    }

    private void Start()
    {
        if (script == PlayerScript.BatmanFirstPerson)
        {
            _player = GetComponentInParent<Player>();
        }
        else if(script == PlayerScript.FreeAimFirstPerson)
         _playerScript = GetComponentInParent<PlayerStraightAim>();   
        else if (script == PlayerScript.ManualTarget)
        {
            _playerManual = GetComponentInParent<PlayerManualTarget>();
        }
    }
}
