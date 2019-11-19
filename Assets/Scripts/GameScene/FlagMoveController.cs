/*
 *  吹动旗帜随机摆动 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagMoveController : MonoBehaviour
{
    public Cloth cloth;         //cloth组件，该组件可让旗帜飘动

    // Start is called before the first frame update
    void Start()
    {
        //参数检查
        if(cloth==null)
        {
            Debug.Log(GetType() + "/Start()/旗帜的Cloth没指定！");
        }

        InvokeRepeating("FlagMove", 0f, 5f);
    }

    private void FlagMove()
    {
        //百分之三十的概率吹动旗帜
        if(Random.Range(0,100)<=30)
        {
            cloth.useGravity = true;
        }
        else
        {
            cloth.useGravity = false;
        }
    }
    
}
