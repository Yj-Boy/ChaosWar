/*
 *  enemy射击脚本 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    public float timeBetweenAttack;                 //射击的时间间隔
                   
    private Transform troopShooter;                 //射击目标的父对象
    private Transform targetTroopShooter;           //射击目标
    private int troopShooterNum;                    //射击目标数量
    private int index;                              //射击目标索引
    private float timer;                            //距离射击间隔的时间

    // Start is called before the first frame update
    void Start()
    {
        troopShooter = GameObject.Find("TroopShooter").transform;
    }

    // Update is called once per frame
    void Update()
    {
        troopShooterNum = troopShooter.childCount;
    }

    //获得射击目标数量公有接口
    public int GetTroopShooterNum()
    {
        return troopShooterNum;
    }

    //射击接口
    public void Shoot()
    {
        //如果射击目标数量大于0
        if (troopShooterNum > 0)
        {
            //如果当前射击目标为空，随机从射击目标对象中赋予一个射击目标
            if (targetTroopShooter == null)
            {   
                index = Random.Range(0, troopShooterNum);
                targetTroopShooter = troopShooter.GetChild(index);
            }
            else if (targetTroopShooter != null)
            {
                //若当前射击目标不为空，将enemy旋转至朝向射击目标
                Quaternion targetRotation = Quaternion.LookRotation(
                    targetTroopShooter.position-transform.position);
                //Debug.Log("targetRotation:" + targetRotation);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    1.5f * Time.deltaTime);
                //Debug.Log("transform.rotation:" + transform.rotation);

                //累计timer，若大于射击间隔，进行一次射击并造成伤害
                timer += Time.deltaTime;
                if (timer >= timeBetweenAttack)
                {
                    timer = 0f;
                    targetTroopShooter.GetComponent<TroopsHealth>().TakeDamage(20);
                    GetComponent<Animator>().SetTrigger("DevilHeadShoot");

                    //如果射击目标的血量小于0，则将当前射击目标设置为空
                    if (targetTroopShooter.GetComponent<TroopsHealth>().currentHealth <= 0)
                    {
                        targetTroopShooter = null;
                    }
                }          
            }
        }


    }
}
