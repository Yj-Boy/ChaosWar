using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTroop : MonoBehaviour
{
    public Transform[] spawnTrans;
    public GameObject troop;
    public Transform parent;

    private void Start()
    {
        //参数检查
        if(spawnTrans.Length==0)
        {
            Debug.LogWarning(GetType() + "SpawnTroop/Start()/spawnTrans没有设置！");
        }
        if(troop==null)
        {
            Debug.LogWarning(GetType() + "SpawnTroop/Start()/troop没有设置！");
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            SpawnTroopGO();
        }
    }

    public void SpawnTroopGO()
    {
        for(int i=0;i<spawnTrans.Length;i++)
        {
            GameObject GO = Instantiate(troop, spawnTrans[i])as GameObject;
            GO.transform.SetParent(parent);
        }
    }
}
