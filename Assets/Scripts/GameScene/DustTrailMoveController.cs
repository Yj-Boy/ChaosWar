/*
 *  大招的烟尘跟进效果 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustTrailMoveController : MonoBehaviour
{
    public float speed;             //烟尘跟进速度
    public float liveTime;          //烟尘存在时间

    private Vector3 position;       //烟尘生成位置（大招尾部）

    private void Start()
    {
        position = transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        position.z += speed * Time.deltaTime;
        transform.position = position;

        liveTime -= Time.deltaTime;
        if(liveTime<=0)
        {
            Object.Destroy(this.gameObject);
        }
    }
}
