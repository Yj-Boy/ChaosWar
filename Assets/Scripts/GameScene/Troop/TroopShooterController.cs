using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum State
{
    Normal,
    Idle,
    Shoot,
    GetHit,
    Death
}

public class TroopShooterController : MonoBehaviour
{
    public float rotateSpeed;
    public float timeBetweenAttack;

    private Transform shootTargetList;
    private Transform shootTarget;

    private int targetIndex;

    private float timer;

    private Animator animator;

    State state;
    
    // Start is called before the first frame update
    void Start()
    {
        state = State.Normal;

        animator = GetComponent<Animator>();

        shootTargetList = GameObject.Find("DevilHeadList").transform;

        timer = timeBetweenAttack;
    }

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            case State.Normal:
                Normal();
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
    }

    private void Normal()
    {
        state = State.Idle;
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
        if(timer>=timeBetweenAttack)
        {
            animator.SetTrigger("Attack");
            timer = 0;
        }    
    }

    private void GetHit()
    {

    }

    private void Death()
    {

    }
}
