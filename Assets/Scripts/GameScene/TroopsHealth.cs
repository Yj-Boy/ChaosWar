using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TroopsHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;

    private bool isDamage;
    private bool isDead;

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

    public void TakeDamage(int amount)
    {
        Debug.Log("TakeDamage:" + currentHealth);
        isDamage = true;
        currentHealth -= amount;
        if(currentHealth<=0&&!isDead)
        {
            Death();
        }
    }

    private void Death()
    {
        isDead = true;
        //test
        //Destroy(gameObject);
        //死亡效果
        GetComponent<Animation>().Play();
        transform.SetParent(null);
    }

    public void StartSinking()
    {
        Destroy(gameObject, 2f);
    }
}
