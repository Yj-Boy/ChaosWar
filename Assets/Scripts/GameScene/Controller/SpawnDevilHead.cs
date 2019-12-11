using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDevilHead : MonoBehaviour
{
    public Transform[] spawnTrans;              //生成位置列表
    public Transform parentTrans;               //生成对象要设置的父对象
    public GameObject goPrefab;                 //实例化的预制体
    public GameObject magicCircle;              //生成的魔法阵特效
    public float heartBeatTime;

    private GameObject devilHeadGO;             //生成对象
    private Transform magicCirleTrans;          //魔法阵的位置
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        //参数检查
        if(spawnTrans==null)
        {
            Debug.Log(GetType() + "SpawnDevilHead/Start()/spwnTrans没有设置！");
        }

        if (goPrefab == null)
        {
            Debug.Log(GetType() + "SpawnDevilHead/Start()/goPrefab没有设置！");
        }

        if (magicCircle == null)
        {
            Debug.Log(GetType() + "SpawnDevilHead/Start()/magicCircle没有设置！");
        }

        if(heartBeatTime==0)
        {
            Debug.Log(GetType() + "SpawnDevilHead/Start()/heartBeatTime为0！请设置！");
        }

        //参数初始化
        timer = heartBeatTime;
    }

    // Update is called once per frame
    void Update()
    {
        //心跳检测
        HeartBeat();
    }

    public void HeartBeat()
    {
        timer += Time.deltaTime;
        if(timer>=heartBeatTime)
        {
            if(parentTrans.childCount<=0)
            {
                SpawnDevilHeadGO();
            }
            timer = 0;
        }
    }

    private void SpawnDevilHeadGO()
    {
        for (int i = 0; i < spawnTrans.Length; i++)
        {
            //实例化对象
            //Debug.Log("spawnTrans:" + spawnTrans[i].position.y);
            devilHeadGO = Instantiate(goPrefab, spawnTrans[i]);
            devilHeadGO.transform.parent = parentTrans;

            //实例化魔法阵
            magicCirleTrans = spawnTrans[i];
            Vector3 beforVec3= spawnTrans[i].position;
            Vector3 tmpVec3 = magicCirleTrans.position;
            tmpVec3.y = 0.2f;
            magicCirleTrans.position = tmpVec3;
            Instantiate(magicCircle, magicCirleTrans);
            spawnTrans[i].position = beforVec3;
        }          
    }
}
