using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDetection : MonoBehaviour
{
    private PlayerManualTarget _playerScript;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && other.gameObject.GetComponent<Enemy>().CanBeTargeted)
        {
            _playerScript.Enemies.Add(other.gameObject);
        }
       
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            _playerScript.Enemies.Remove(other.gameObject);
        }
      
    }

    private void Start()
    {
        _playerScript = GetComponentInParent<PlayerManualTarget>();
    }
}
