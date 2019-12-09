/*
 *  怪物随机移动脚本 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyMovement : MonoBehaviour
{
    private int targetIndex;            //移动的目标点索引
    private int tmpIndex;               //上一个目标点索引，与当前随机的目标点比较是否相同
    private float waitTime;             //等待移动到下一个目标的时间

    EnemyMoveTarget enemyMoveTarget;    //移动目标点
    EnemyHealth enemyHealth;            //enemy血量对象
    TroopsHealth troopsHealth;          //troop血量对象
    NavMeshAgent nav;                   //NavMeshAgent对象

    private void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        enemyMoveTarget = GameObject.Find("EnemyMoveTarget").GetComponent<EnemyMoveTarget>();
        targetIndex = Random.Range(0, enemyMoveTarget.GetLength());
        Debug.Log("tmpIndex:" + tmpIndex);
        waitTime = 0;
    }

    public void Move()
    {
        if(nav.enabled==false)
        {
            nav.enabled = true;
        }
        //若enemy与目标点的距离小于某个值时，移动到目标点
        if((transform.position- enemyMoveTarget.GetPosition(targetIndex)).sqrMagnitude>=18)
        {
            if(nav.isOnNavMesh)
            {
                nav.SetDestination(enemyMoveTarget.GetPosition(targetIndex));
                GetComponent<Animator>().SetBool("DevilHeadMove", true);
            }
            else
            {
                Debug.Log("不在nav网格内");
            }         
        }
        else
        {
            //与目标点的距离小于某个值后，等待一段时间，然后换其他目标点移动并重置等待时间
            GetComponent<Animator>().SetBool("DevilHeadMove", false);
            waitTime += Time.deltaTime*1f;
            if (waitTime>=3)
            {
                do
                {
                    tmpIndex = targetIndex;
                    targetIndex = Random.Range(0, enemyMoveTarget.GetLength());
                    //Debug.Log("tmpIndex" + tmpIndex);
                } while (tmpIndex == targetIndex);
                waitTime = 0;
            }
            
        }       
    }
}
