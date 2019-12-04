/*
 *  enemy控制脚本 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//enemy状态
enum EnemyState
{
    normal,             //常规
    move,               //移动
    attack,             //攻击
    shoot,              //射击
    death               //死亡
};

public class EnemyController : MonoBehaviour
{
    EnemyState enemyState;      //enemy状态对象

    NavMeshAgent nav;

    // Start is called before the first frame update
    void Start()
    {
        //初始化enemy的状态为move
        enemyState = EnemyState.move;
        nav = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        //若enemy的血量小于0，将enemy的状态切换为death
        if(GetComponent<EnemyHealth>().currentHealth<=0)
        {
            enemyState = EnemyState.death;
        }
        else
        {
            //若场上同时有攻击目标也有射击目标，优先进行攻击
            if (GetComponent<EnemyAttack>().GetTroopsNum() != 0
            && GetComponent<EnemyShoot>().GetTroopShooterNum() != 0)
            {
                enemyState = EnemyState.attack;
            }
            //若只有攻击目标，没有射击目标，转为射击状态
            else if (GetComponent<EnemyAttack>().GetTroopsNum() != 0
                && GetComponent<EnemyShoot>().GetTroopShooterNum() == 0)
            {
                enemyState = EnemyState.attack;
            }
            //若只有射击目标，没有攻击目标，则转为攻击状态
            else if (GetComponent<EnemyAttack>().GetTroopsNum() == 0
                && GetComponent<EnemyShoot>().GetTroopShooterNum() != 0)
            {
                enemyState = EnemyState.shoot;
            }
            //若没有攻击目标，也没有射击目标，则转为移动状态
            else if (GetComponent<EnemyAttack>().GetTroopsNum() == 0
                && GetComponent<EnemyShoot>().GetTroopShooterNum() == 0)
            {
                enemyState = EnemyState.move;
            }
        }
        

        switch (enemyState)
        {
            //移动状态下，调用移动脚本的move方法
            case EnemyState.move:
                //nav.enabled = true;
                GetComponent<EnemyMovement>().Move();
                break;
            //攻击状态下，调用攻击脚本的attack方法
            case EnemyState.attack:
                //nav.enabled = true;
                GetComponent<EnemyAttack>().Attack();
                break;
            //射击状态下，调用射击脚本的shoot方法
            case EnemyState.shoot:
                //nav.enabled = false;
                //nav.enabled = true;
                GetComponent<EnemyShoot>().Shoot();
                break;
            //死亡状态下，什么都不做
            case EnemyState.death:
                break;
        }

        //测试用
        if(Input.GetKeyDown(KeyCode.D))
        {
            GetState();
        }
    }

    
    //获得enemy当前状态
    public string GetState()
    {
        Debug.Log("GetState:" + enemyState.ToString());
        return enemyState.ToString();
    }

    //设置enemy状态
    public void SetState(string state)
    {
        switch(state)
        {
            case "normal":
                enemyState = EnemyState.normal;
                break;
            case "move":
                enemyState = EnemyState.move;
                break;
            case "attack":
                enemyState = EnemyState.attack;
                break;
            case "shoot":
                enemyState = EnemyState.shoot;
                break;
        }
    }
}
