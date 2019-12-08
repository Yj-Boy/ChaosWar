/*
 *  射手士兵控制脚本 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum State
{
    Normal,     //初始状态，该状态下什么都不做，预留缓冲
    Spawn,      //生成状态，该状态下做一些生成时的变化
    Idle,       //闲置状态，该状态作为其他状态的中转
    Shoot,      //射击状态，该状态下对象旋转朝向攻击对象并射击
    GetHit,     //被击状态，该状态下触发被击效果并判断是否会死亡
    Death       //死亡状态，该状态完成销毁对象的相关操作
}

public class TroopShooterController : MonoBehaviour
{
    public float rotateSpeed;               //旋转速度
    public float miniAttackTime;
    public float maxAttackTime;
    private float timeBetweenAttack;         //射击时间间隔

    public ParticleSystem shootParticle;    //射击瞬间特效
    public ParticleSystem spawnParticle;    //生成特效

    public GameObject arrow;                //箭

    private Transform shootTargetList;      //射击目标队列
    private Transform shootTarget;          //射击目标

    private int targetIndex;        //射击目标索引

    private float timer;            //距离射击间隔的

    private Vector3 scale;          //对象的scale属性

    private Animator animator;      //对象的状态机

    State state;
    
    // Start is called before the first frame update
    void Start()
    {
        state = State.Normal;

        animator = GetComponent<Animator>();
        
        shootTargetList = GameObject.Find("DevilHeadList").transform;

        //timer = timeBetweenAttack-1f;
        timeBetweenAttack = Random.Range(miniAttackTime, maxAttackTime);

        scale = transform.localScale;
        transform.localScale = new Vector3(0, 0, 0);

        shootParticle.Stop();
        spawnParticle.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        //对象状态切换
        switch(state)
        {
            case State.Normal:
                Normal();
                break;
            case State.Spawn:
                Spawn();
                break;
            case State.Idle:
                Idle();
                break;
            case State.Shoot:
                Shoot();
                break;
            case State.GetHit:
                GetHit();
                break;
            case State.Death:
                Death();
                break;
        }

        //被击测试
        if(Input.GetKeyDown(KeyCode.F))
        {
            animator.SetTrigger("GetHit");
            //伤害应该由攻击方决定，这里只做测试
            GetComponent<TroopsHealth>().TakeDamage(30);

            state = State.GetHit;
        }
    }

    //初始状态接口
    private void Normal()
    {
        state = State.Spawn;
    }

    //生成状态接口
    private void Spawn()
    {
        animator.enabled = false;
        //播放生成特效
        spawnParticle.Play();
        //生成时的Scale变化，由0到1
        transform.localScale = Vector3.Lerp(
            transform.localScale,
            scale,
            2f * Time.deltaTime
            );

        Invoke("SpawnToIdle", 3f);
    }
    
    //生成完后将状态改为Idle接口，该接口延时调用
    private void SpawnToIdle()
    {
        spawnParticle.Stop();
        state = State.Idle;
        animator.enabled = true;     
        CancelInvoke();
    }

    //闲置状态接口
    private void Idle()
    {
        //射击目标队列不为空且射击目标为空
        //从射击队列里随机选一个当射击目标
        if(shootTargetList.childCount>0&&shootTarget==null)
        {
            targetIndex = Random.Range(0, shootTargetList.childCount);
            shootTarget = shootTargetList.GetChild(targetIndex);
            state = State.Shoot;        
        }
    }

    //射击状态接口
    private void Shoot()
    {
        //旋转对象朝向射击目标
        Vector3 tmpVc3 = shootTarget.position - transform.position;
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            Quaternion.LookRotation(tmpVc3),
            rotateSpeed * Time.deltaTime
            );

        //到达攻击时间间隔且攻击对象不为空，则射击
        timer += Time.deltaTime;
        if(timer>=timeBetweenAttack&&shootTarget!=null)
        {
            animator.SetTrigger("Attack");
            //shootTarget.GetComponent<EnemyHealth>().TakeDamage(20);

            timeBetweenAttack = Random.Range(miniAttackTime, maxAttackTime);
            timer = 0;
        }    
    }

    public void ChangeShootToIdle()
    {
        if (shootTarget.GetComponent<EnemyHealth>().currentHealth <= 0)
        {
            Destroy(shootTarget.gameObject);//测试用
            shootTarget = null;
            animator.SetTrigger("Victory");
            state = State.Idle;
        }
    }

    //被击接口
    private void GetHit()
    {
        //被击若hp小于0，则死，否则转为闲置状态
        if(GetComponent<TroopsHealth>().currentHealth<=0)
        {
            Debug.Log("Dead");
            animator.SetBool("IsDeath", true);
            state = State.Death;
        }
        else
        {
            state = State.Idle;
        }
    }

    //死亡接口
    private void Death()
    {
        
    }

    /*
     *  动画调用接口 
     */

    //切换状态为Idle
    public void AnimToIdle()
    {
        state = State.Idle;
    }

    //销毁对象
    public void AnimToDestroy()
    {
        Destroy(gameObject);
    }

    //射击特效
    public void AnimToParticle()
    {
        shootParticle.Play();
    }

    //生成箭
    public void AnimToSpawnArrow()
    {
        Transform trans;

        trans=transform;
   
        trans.LookAt(shootTarget);

        GameObject arrowGO = Instantiate(arrow, trans)as GameObject;

        arrowGO.transform.position = shootParticle.transform.position;
        arrowGO.transform.localScale = shootParticle.transform.localScale;
    }
}
