using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStunDetection : MonoBehaviour
{
    private Enemy _enemyScript;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (other.gameObject.GetComponent<Enemy>().enemyType == EnemyType.enemy)
            {
                _enemyScript.EnemiesToStun.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (other.gameObject.GetComponent<Enemy>().enemyType == EnemyType.enemy)
            {
                _enemyScript.EnemiesToStun.Remove(other.gameObject);
            }
        }
    }
    private void Start()
    {
        _enemyScript = GetComponentInParent<Enemy>();
    }
}
