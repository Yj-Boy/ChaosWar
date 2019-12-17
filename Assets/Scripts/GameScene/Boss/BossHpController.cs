using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHpController : MonoBehaviour
{
    //单例模式
    private static BossHpController instance = null;
    private static readonly object threadSafeLock = new object();

    public static BossHpController Instance
    {
        get
        {
            if (instance == null)
            {
                lock (threadSafeLock)
                {
                    if (instance == null)
                    {
                        instance = new BossHpController();
                    }
                }
            }
            return instance;
        }
    }

    public int startingHealth;

    private int currentHealth;
    private bool isDead;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        isDead = false;
        currentHealth = startingHealth;
        UIManager.Instance.InitBossSlider(currentHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if(isDead)
        {
            Death();
        }
    }

    public void AddHp(int amount)
    {
        currentHealth += amount;
        UIManager.Instance.UpdateBossSliderValue(currentHealth);
    }

    public void SubHp(int amount)
    {
        if(currentHealth-amount<0)
        {
            currentHealth = 0;
            isDead = true;
        }
        else
        {
            currentHealth -= amount;
        }
        UIManager.Instance.UpdateBossSliderValue(currentHealth);
    }

    private void Death()
    {
        //死亡的相关操作
        //UIManager.Instance.ShowWinOrLoseText(true);
        UIManager.Instance.ShowGameOver(true);
    }
}
