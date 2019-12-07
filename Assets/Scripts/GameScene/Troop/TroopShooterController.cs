using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum State
{
    Normal,
    Spawn,
    Idle,
    Shoot,
    GetHit,
    Death
}

public class TroopShooterController : MonoBehaviour
{
    public float rotateSpeed;
    public float timeBetweenAttack;

    public ParticleSystem shootParticle;
    public ParticleSystem spawnParticle;

    private Transform shootTargetList;
    private Transform shootTarget;

    private int targetIndex;

    private float timer;

    private Vector3 scale;

    private Animator animator;

    State state;
    
    // Start is called before the first frame update
    void Start()
    {
        state = State.Normal;

        animator = GetComponent<Animator>();
        

        shootTargetList = GameObject.Find("DevilHeadList").transform;

        timer = timeBetweenAttack-1f;

        scale = transform.localScale;
        transform.localScale = new Vector3(0, 0, 0);

        shootParticle.Stop();
        spawnParticle.Stop();
    }

    // Update is called once per frame
    void Update()
    {
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

    private void Normal()
    {
        state = State.Spawn;
    }

    private void Spawn()
    {
        animator.enabled = false;
       
        spawnParticle.Play();

        transform.localScale = Vector3.Lerp(
            transform.localScale,
            scale,
            2f * Time.deltaTime
            );

        Invoke("SpawnToIdle", 2f);
    }

    private void SpawnToIdle()
    {
        state = State.Idle;
        animator.enabled = true;
        CancelInvoke();
    }

    private void Idle()
    {
        if(shootTargetList.childCount>0&&shootTarget==null)
        {
            targetIndex = Random.Range(0, shootTargetList.childCount);
            shootTarget = shootTargetList.GetChild(targetIndex);
            state = State.Shoot;        
        }
    }

    private void Shoot()
    {
        Vector3 tmpVc3 = shootTarget.position - transform.position;
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            Quaternion.LookRotation(tmpVc3),
            rotateSpeed * Time.deltaTime
            );

        timer += Time.deltaTime;
        if(timer>=timeBetweenAttack&&shootTarget!=null)
        {
            animator.SetTrigger("Attack");
            shootTarget.GetComponent<EnemyHealth>().TakeDamage(20);
            if(shootTarget.GetComponent<EnemyHealth>().currentHealth<=0)
            {              
                Destroy(shootTarget.gameObject);//测试用
                shootTarget = null;
                animator.SetTrigger("Victory");                      
                state = State.Idle;
            }
            timer = 0;
        }    
    }

    private void GetHit()
    {
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
}
