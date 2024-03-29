using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float HitForce = 15;
    public float UpForce = 10;
    public float Distance = 10;
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
