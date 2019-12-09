using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordColllision : MonoBehaviour
{
    public GameObject attackParticle;//攻击特效
    public Transform particleTrans;

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
            //attackParticle.Play();
            GameObject particle = Instantiate(attackParticle)as GameObject;
            particle.transform.position = particleTrans.position;
            GetComponent<CapsuleCollider>().enabled = false;
        }
    }
}
