using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTroop : MonoBehaviour
{
    public Transform[] spawnTrans;
    public GameObject troop;
    public Transform parent;
    public int needGold;
    public GoldManager goldManager;

    private void Start()
    {
        //参数检查
        CheckValue();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q)
            &&UIManager.Instance.GetSkillButtonEnable(0))
        {
            if (goldManager.SubGold(needGold))
            {
                UIManager.Instance.ResetSkillCountDownTime(0);
                SpawnTroopGO();
            }
        }
    }

    public void SpawnTroopGO()
    {
        //if(goldManager.SubGold(needGold))
        //{
            for (int i = 0; i < spawnTrans.Length; i++)
            {
                GameObject GO = Instantiate(troop, spawnTrans[i]) as GameObject;
                GO.transform.SetParent(parent);
            }
        //}
    }

    //参数检查接口
    private void CheckValue()
    {   
        if (spawnTrans.Length == 0)
        {
            Debug.LogWarning(GetType() + "SpawnTroop/Start()/spawnTrans没有设置！");
        }
        if (troop == null)
        {
            Debug.LogWarning(GetType() + "SpawnTroop/Start()/troop没有设置！");
        }
        if (parent == null)
        {
            Debug.LogWarning(GetType() + "SpawnTroop/Start()/parent没有设置！");
        }
        if (needGold == 0)
        {
            Debug.LogWarning(GetType() + "SpawnTroop/Start()/needGold为0，请设置！");
        }
        if (goldManager == null)
        {
            Debug.LogWarning(GetType() + "SpawnTroop/Start()/goldManager没有设置！");
        }
    }
}
