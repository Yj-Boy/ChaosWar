/*
 *  enemy进攻脚本
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttack : MonoBehaviour
{
    public float timeBetweenAttack;         //进攻时间间隔
    public ParticleSystem attackParticles;  //攻击粒子特效

    private Transform troops;               //攻击目标的父对象
    private Transform targetTroop;          //当前攻击目标
    private int troopsNum;                  //攻击目标的总数量
    private int index;                      //当前攻击目标的索引
    private float timer;                    //累计到攻击时间间隔的变量
    private bool isHasTarget;

    NavMeshAgent nav;                       //NavMeshAgent对象
    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        troops = GameObject.Find("Troops").transform;
        isHasTarget = false;
    }

    private void Update()
    {     
        troopsNum = troops.childCount; 
    }

    public int GetTroopsNum()
    {
        return troopsNum;
    }

    public void Attack()
    {
        //如果攻击目标的数量大于0
        //如果当前攻击目标为空，则随机从攻击对象中赋予一个给当前攻击目标
        if (!isHasTarget && targetTroop == null && troopsNum > 0) 
        {
            index = Random.Range(0, troopsNum);
            targetTroop = troops.GetChild(index);
            isHasTarget = true;
        }
        else
        {
            if(targetTroop!=null)
            {
                //若不为空，则攻击当前对象
                GetComponent<Animator>().SetBool("DevilHeadMove", true);

                //若与攻击目标的距离小于某个值，则攻击，否则移动
                if ((transform.position - targetTroop.position).sqrMagnitude < 25)
                {
                    //累计时间，若大于攻击时间间隔，则攻击
                    timer += 1f*Time.deltaTime;
                    if (timer >= timeBetweenAttack && targetTroop.GetComponent<TroopsHealth>().currentHealth > 0)
                    {
                        GetComponent<Animator>().SetTrigger("DevilHeadAttack");
                        timer = 0;
                        Debug.Log("EnemyAttackTime"+ timer);
                        Debug.Log("timeBetweenAttack======" + timeBetweenAttack);
                    }

                    //若攻击对象的血量小于0，则将当前攻击对象设置为空
                    if (targetTroop.GetComponent<TroopsHealth>().currentHealth <= 0)
                    {
                        targetTroop = null;
                        isHasTarget = false;
                    }
                }
                else
                {
                    nav.SetDestination(targetTroop.position);
                }
            }
            else
            {
                targetTroop = null;
                isHasTarget = false;
            }
        }  
    }

    //动画中触发真实伤害接口
    public void AttackTakeDamage()
    {
        if(targetTroop!=null)
        {
            targetTroop.GetComponent<TroopsHealth>().TakeDamage(20);
            targetTroop.GetComponent<TroopsController>().SetStateToGetHit();
            attackParticles.Play();
        }
    }
}
