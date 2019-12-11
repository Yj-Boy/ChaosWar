/*
 *  士兵血量脚本
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TroopsHealth : MonoBehaviour
{
    public int startingHealth = 100;        //士兵血量
    public int currentHealth;               //士兵血量中间对象，用于运算

    private bool isDamage;                  //是否受伤
    private bool isDead;                    //是否死亡

    private void Awake()
    {
        currentHealth = startingHealth;
    }

    private void Update()
    {
        //如果受伤了（isDamage==true）
        //展示受伤效果
        //否则恢复原样
        isDamage = false;
    }

    //受伤害的公告接口
    public void TakeDamage(int amount)
    {     
        isDamage = true;
        currentHealth -= amount;
        //Debug.Log("TakeDamage:" + currentHealth);
        if (currentHealth<=0&&!isDead)
        {
            Death();
        }
    }

    //死亡接口
    private void Death()
    {
        isDead = true;
        //test
        //Destroy(gameObject);
        //死亡效果
        //GetComponent<Animation>().Play();
        transform.SetParent(null);
        Destroy(gameObject, 5f);
    }

    //销毁接口
    public void StartSinking()
    {
        Destroy(gameObject, 2f);
    }
}
