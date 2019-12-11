using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldManager : MonoBehaviour
{
    public Text goldText;

    private int gold;

    private void Start()
    {
        gold = 100;
        goldText.text = gold.ToString();
    }

    public void AddGold(int amount)
    {
        gold += amount;
        goldText.text = gold.ToString();
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
            goldText.text = gold.ToString();
            return true;
        }       
    }
}
