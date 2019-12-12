using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleHealth : MonoBehaviour
{
    //单例模式
    private static CastleHealth instance;
    private static readonly object threadSafeLock = new object();

    public static CastleHealth Instance
    {
        get
        {
            if(instance==null)
            {
                lock(threadSafeLock)
                {
                    if (instance == null)
                    {
                        instance = new CastleHealth();
                    }
                }
            }      
            return instance;
        }
    }

    //参数
    public int startingHealth;

    private int currentHealth;

    //方法
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        currentHealth = startingHealth;
        UIManager.Instance.InitCastleHpSlider(currentHealth);
    }

    //加血接口
    public void AddHealth(int amount)
    {
        currentHealth += amount;
    }

    //扣血接口
    public void SubHealth(int amount)
    {
        if(currentHealth-amount<=0)
        {
            currentHealth = 0;
        }
        else
        {
            currentHealth -= amount;
        }       
        UIManager.Instance.UpdateHpSliderValue(currentHealth);
    }

    //判断是否死亡接口
    public bool IsDead()
    {
        if(currentHealth<=0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //获得HP接口
    public int GetHealth()
    {
        return currentHealth;
    }
}
