using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMainSkillCollider : MonoBehaviour
{
    public GameObject particle;


    private void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("CastleAttackTarget2"))
        {
            ShakeCamera.Instance.StartShakeCamera();
            UIManager.Instance.ShowDamageImage();
            CastleHealth.Instance.SubHealth(1);
            other.GetComponent<AudioSource>().Stop();
            other.GetComponent<AudioSource>().Play();
            GameObject go = Instantiate(particle,transform);
            go.transform.SetParent(null);
            Destroy(gameObject);
        }
    }
}
