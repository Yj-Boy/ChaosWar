using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttack : MonoBehaviour
{
    private Vector3 pos;

    NavMeshAgent nav;
    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Troop"))
        {
            GetComponent<EnemyController>().SetState(EnemyState.attack.ToString());
            pos = other.transform.position;
        }
    }

    public void Attack()
    {
        //如果攻击对象生命值大于0
        nav.SetDestination(pos);
    }
}
