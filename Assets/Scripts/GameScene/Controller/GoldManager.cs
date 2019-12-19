using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldManager : MonoBehaviour
{
    private static GoldManager instance;
    private static readonly object threadSafeLock = new object();

    public static GoldManager Instance
    {
        get
        {
            if(instance==null)
            {
                lock(threadSafeLock)
                {
                    if(instance==null)
                    {
                        instance = new GoldManager();
                    }
                }
            }
            return instance;
        }
    }

    private int initGold;
    private int gold;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        initGold = 200;
        gold = 80;
        UIManager.Instance.InitGoldSlider(initGold);
        UIManager.Instance.UpdateGoldSliderValue(gold);
    }

    public void AddGold(int amount)
    {
        gold += amount;
        UIManager.Instance.UpdateGoldSliderValue(gold);
    }

    public bool SubGold(int amount)
    {
        if(gold - amount<0)
        {
            UIManager.Instance.ShowTipText("魔力值不足！");
            return false;
        }
        else
        {
            gold -= amount;
            UIManager.Instance.UpdateGoldSliderValue(gold);
            return true;
        }       
    }
}
