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

    private int gold;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        gold = 100;
        UIManager.Instance.InitGoldSlider(gold);
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
            UIManager.Instance.ShowTipText("金币不足！");
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
