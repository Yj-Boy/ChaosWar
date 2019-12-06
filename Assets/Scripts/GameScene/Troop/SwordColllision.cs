using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordColllision : MonoBehaviour
{
    public ParticleSystem attackParticle;//攻击特效

    private void Start()
    {
        
    }
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.transform.CompareTag("Enemy"))
    //    {
    //        Debug.Log("SwordColllisionEnter");
    //        GetComponent<CapsuleCollider>().enabled = false;
    //    }
    //}
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            Debug.Log("SwordTriggerEnter");
            attackParticle.Play();
            GetComponent<CapsuleCollider>().enabled = false;
        }
    }
}
