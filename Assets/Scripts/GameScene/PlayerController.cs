/*
 *  主角的控制脚本
 *  
 *  用于控制主角移动，动画播放，大招释放
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;             //主句移动速度
    public Animator animator;           //主角状态机

    private Vector3 pos;                //主角移动位置
    private Vector3 rotate;             //主角旋转角度
    private float horizontal;           //水平移动变量
    private static bool canMove;        //判断主角是否可移动

    private void Start()
    {
        //检查public参数是否赋值
        if(moveSpeed==0)
        {
            Debug.Log(GetType() + "/Start/移动速度moveSpeed没有初始化或初值为0！");
        }
        if(animator==null)
        {
            Debug.Log(GetType() + "/Start/状态机animator没有指定！");
        }
        //初始化private参数
        pos = transform.parent.position;
        rotate=transform.parent.rotation.eulerAngles;
        canMove = true;
    }

    private void Update()
    {
        MoveHero();
    }

    //主角移动方法
    private void MoveHero()
    {
        //if (Input.GetKey(KeyCode.A))
        //{
        //    pos.x -= moveSpeed * Time.deltaTime;
        //    transform.position = pos;
        //}
        //else if (Input.GetKey(KeyCode.D))
        //{
        //    pos.x += moveSpeed * Time.deltaTime;
        //    transform.position = pos;
        //}

        //如果可移动
        if(canMove==true)
        {
            //计算水平方向的移动差值
            horizontal = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
            //如果小于0，向左移动，计算主角移动位置和旋转角度，播放Walk动画
            if (horizontal < 0)
            {
                pos.x += horizontal;
                rotate.z = Mathf.LerpAngle(0, -90, 2);
                animator.SetBool("Walk", true);
            }
            //如果大于0，向右移动，计算主角移动位置和旋转角度，播放Walk动画
            else if (horizontal > 0)
            {
                pos.x += horizontal;
                rotate.z = Mathf.LerpAngle(0, 90, 2);
                animator.SetBool("Walk", true);
            }
            //如果等于0，计算主角旋转角度，停止播放Walk动画
            else
            {
                rotate = new Vector3(-90, 0, 0);
                animator.SetBool("Walk", false);
            }
            //将计算好的位置（pos）和角度（rotate）重新赋值给主角
            transform.parent.position = pos;
            transform.parent.rotation = Quaternion.Euler(rotate);
        }      
    }

    //Button触发主角释放大招动画
    public void ButtonLaunchThousandsOfTroops()
    {
        //设置主角朝向，使其在移动过程中触发大招也能面向敌人
        GetComponent<Transform>().parent.rotation = Quaternion.Euler(new Vector3(-90,0,0));
        //设置Attack触发器，播放主角释放大招动画
        animator.SetTrigger("Attack");
        //主角释放大招的时候不可移动
        canMove = false;
        Debug.Log("ButtonLaunchThousandsOfTroops");
    }
    //动画中途释放主角大招
    public void AnimLaunchThousandsOfTroops()
    {
        //通过ECS生成大招所需的实体
        GameObject.Find("_ECSscript").GetComponent<CreatThousandsOfTroops>().SpawnEntity();
        //GameObject.Find("_ECSscript").GetComponent<CreatThousandsOfTroops>().SpawnEntityByHyBrid();
        Debug.Log("AnimLaunchThousandsOfTroops");
    }
    //大招动画最后回调，设置主角为可移动
    public void SetCanMoveTrue()
    {
        canMove = true;
    }
}
