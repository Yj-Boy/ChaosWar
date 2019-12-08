using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public float moveSpeed;

    public GameObject arrowParticle;

    // Start is called before the first frame update
    void Start()
    {
        arrowParticle.GetComponent<ParticleSystem>().Stop() ;
    }

    // Update is called once per frame
    void Update()
    {
        ArrowMove();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            arrowParticle.transform.SetParent(null);
            arrowParticle.GetComponent<ParticleSystem>().Play();
            other.GetComponent<EnemyHealth>().TakeDamage(20);
            transform.parent.GetComponent<TroopShooterController>().ChangeShootToIdle();
            Destroy(gameObject);
        }
    }

    private void ArrowMove()
    {
        transform.Translate(
            Vector3.forward * moveSpeed * Time.deltaTime,
            Space.Self        
            );
    }
}
