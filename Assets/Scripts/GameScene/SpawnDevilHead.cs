using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDevilHead : MonoBehaviour
{
    public Transform spawnTrans;
    public GameObject goPrefab;
    public GameObject magicCircle;

    private GameObject devilHeadGO;
    private Transform targetTrans;
    private Transform magicCirleTrans;
    // Start is called before the first frame update
    void Start()
    {
        //参数检查
        if(spawnTrans==null)
        {
            Debug.Log(GetType() + "SpawnDevilHead/Start()/spwnTrans没有设置！");
        }
        //else
        //{
        //    targetTrans.position.Set(
        //        spawnTrans.position.x,
        //        spawnTrans.position.x + 2f,
        //        spawnTrans.position.z
        //        );
        //}

        if (goPrefab == null)
        {
            Debug.Log(GetType() + "SpawnDevilHead/Start()/goPrefab没有设置！");
        }

        if (magicCircle == null)
        {
            Debug.Log(GetType() + "SpawnDevilHead/Start()/magicCircle没有设置！");
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        //测试用
        if(Input.GetKeyDown(KeyCode.J))
        {
            SpawnDevilHeadGO();         
        }

        //if(devilHeadGO!=null)
        //{
        //    if(devilHeadGO.transform.position.y<=targetTrans.position.y)
        //    {
        //        devilHeadGO.transform.position = Vector3.Lerp(
        //            devilHeadGO.transform.position,
        //            targetTrans.position,
        //            0.5f * Time.deltaTime
        //            );
        //    }
        //    else
        //    {
        //        devilHeadGO.GetComponent<EnemyController>().enabled = true;
        //        devilHeadGO = null;
        //    }
        //}
    }

    private void SpawnDevilHeadGO()
    {
        Debug.Log("spawnTrans:" + spawnTrans.position.y);
        devilHeadGO=Instantiate(goPrefab, spawnTrans);
        devilHeadGO.transform.parent = null;

        magicCirleTrans = spawnTrans;
        Vector3 tmpVec3 = magicCirleTrans.position;
        tmpVec3.y = 0;
        magicCirleTrans.position = tmpVec3;
        Instantiate(magicCircle, magicCirleTrans);   
    }
}
