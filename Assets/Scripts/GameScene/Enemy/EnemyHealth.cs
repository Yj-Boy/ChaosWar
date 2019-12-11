/*
 *  enemy血量脚本 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    public int startingHealth;          //enemy血量
    public int currentHealth;           //enemy血量中间值，用于运算
    public ParticleSystem deathParticle;//死亡特效
    public int price;

    private GoldManager goldManager;

    bool isDead;                        //是否死亡
    bool isDark;                        //是否下沉

    private void Awake()
    {
        currentHealth = startingHealth;
        deathParticle.Stop();
        goldManager = GameObject.Find("GoldManager").GetComponent<GoldManager>();
    }

    private void Update()
    {
        //测试用，以后删除
        //if(Input.GetKeyDown(KeyCode.Q))
        //{
        //    TakeDamage(40);
        //}
        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    Destroy(gameObject);
        //}
    }

    //受伤害公共接口
    public void TakeDamage(int amount)
    {
        if(isDead)
        {
            return;
        }

        currentHealth -= amount;
        
        if(currentHealth<=0)
        {
            //测试用，直接销毁
            //Destroy(gameObject);
            Death();
        }
    }

    //死亡接口
    private void Death()
    {
        isDead = true;
        GetComponent<SphereCollider>().isTrigger = true;
        GetComponent<Animator>().SetTrigger("DevilHeadDown");
        goldManager.AddGold(price);
    }

    //暗化接口
    public void StartDark()
    {
        GetComponent<NavMeshAgent>().enabled = false;
        isDark = true;
        Destroy(gameObject, 3f);
    }

    //销毁跟生成气雾接口
    public void DeathParticle()
    {
        deathParticle.Play();
    }
}
