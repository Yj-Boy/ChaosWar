using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class EnemyMovement : MonoBehaviour
{
    private int targetIndex;
    private int tmpIndex;
    private float waitTime;

    EnemyMoveTarget enemyMoveTarget;
    EnemyHealth enemyHealth;
    TroopsHealth troopsHealth;
    NavMeshAgent nav;

    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        enemyMoveTarget = GameObject.Find("EnemyMoveTarget").GetComponent<EnemyMoveTarget>();
        targetIndex = Random.Range(0, enemyMoveTarget.GetLength());
        waitTime = 0;
    }

    public void Move()
    {
        if((transform.position- enemyMoveTarget.GetPosition(targetIndex)).sqrMagnitude>=14)
        {
            nav.SetDestination(enemyMoveTarget.GetPosition(targetIndex));
            //Debug.Log((transform.position - enemyMoveTarget.GetPosition(targetIndex)).sqrMagnitude);
        }
        else
        {
            waitTime += Time.deltaTime*1f;
            if(waitTime>=3)
            {
                do
                {
                    tmpIndex = targetIndex;
                    targetIndex = Random.Range(0, enemyMoveTarget.GetLength());
                    Debug.Log("tmpIndex" + tmpIndex);
                } while (tmpIndex == targetIndex);
                waitTime = 0;
            }
            
        }       
    }
}
