/*
 *  enemy移动目标点接口 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveTarget : MonoBehaviour
{
    public Transform[] enemyMoveTarget;      //Enemy移动点

    private void Start()
    {
        //参数检查
        if(enemyMoveTarget.Length<=0)
        {
            Debug.Log(GetType() + "EnemyMoveTarget/Start()/enemyMoveTarget数组长度为0！");
        }
    }

    public int GetLength()
    {
        return enemyMoveTarget.Length;
    }

    //返回移动点的position
    public Vector3 GetPosition(int index)
    {
        return enemyMoveTarget[index].position;
    }
}
