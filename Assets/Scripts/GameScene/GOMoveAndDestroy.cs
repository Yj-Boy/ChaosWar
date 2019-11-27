/*
 *  对象移动并在一定时间后自动销毁脚本 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOMoveAndDestroy : MonoBehaviour
{
    public float speed;             //对象移动速度
    public float liveTime;          //对象存在时间

    private Vector3 position;       //对象的初始位置

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
