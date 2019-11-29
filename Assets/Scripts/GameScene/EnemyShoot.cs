using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyShoot : MonoBehaviour
{
    public float timeBetweenAttack = 0.5f;

    private Vector3 pos;
    private Transform troopShooter;
    private Transform targetTroopShooter;
    private int troopShooterNum;
    private int index;
    private float timer;

    //NavMeshAgent nav;
    // Start is called before the first frame update
    void Start()
    {
        //nav = GetComponent<NavMeshAgent>();
        troopShooter = GameObject.Find("TroopShooter").transform;
    }

    // Update is called once per frame
    void Update()
    {
        troopShooterNum = troopShooter.childCount;
        //if (troopShooterNum == 0)
        //{
        //    GetComponent<EnemyController>().SetState(EnemyState.move.ToString());
        //    return;
        //}
        //else
        //{
        //    GetComponent<EnemyController>().SetState(EnemyState.shoot.ToString());
        //}
    }

    public int GetTroopShooterNum()
    {
        return troopShooterNum;
    }

    public void Shoot()
    {
        //如果攻击对象生命值大于0
        //nav.SetDestination(pos);

        timer += Time.deltaTime;
        if(timer>=timeBetweenAttack)
        {
            timer = 0f;
            if (troopShooterNum > 0)
            {
                if (targetTroopShooter == null)
                {
                    //int tmpIndex = index;
                    //do
                    //{
                    //    index = Random.Range(0, troopShooterNum);
                    //} while (tmpIndex == index);
                    index = Random.Range(0, troopShooterNum);
                    targetTroopShooter = troopShooter.GetChild(index);
                }
                else if (targetTroopShooter != null)
                {
                    //transform.LookAt(targetTroopShooter);
                    Quaternion targetRotation = Quaternion.LookRotation(targetTroopShooter.position - transform.position, Vector3.up);
                    transform.rotation = Quaternion.Slerp(
                        transform.rotation,
                        targetRotation,
                        //targetTroopShooter.rotation,
                        2.5f * Time.deltaTime);

                    //transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, 
                    //    targetTroopShooter.position,
                    //    Time.deltaTime * 2);
                    targetTroopShooter.GetComponent<TroopsHealth>().TakeDamage(20);

                    if (targetTroopShooter.GetComponent<TroopsHealth>().currentHealth <= 0)
                    {
                        //GetComponent<EnemyController>().SetState(EnemyState.move.ToString());
                        targetTroopShooter = null;
                    }
                }
            }
        }
        
    }
}
