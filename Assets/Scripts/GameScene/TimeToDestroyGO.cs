using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeToDestroyGO : MonoBehaviour
{
    public float time;

    // Start is called before the first frame update
    void Start()
    {
        //参数检查
        if(time==0)
        {
            Debug.Log(GetType() + "TimeToDestroyGO/Start()/time没有赋值或者为0！");
        }
    }

    // Update is called once per frame
    void Update()
    {
        time -= 1*Time.deltaTime;
        if(time<=0)
        {
            Object.Destroy(this.gameObject);
        }
    }
}
