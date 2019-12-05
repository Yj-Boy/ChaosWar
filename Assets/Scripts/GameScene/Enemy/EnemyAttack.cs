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

    NavMeshAgent nav;                       //NavMeshAgent对象
    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        troops = GameObject.Find("Troops").transform;
    }

    private void Update()
    {     
        troopsNum = troops.childCount; 
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
        
        //如果攻击目标的数量大于0
        if (troopsNum > 0)
        {
            //如果当前攻击目标为空，则随机从攻击对象中赋予一个给当前攻击目标
            if(targetTroop == null)
            {
                index = Random.Range(0, troopsNum);
                targetTroop = troops.GetChild(index);
            }
            else if (targetTroop != null)
            {
                //若不为空，则攻击当前对象
                GetComponent<Animator>().SetBool("DevilHeadMove", true);
                
                //若与攻击目标的距离小于某个值，则攻击，否则移动
                if ((transform.position - targetTroop.position).sqrMagnitude < 14)
                {
                    //nav.updatePosition = false;
                    //nav.updateRotation = false;

                    //累计时间，若大于攻击时间间隔，则攻击
                    timer += Time.deltaTime;
                    if (timer >= timeBetweenAttack&& targetTroop.GetComponent<TroopsHealth>().currentHealth >0)
                    {
                        GetComponent<Animator>().SetTrigger("DevilHeadAttack");                    
                    }
                    
                    //若攻击对象的血量小于0，则将当前攻击对象设置为空
                    if (targetTroop.GetComponent<TroopsHealth>().currentHealth <= 0)
                    {
                        //GetComponent<EnemyController>().SetState(EnemyState.move.ToString());
                        targetTroop = null;
                    }
                }
                else
                {
                    nav.SetDestination(targetTroop.position);
                    //nav.updatePosition = true;
                    //nav.updateRotation = true;
                    //nav.enabled = false;
                    //transform.rotation = Quaternion.Slerp(
                    //    transform.rotation,
                    //    Quaternion.LookRotation(targetTroop.position - transform.position),
                    //    Time.deltaTime);
                    //transform.position = Vector3.Lerp(
                    //    transform.position,
                    //    targetTroop.position,
                    //    Time.deltaTime
                    //    );
                }
            }
        }   
    }

    //动画中触发真实伤害接口
    public void AttackTakeDamage()
    {
        targetTroop.GetComponent<TroopsHealth>().TakeDamage(20);
        attackParticles.Play();
        timer = 0f;
    }
}
