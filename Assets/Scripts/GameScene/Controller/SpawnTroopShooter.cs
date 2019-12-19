using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnTroopShooter : MonoBehaviour
{
    public Transform[] spawnTrans;
    public GameObject shooter;
    public Transform parent;
    public int needGold;
    public GoldManager goldManager;
    //public Text tipText;

    private GameObject[] GO;

    private void Start()
    {
        //参数检查
        CheckValue();

        GO = new GameObject[spawnTrans.Length];
        for(int i=0;i<GO.Length;i++)
        {
            GO[i] = null;
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.W)
            &&UIManager.Instance.GetSkillButtonEnable(1))
        {
            if (goldManager.SubGold(needGold))
            {
                //Debug.Log(UIManager.Instance.GetSkillButtonEnable(1));
                UIManager.Instance.ResetSkillCountDownTime(1);
                SpawnShooter();
            }               
        }
    }

    public void SpawnShooter()
    {
        bool isFull = true;
        for (int i = 0; i < spawnTrans.Length; i++)
        {
            if (GO[i] == null)
            {
                isFull = false;
            }
        }
        if(isFull)
        {
            UIManager.Instance.ShowTipText("射手已满");
            return;
        }
        //for (int i = 0; i < spawnTrans.Length; i++)
        //{        
        //    //Debug.Log("GO:" + GO[i]);
        //    if (GO[i] == null)
        //    {           
        //        if(goldManager.SubGold(needGold))
        //        {
        //            GO[i] = Instantiate(shooter, spawnTrans[i]) as GameObject;
        //            GO[i].transform.SetParent(parent);
        //            break;
        //        }              
        //    }
        //}
        int x = Random.Range(0, spawnTrans.Length);
        if (GO[x] == null)
        {
            //if (goldManager.SubGold(needGold))
            //{
                GO[x] = Instantiate(shooter, spawnTrans[x]) as GameObject;
                GO[x].transform.SetParent(parent);
            //}
        }
        else
        {
            x = Random.Range(0, spawnTrans.Length);
        }
    }

    //参数检查接口
    private void CheckValue()
    {
        if (spawnTrans.Length == 0)
        {
            Debug.LogWarning(GetType() + "SpawnTroopShooter/Start()/spawnTrans没有设置！");
        }
        if (shooter == null)
        {
            Debug.LogWarning(GetType() + "SpawnTroopShooter/Start()/shooter没有设置！");
        }
        if (parent == null)
        {
            Debug.LogWarning(GetType() + "SpawnTroopShooter/Start()/parent没有设置！");
        }
        if (needGold == 0)
        {
            Debug.LogWarning(GetType() + "SpawnTroopShooter/Start()/needGold为0！请设置！");
        }
        if (goldManager == null)
        {
            Debug.LogWarning(GetType() + "SpawnTroopShooter/Start()/goldManager为0！请设置！");
        }
    }
}
