using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttack : MonoBehaviour
{
    private Vector3 pos;
    private Transform troops;
    private Transform targetTroop;
    private int troopsNum;
    private int index;

    NavMeshAgent nav;
    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        troops = GameObject.Find("Troops").transform;
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.X))
        //{
        //    Debug.Log(troopsNum);
        //}
        troopsNum = troops.childCount;
        //if (troopsNum==0)
        //{
        //    GetComponent<EnemyController>().SetState(EnemyState.move.ToString());
        //    return;
        //}
        //else
        //{
        //    GetComponent<EnemyController>().SetState(EnemyState.attack.ToString());
        //}   
    }

    public int GetTroopsNum()
    {
        return troopsNum;
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Troop"))
    //    {
    //        if(pos==new Vector3(0,0,0))
    //        {
    //            GetComponent<EnemyController>().SetState(EnemyState.attack.ToString());
    //            pos = other.transform.position;
    //        }          
    //    }
    //}

    //private void OnTriggerStay(Collider other)
    //{
    //    if ((transform.position - other.transform.position).sqrMagnitude < 14)
    //    {
    //        other.GetComponent<TroopsHealth>().TakeDamage(20);
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Troop"))
    //    {
    //        Debug.Log(GetType() + "OnTriggerExit");
    //        GetComponent<EnemyController>().SetState(EnemyState.move.ToString());
    //        ResetPos();
    //    }
    //}

    public void Attack()
    {
        //如果攻击对象生命值大于0
        //nav.SetDestination(pos);

        if (troopsNum > 0)
        {
            if(targetTroop == null)
            {
                //int tmpIndex = index;
                //do
                //{
                //    index = Random.Range(0, troopsNum);
                //} while (tmpIndex == index);
                index = Random.Range(0, troopsNum);
                targetTroop = troops.GetChild(index);
            }
            else if (targetTroop != null)
            {
                nav.SetDestination(targetTroop.position);
                if ((transform.position - targetTroop.position).sqrMagnitude < 14)
                {
                    targetTroop.GetComponent<TroopsHealth>().TakeDamage(20);
                    if (targetTroop.GetComponent<TroopsHealth>().currentHealth <= 0)
                    {
                        //GetComponent<EnemyController>().SetState(EnemyState.move.ToString());
                        targetTroop = null;
                    }
                }
            }
        }   
    }

    public void ResetPos()
    {
        pos = new Vector3(0,0,0);
    }
}
