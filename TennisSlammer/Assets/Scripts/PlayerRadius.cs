using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental.FileFormat;
using UnityEngine;

public class PlayerRadius : MonoBehaviour
{
    private PlayerManualTarget _player;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (other.GetComponent<Enemy>().enemyType == EnemyType.enemy)
            {
             //   Vector3 dir = other.ClosestPoint(transform.position) - transform.position;
              //  dir.Normalize();
               // _player.GetComponent<CharacterController>().Move(dir * _player.PushBackStrength * Time.deltaTime);
                _player.Health--;
                _player.HealthText.text = _player.Health.ToString() + "/3";
            }
        }
    }

    private void Start()
    {
        _player = GetComponentInParent<PlayerManualTarget>();
    }
}
