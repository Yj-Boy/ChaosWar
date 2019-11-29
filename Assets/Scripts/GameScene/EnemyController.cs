﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

enum EnemyState
{
    normal,
    move,
    attack,
    shoot
};

public class EnemyController : MonoBehaviour
{
    EnemyState enemyState;
    // Start is called before the first frame update
    void Start()
    {
        enemyState = EnemyState.move;
    }

    // Update is called once per frame
    void Update()
    {
        if(GetComponent<EnemyAttack>().GetTroopsNum()!=0
            &&GetComponent<EnemyShoot>().GetTroopShooterNum()!=0)
        {
            enemyState = EnemyState.attack;
        }
        else if(GetComponent<EnemyAttack>().GetTroopsNum() != 0
            && GetComponent<EnemyShoot>().GetTroopShooterNum() == 0)
        {
            enemyState = EnemyState.attack;
        }
        else if (GetComponent<EnemyAttack>().GetTroopsNum() == 0
            && GetComponent<EnemyShoot>().GetTroopShooterNum() != 0)
        {
            enemyState = EnemyState.shoot;
        }
        else if (GetComponent<EnemyAttack>().GetTroopsNum() == 0
            && GetComponent<EnemyShoot>().GetTroopShooterNum() == 0)
        {
            enemyState = EnemyState.move;
        }

        switch (enemyState)
        {
            case EnemyState.move:
                GetComponent<EnemyMovement>().Move();
                break;
            case EnemyState.attack:
                GetComponent<EnemyAttack>().Attack();
                break;
            case EnemyState.shoot:
                GetComponent<EnemyShoot>().Shoot();
                break;
        }

        if(Input.GetKeyDown(KeyCode.D))
        {
            GetState();
        }
    }

    

    public string GetState()
    {
        Debug.Log("GetState:" + enemyState.ToString());
        return enemyState.ToString();
    }

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
