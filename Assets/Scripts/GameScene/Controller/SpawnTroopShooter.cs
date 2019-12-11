using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTroopShooter : MonoBehaviour
{
    public Transform[] spawnTrans;
    public GameObject shooter;
    public Transform parent;

    private GameObject[] GO;

    private void Start()
    {
        //参数检查
        if(spawnTrans.Length==0)
        {
            Debug.LogWarning(GetType() + "SpawnTroopShooter/Start()/spawnTrans没有设置！");
        }
        if (shooter==null)
        {
            Debug.LogWarning(GetType() + "SpawnTroopShooter/Start()/shooter没有设置！");
        }
        if (parent == null)
        {
            Debug.LogWarning(GetType() + "SpawnTroopShooter/Start()/parent没有设置！");
        }

        GO = new GameObject[spawnTrans.Length+1];
        for(int i=0;i<GO.Length;i++)
        {
            GO[i] = null;
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            SpawnShooter();
        }
    }

    public void SpawnShooter()
    {
        for(int i=0;i<spawnTrans.Length;i++)
        {
            //Debug.Log("GO:" + GO[i]);
            if(GO[i]==null)
            {
                GO[i] = Instantiate(shooter, spawnTrans[i]) as GameObject;
                GO[i].transform.SetParent(parent);
                break;
            }          
        }
    }
}
