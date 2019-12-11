/*
 *  Troop控制脚本
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Troop状态
enum TroopState
{
    Normal,     //普通状态，该状态下什么都不干
    Spawn,      //生成状态，该状态下实现生成Troop时的一些特效操作            
    Idle,       //闲置状态，该状态为Troop的一般状态
    Rotate,     //旋转状态，该状态在发现敌人后，跑向敌人前旋转至面向敌人
    Run,        //奔跑状态，该状态下奔跑向敌人  
    Attack,     //攻击状态，该状态下每隔一段时间攻击敌人
    GetHit,     //被击状态，该状态下要检查受伤是否导致死亡
    Victory,    //胜利状态，该状态为击败敌人展示胜利动画
    Death       //死亡状态，该状态下进行对象销毁的相关操作
}

public class TroopsController : MonoBehaviour
{
    public float runSpeed;              //对象奔跑速度
    public float rotateSpeed;           //对象旋转速度
    public float attackRange;           //对象攻击范围
    public float runAngleRange;         //对象旋转至可以奔跑的角度
    public float timeBetweenAttack;     //对象攻击的时间间隔

    public CapsuleCollider swordCollider; //剑的碰撞脚本

    private int devilHeadIndex;         //攻击对象的索引

    private float timer;                //距离上一次攻击时间间隔的时间

    private Transform devilHeadList;    //攻击对象列表
    private Transform targetDevilHead;  //攻击对象

    private Animator animator;          //对象的状态机
    
    private Vector3 scale;              //对象的Scale属性，用于生成对象时的变化效果

    TroopState troopState;              //对象的状态    

    // Start is called before the first frame update
    void Start()
    {
        //设置对象的初始状态为Normal
        troopState = TroopState.Normal;
        //获取攻击对象列表
        devilHeadList = GameObject.Find("DevilHeadList").transform;
        //获取对象状态机
        animator = GetComponent<Animator>();
        //初始化timer为攻击事件间隔，使得对象在碰到敌人时马上发起第一次攻击
        timer = timeBetweenAttack;
        //获取对象的Scale属性
        scale = transform.localScale;
        //把对象的Scale属性设置为0
        transform.localScale = new Vector3(0, 0, 0);
        //在0.5s后生成对象
        Invoke("Spawn", 0.5f);
        //初始关闭剑的碰撞脚本
        swordCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //对象状态之间的切换
        switch(troopState)
        {
            case TroopState.Spawn:
                Scale();
                break;
            case TroopState.Idle:
                Idle();
                break;
            case TroopState.Run:
                Run();
                break;
            case TroopState.Rotate:
                Rotate();
                break;
            case TroopState.Attack:
                Attack();
                break;
            case TroopState.Victory:
                
                break;
            case TroopState.GetHit:
                GetHit();
                break;
            case TroopState.Death:
                Death();
                break;
        }

        //测试用
        //if(Input.GetKey(KeyCode.E))
        //{
        //    GetComponent<TroopsHealth>().TakeDamage(110);

        //    troopState = TroopState.GetHit;
            
        //}
    }

    //生成对象接口
    private void Spawn()
    {
        troopState = TroopState.Spawn;
        //在生成对象后，等待2s让对象完成变换效果，再将状态转换成Idle
        Invoke("SpawnToIdle", 2f);
    }

    //生成时等待状态转换成Idle的接口
    private void SpawnToIdle()
    {
        troopState = TroopState.Idle;
        CancelInvoke();
    }

    //生成对象时的Scale变换接口
    public void Scale()
    {
        transform.localScale = Vector3.Lerp(
            transform.localScale,
            scale,
            2f * Time.deltaTime
            );
    }

    //闲置状态接口
    private void Idle()
    {
        //如果攻击对象列表不为空且攻击目标为空，则从攻击对象列表中随机一个
        if(devilHeadList.childCount>0&&targetDevilHead==null)
        {
            devilHeadIndex = Random.Range(0, devilHeadList.childCount);
            targetDevilHead = devilHeadList.GetChild(devilHeadIndex);
            //获得攻击对象后，将状态改为旋转状态
            troopState = TroopState.Rotate;
        }
        else
        {
            troopState = TroopState.Attack;
        }
    }

    //旋转状态接口
    private void Rotate()
    {        
        if (targetDevilHead != null)
        {
            animator.SetBool("IsWalk", true);

            Vector3 tmpVc3 = targetDevilHead.position - transform.position;
            tmpVc3.y = transform.position.y;
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                Quaternion.LookRotation(tmpVc3),
                rotateSpeed * Time.deltaTime
                );

            //Debug.Log("runAngleRange:" + Vector3.Angle(transform.forward, tmpVc3));
            if (Vector3.Angle(transform.forward, tmpVc3) <= runAngleRange)
            {
                animator.SetBool("IsWalk", false);
                troopState = TroopState.Run;
            }
        }
        else
        {
            targetDevilHead = null;
            animator.SetBool("IsWalk", false);
            troopState = TroopState.Run;
        }         
    }

    //奔跑状态接口
    private void Run()
    {
        if(targetDevilHead!=null)
        {
            animator.SetBool("IsRun", true);

            Vector3 tmpVc3 = targetDevilHead.position - transform.position;
            tmpVc3.y = transform.position.y;
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                Quaternion.LookRotation(tmpVc3),
                rotateSpeed * Time.deltaTime
                );

            tmpVc3 = targetDevilHead.position;
            tmpVc3.y = transform.position.y;
            transform.position = Vector3.Lerp(
                transform.position,
                tmpVc3,
                runSpeed * Time.deltaTime
                );

            //Debug.Log("attackRange:" + (targetDevilHead.position - transform.position).sqrMagnitude);
            if ((tmpVc3 - transform.position).sqrMagnitude <= attackRange)
            {
                animator.SetBool("IsRun", false);
                troopState = TroopState.Attack;
            }
        }
        else
        {
            animator.SetBool("IsRun", false);
            targetDevilHead = null;
            troopState = TroopState.Idle;
        }
    }

    //攻击状态接口
    private void Attack()
    {
        if(targetDevilHead!=null)
        {
            if(targetDevilHead.GetComponent<EnemyHealth>().currentHealth<=0)
            {
                troopState = TroopState.Idle;
            }

            if ((targetDevilHead.position - transform.position).sqrMagnitude > attackRange)
            {
                animator.SetBool("IsRun", true);
                troopState = TroopState.Run;
            }

            timer += Time.deltaTime;

            if (timer >= timeBetweenAttack)
            {
                animator.SetTrigger("Attack");
                timer = 0;
            }

            //if (targetDevilHead.GetComponent<EnemyHealth>().currentHealth <= 0)
            //{
            //    //Destroy(targetDevilHead.gameObject);//临时顶替，要是Enemy死亡销毁无问题，则删除
            //    animator.SetTrigger("Victory");            
            //}
            
        }
        else
        {
            targetDevilHead = null;
            troopState = TroopState.Idle;
        }
    }

    //胜利状态接口
    private void Victory()
    {

    }

    //被击状态接口
    private void GetHit()
    {
        animator.SetBool("IsRun", false);
        animator.SetBool("IsWalk", false);
        animator.SetBool("IsGetHit",true);

        if(GetComponent<TroopsHealth>().currentHealth<=0)
        {
            //animator.SetBool("IsDeath", true);
            troopState = TroopState.Death;
        }
    }

    //死亡状态接口
    private void Death()
    {

    }

    //设置对象状态接口
    public void SetStateToGetHit()
    {
        troopState = TroopState.GetHit;
    }

    /*
     *  动画调用接口 
     */

    //造成伤害的公有接口
    public void AnimTakeDamage()
    {
        if(targetDevilHead!=null)
        {
            targetDevilHead.GetComponent<EnemyHealth>().TakeDamage(20);
        }    
    }

    //激活剑的碰撞脚本
    public void AnimOpenSwordScript()
    {
        swordCollider.enabled = true;
    }

    public void AnimJudgeDeathOrIdle()
    { 
        if (GetComponent<TroopsHealth>().currentHealth>0)
        {
            timer = timeBetweenAttack;
            animator.SetBool("IsGetHit", false);
            troopState = TroopState.Idle;         
        }
        else
        {
            animator.SetBool("IsDeath", true);
        }
    }
}
